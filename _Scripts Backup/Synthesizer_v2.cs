using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Synthesizer_v2 : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{ 
    private GameObject button;
    private char[] alphabet = "CDEFGAB".ToCharArray();
    private double[] freqLib = {29.3678, 32.0392, 35.9628, 19.6006, 42.7672, 48.0046, 53.8833};
    // 440.0, 493.8833, 523.2511, 587.3295, 659.2551};

    public GameObject MChordButton;
    public AudioSource audio_source;
    public List<GameObject> buttons = new List<GameObject>();
    public KeyCode key;
    public GameObject prefab;
    public GameObject _parent;
    public GameObject text;

    // public GameObject button.GetComponentInParent<Canvas>
    public bool isSelected = false;
    public int noteIndex = 0;
    public double frequency = 440.0;
    public double phase_reset;
    private double increment;
    public double phase;
    private double sampling_frequency = 48000.0;

    public float gain;

    //variables for rhytmic features
    public double sampleA;
    public double samplesPerTick;
    public double nextTick = 0.0f;
    public double bpm = 70.0f;
    public GameObject Metronome;
    public int score = 0;

    void Start()
    {
        MChordButton = GameObject.Find("MChord");
        audio_source = GetComponent<AudioSource> ();
        audio_source.mute = true;
        audio_source.Play();
        Button button = GetComponent<Button> (); 
        buttons.Add(gameObject);
        GenerateButtons();
        if(Metronome==null){
            Metronome = GameObject.Find("Metronome");
        }
    }

    void GenerateButtons()
    {
        if (prefab!=null)
        {
            int noteNum = 1;
            int octaveNum = 4;
            double currentFreq = 261.6256;
            for(int i = 0; i < 14; i++)
                {   
                    noteNum = (noteNum>6) ? 0:noteNum;
                    currentFreq += freqLib[(noteNum)]*(octaveNum-3);
                    octaveNum = (noteNum==0) ? octaveNum+1:octaveNum;
                    GameObject nextKey = Instantiate(prefab, new Vector3((i+1)*170-960,300,0), Quaternion.identity) as GameObject;
                    // Note names and octave numbers:
                    // nextKey.GetComponentInChildren<Text>().text = alphabet[noteNum].ToString() + octaveNum.ToString();
                    // Note names only:
                    nextKey.GetComponentInChildren<Text>().text = alphabet[noteNum].ToString();
                    // Note and octave numbers:
                    // nextKey.GetComponentInChildren<Text>().text = (noteNum).ToString() + octaveNum.ToString();
                    nextKey.transform.SetParent(_parent.transform, false);
                    nextKey.GetComponent<Synthesizer_v2>().frequency = currentFreq;
                    nextKey.GetComponent<Synthesizer_v2>().noteIndex = i+1;
                    buttons.Add(nextKey);
                    noteNum += 1;
                }
            }
    }

    void HighlightButton()
    {
        buttons[2].GetComponent<Image>().color = Color.red;
    }

    void Update () {
         if (Input.GetKeyDown(key)) {
             audio_source.Play();
         }else if (Input.GetKeyUp(key)) {
             audio_source.Stop();
         }
         nextTick = Metronome.GetComponent<Metronome>().nextTick;
        //  if(text!=null) text.GetComponent<Text>().text = scoreTotal.ToString();
    }
    
    public void OnPointerDown(PointerEventData eventData){
        audio_source.mute = !audio_source.mute;
        // void MChord.HighlightButton(noteIndex); Can I do sth like that?
        MChordButton.GetComponent<MChord>().HighlightButton(noteIndex);
        double timePressed = sampleA;
        if(nextTick - timePressed > samplesPerTick/2)
        {
            //the player is late
            score = Mathf.RoundToInt((float) ((samplesPerTick/2 - (timePressed - (nextTick - samplesPerTick)))*bpm*bpm/10000));
            Metronome.GetComponent<Metronome>().scoreTotal += score;
        }else if(nextTick - timePressed <= samplesPerTick/2){
            //the player is early
            score =  Mathf.RoundToInt((float) ((samplesPerTick/2 - (nextTick - timePressed))*bpm*bpm/10000));
            Metronome.GetComponent<Metronome>().scoreTotal += score;
        }else{
            Debug.LogWarning("Unknown error");
        }
    }

    public void OnPointerUp(PointerEventData eventData){
        audio_source.mute = !audio_source.mute;
    }

    public void PlayNote(float freq)
    {
        frequency = (double) freq;
        audio_source.Play ();
    }

    void OnAudioFilterRead(float[] data, int channels)
    {
        samplesPerTick = sampling_frequency * 60.0F / bpm * 4.0F / 4;
        sampleA = AudioSettings.dspTime * sampling_frequency;

        increment = frequency * 2.0 * Mathf.PI / sampling_frequency;

        for (int i = 0; i < data.Length; i += channels)
        {
            phase += increment;
            data[i] = (float) (gain * Mathf.Sin((float)phase));

            if(channels == 2)
            {
                data[i + 1] = data[i];
            }

            if (phase > (Mathf.PI * 2))
            {
                phase = 0.0;
            }
        }
    }
}
