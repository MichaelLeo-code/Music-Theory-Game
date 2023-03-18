using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Synthesizer_v2 : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{ 
    private GameObject button;
    private char[] alphabet = "CDEFGAB".ToCharArray();
    private char[] alphabetB = "CDFGA".ToCharArray();
    private char[] alphabetB2 = "DEGAB".ToCharArray();
    private double[] freqLibW = {29.3678, 32.0392, 35.9628, 19.6006, 42.7672, 48.0046, 53.8833};
    private double[] freqLibB = {88.2015, 33.9444, 58.8674, 45.3103, 50.8591};
    // 440.0, 493.8833, 523.2511, 587.3295, 659.2551};

    public GameObject MChordManager;
    public AudioSource audio_source;
    public List<GameObject> buttons = new List<GameObject>();
    public List<GameObject> buttons_all_in_order = new List<GameObject>();
    public KeyCode key;
    public GameObject prefabW;
    public GameObject prefabB;
    public GameObject _parent;
    public GameObject text;
    public GameObject CSharpButton;
    public GameObject IntervalHighlighter;

    // public GameObject button.GetComponentInParent<Canvas>
    public int NotesToInsenitate_quantity;
    public int Starting_octave;
    public int NoteName;
    public bool isSelected = false;
    public int noteIndex = 0;
    public bool isBlack = false;
    public double frequency;
    public double phase_reset;
    private double increment;
    public double phase;
    private double sampling_frequency = 48000.0;

    public float gain;
    public int testInt = 0;

    //variables for rhytmic features
    public double sampleA;
    public double samplesPerTick;
    public double nextTick = 0.0f;
    public double bpm = 70.0f;
    public GameObject Metronome;
    public int score = 0;

    void Start()
    {
        MChordManager = GameObject.Find("Memory Slots");
        IntervalHighlighter = GameObject.Find("Interval Highlighter");
        audio_source = GetComponent<AudioSource> ();
        audio_source.mute = true;
        audio_source.Play();
        Button button = GetComponent<Button> (); 
        buttons.Add(gameObject);
        if(Metronome==null){
            Metronome = GameObject.Find("Metronome");
        }
        if(prefabW!=null)
        {
            GenerateButtons();
            GenerateButtonsBlack();
            GenerateButtonsWithBlack();
        }
    }

    void GenerateButtons()
    {
        if (prefabW!=null)
        {
            gameObject.GetComponentInChildren<Text>().text = "C";
            int noteNum = 1;
            int noteNumLooped = 2;
            int octaveNum = Starting_octave;
            // double currentFreq = 261.6256;
            double currentFreq = frequency;
            int noteIndexLoc = 2;
            for(int i = 0; i < NotesToInsenitate_quantity; i++)
            {   
                noteNum = (noteNum>6) ? 0:noteNum;
                noteNumLooped = (noteNumLooped>11) ? 0:noteNumLooped;
                if(octaveNum==2)
                {
                    currentFreq += freqLibW[(noteNum)]/(4);
                }else if(octaveNum==3)
                {
                    currentFreq += freqLibW[(noteNum)]/(2);
                }else if(octaveNum==6)
                {
                    currentFreq += freqLibW[(noteNum)]*(4);
                }else
                {
                    currentFreq += freqLibW[(noteNum)]*(octaveNum-3);
                }
                octaveNum = (noteNum==0) ? octaveNum+1:octaveNum;
                GameObject nextKey = Instantiate(prefabW, new Vector3((i+1)*170+120,300,0), Quaternion.identity) as GameObject;
                // Note names and octave numbers:
                // nextKey.GetComponentInChildren<Text>().text = alphabet[noteNum].ToString() + octaveNum.ToString();
                // Note names only:

                // if((i-(octaveNum-Starting_octave)*7)==(-1)) //If the note is C, add octave number
                // {
                //     nextKey.GetComponentInChildren<Text>().text = alphabet[noteNum].ToString() + octaveNum.ToString();
                // }else
                // {
                    nextKey.GetComponentInChildren<Text>().text = alphabet[noteNum].ToString();
                // }

                // Note and octave numbers:                    // nextKey.GetComponentInChildren<Text>().text = (noteNum).ToString() + octaveNum.ToString();
                nextKey.transform.SetParent(_parent.transform, false);
                nextKey.transform.SetAsFirstSibling();
                nextKey.GetComponent<Synthesizer_v2>().frequency = currentFreq;
                nextKey.GetComponent<Synthesizer_v2>().NoteName = noteNumLooped;
                if((i-(octaveNum-Starting_octave)*7)==1 || (i-(octaveNum-Starting_octave)*7)==5)
                {
                    nextKey.GetComponent<Synthesizer_v2>().noteIndex = noteIndexLoc;
                    noteIndexLoc ++;
                    noteNumLooped ++;
                }else
                {
                    nextKey.GetComponent<Synthesizer_v2>().noteIndex = noteIndexLoc;
                    noteIndexLoc += 2;
                    noteNumLooped +=2;
                }
                buttons.Add(nextKey);
                noteNum ++;
            }
        }
    }

    void GenerateButtonsBlack()
    {
        buttons.Add(CSharpButton);
        CSharpButton.GetComponent<Synthesizer_v2>().noteIndex = 1;
        CSharpButton.GetComponent<Synthesizer_v2>().isBlack = true;
        CSharpButton.GetComponent<Synthesizer_v2>().NoteName = 1;
        if (prefabB!=null)
        {
            int noteNum = 1;
            int noteNumLooped = 3;
            int octaveNum = Starting_octave;
            double currentFreq;
            switch (octaveNum)
            {
                case 2:
                currentFreq = 69.29566;
                break;

                case 3:
                currentFreq = 138.5913;
                break;

                default:
                currentFreq = 277.1826;
                break;

                case 5:
                currentFreq = 554.3653;
                break;
            }
            int noteIndexLoc = 3;
            for(int i = 0; i < NotesToInsenitate_quantity-2; i++)
            {
                noteNum = (noteNum>4) ? 0:noteNum;
                noteNumLooped = (noteNumLooped>10) ? 1:noteNumLooped;
                if(octaveNum==2)
                {
                    currentFreq += freqLibB[(noteNum)]/(4);
                }else if(octaveNum==3)
                {
                    currentFreq += freqLibB[(noteNum)]/(2);
                }else if(octaveNum==6)
                {
                    currentFreq += freqLibB[(noteNum)]*(4);
                }else
                {
                    currentFreq += freqLibB[(noteNum)]*(octaveNum-3);
                }
                octaveNum = (noteNum==0) ? octaveNum+1:octaveNum;
                GameObject nextKey;
                if(noteNum==2 || noteNum==0)
                {
                    i+=1;
                }
                nextKey = Instantiate(prefabB, new Vector3((i+1)*170+205,400,0), Quaternion.identity) as GameObject;
                // Note names and octave numbers:
                // nextKey.GetComponentInChildren<Text>().text = alphabet[noteNum].ToString() + octaveNum.ToString();
                // Note names only:
                nextKey.GetComponentInChildren<Text>().text = alphabetB[noteNum].ToString() + "#" + "\n" + alphabetB2[noteNum].ToString() + "b";
                // Note and octave numbers:
                // nextKey.GetComponentInChildren<Text>().text = (noteNum).ToString() + octaveNum.ToString();
                nextKey.GetComponent<Synthesizer_v2>().NoteName = noteNum+7;
                nextKey.transform.SetParent(_parent.transform, false);
                nextKey.GetComponent<Synthesizer_v2>().frequency = currentFreq;
                nextKey.GetComponent<Synthesizer_v2>().isBlack = true;
                nextKey.GetComponent<Synthesizer_v2>().NoteName = noteNumLooped;
                if(noteNum==4 || noteNum==1)
                {
                    nextKey.GetComponent<Synthesizer_v2>().noteIndex = noteIndexLoc;
                    noteIndexLoc += 3;
                    noteNumLooped += 3;
                }else
                {
                    nextKey.GetComponent<Synthesizer_v2>().noteIndex = noteIndexLoc;
                    noteIndexLoc += 2;
                    noteNumLooped += 2;
                }
                // nextKey.GetComponent<Synthesizer_v2>().noteIndex = 15 + noteNum + (octaveNum-4)*5;
                buttons.Add(nextKey);
                noteNum ++;
            }
        }
    }

    void GenerateButtonsWithBlack()
    {
        int whiteKeyNo = 0;
        int blackKeyNo = NotesToInsenitate_quantity+1;
        int countCircles = 0;
        for(int i = 0; i < NotesToInsenitate_quantity; i++)
        {   
            if(countCircles==2)
            {
                buttons_all_in_order.Add(gameObject.GetComponent<Synthesizer_v2>().buttons[whiteKeyNo]);
                whiteKeyNo++;
                countCircles++;
            }else if(countCircles==6){
                buttons_all_in_order.Add(gameObject.GetComponent<Synthesizer_v2>().buttons[whiteKeyNo]);
                countCircles = 0;
                whiteKeyNo++;
            }else{
                buttons_all_in_order.Add(gameObject.GetComponent<Synthesizer_v2>().buttons[whiteKeyNo]);
                buttons_all_in_order.Add(gameObject.GetComponent<Synthesizer_v2>().buttons[blackKeyNo]);
                blackKeyNo++;
                whiteKeyNo++;
                countCircles++;
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
         if(Metronome!=null) nextTick = Metronome.GetComponent<Metronome>().nextTick;
        //  if(text!=null) text.GetComponent<Text>().text = scoreTotal.ToString();
    }
    
    public void OnPointerDown(PointerEventData eventData){
        audio_source.mute = !audio_source.mute;
        if(MChordManager!=null)
        {
            MChordManager.GetComponent<Memory_Slots_Manager>().HighlightButtonManager(noteIndex, NoteName);
        }else if(IntervalHighlighter!=null)
        {
            IntervalHighlighter.GetComponent<Intervals_Highlight>().HighlightButton(noteIndex, NoteName);
        }
        double timePressed = sampleA;
        if(nextTick - timePressed > samplesPerTick/2)
        {
            //the player is late
            score = Mathf.RoundToInt((float) ((samplesPerTick/2 - (timePressed - (nextTick - samplesPerTick)))*bpm*bpm/10000));
            if(Metronome) Metronome.GetComponent<Metronome>().scoreTotal += score;
        }else if(nextTick - timePressed <= samplesPerTick/2){
            //the player is early
            score =  Mathf.RoundToInt((float) ((samplesPerTick/2 - (nextTick - timePressed))*bpm*bpm/10000));
            if(Metronome) Metronome.GetComponent<Metronome>().scoreTotal += score;
        }else{
            Debug.LogWarning("Unknown error");
        }
        
        // gameObject.GetComponent<Synthesizer_v2>().buttons[noteIndex].GetComponent<Image>().color = Color.cyan;
        Debug.Log(NoteName);
    }

    public void OnPointerUp(PointerEventData eventData){
        audio_source.mute = !audio_source.mute;
    }

    public void PlayNote(float freq)
    {
        frequency = (double) freq;
        audio_source.Play();
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
