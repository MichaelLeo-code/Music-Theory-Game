using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Synthesizer_v2_BlackKeys : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{ 
    private GameObject button;
    private char[] alphabet = "CDFGA".ToCharArray();
    private char[] alphabetB = "DEGAB".ToCharArray();
    private double[] freqLib = {88.2015, 33.9444, 58.8674, 45.3103, 50.8591};
    // 440.0, 493.8833, 523.2511, 587.3295, 659.2551};

    public GameObject MChordButton;
    public AudioSource audio_source;
    public List<GameObject> buttons = new List<GameObject>();
    public KeyCode key;
    public GameObject prefab;
    public GameObject _parent;

    // public GameObject button.GetComponentInParent<Canvas>
    public bool isSelected = false;
    public int noteIndex = 0;
    public double frequency = 440.0;
    public double phase_reset;
    private double increment;
    public double phase;
    private double sampling_frequency = 48000.0;
    
    public GameObject Ask;

    public float gain;

    void Start()
    {
        MChordButton = GameObject.Find("MChord");
        audio_source = GetComponent<AudioSource> ();
        audio_source.mute = true;
        audio_source.Play();
        Button button = GetComponent<Button> (); 
        buttons.Add(gameObject);
        GenerateButtons();
    }

    void GenerateButtons()
    {
        if (prefab!=null)
        {
            int noteNum = 1;
            int octaveNum = 4;
            double currentFreq = 277.1826;
            for(int i = 0; i < 14; i++)
                {
                    noteNum = (noteNum>4) ? 0:noteNum;
                    currentFreq += freqLib[(noteNum)]*(octaveNum-3);
                    octaveNum = (noteNum==0) ? octaveNum+1:octaveNum;
                    GameObject nextKey;
                    if(noteNum==2 || noteNum==0)
                    {
                        i+=1;
                    }
                    nextKey = Instantiate(prefab, new Vector3((i+1)*170+205,400,0), Quaternion.identity) as GameObject;
                    // Note names and octave numbers:
                    // nextKey.GetComponentInChildren<Text>().text = alphabet[noteNum].ToString() + octaveNum.ToString();
                    // Note names only:
                    nextKey.GetComponentInChildren<Text>().text = alphabet[noteNum].ToString() + "#" + "\n" + alphabetB[noteNum].ToString() + "b";
                    // Note and octave numbers:
                    // nextKey.GetComponentInChildren<Text>().text = (noteNum).ToString() + octaveNum.ToString();
                    nextKey.transform.SetParent(_parent.transform, false);
                    nextKey.GetComponent<Synthesizer_v2_BlackKeys>().frequency = currentFreq;
                    nextKey.GetComponent<Synthesizer_v2_BlackKeys>().noteIndex = noteNum + (octaveNum-4)*5;
                    buttons.Add(nextKey);
                    noteNum += 1;
                }
            }
            if(Ask!=null) Ask.GetComponent<AskChord>().GenerateButtonsLoc();
    }

    void HighlightButton()
    {
        buttons[2].GetComponent<Image>().color = Color.red;
    }

    void Update () {
         if (Input.GetKeyDown(key)) {
             audio_source.Play();
         } else if (Input.GetKeyUp(key)) {
             audio_source.Stop();
         }
    }
    
    public void OnPointerDown(PointerEventData eventData){
        audio_source.mute = !audio_source.mute;
        // void MChord.HighlightButton(noteIndex); Can I do sth like that?
        MChordButton.GetComponent<MChord>().HighlightButtonBlack(noteIndex);
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
