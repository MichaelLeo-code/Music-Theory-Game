using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Metronome : MonoBehaviour
{
    public GameObject C4Button;
    public GameObject text;
    public int scoreTotal;

    public float ampPlus = 0.999F;
    public float phasePlus = 0.3F;

    public double bpm = 70.0F;
    public float gain = 0.5F;
    public int signatureHi = 4;
    public int signatureLo = 4;

    public double nextTick = 0.0F;
    public double samplesPerTick;
    public double sample;

    public float amp = 0.0F;
    private float phase = 0.0F;
    private double sampleRate = 0.0F;
    private int accent;
    private bool running = false;
    void Start()
    {
        accent = signatureHi;
        double startTick = AudioSettings.dspTime;
        sampleRate = AudioSettings.outputSampleRate;
        nextTick = startTick * sampleRate;
        running = true;
    }

    void Update()
    {
        if(text!=null) text.GetComponent<Text>().text = scoreTotal.ToString();
    }

    void OnAudioFilterRead(float[] data, int channels)
    {
        if (!running)
            return;

        samplesPerTick = sampleRate * 60.0F / bpm * 4.0F / signatureLo;
        sample = AudioSettings.dspTime * sampleRate;
        int dataLen = data.Length / channels;
        int n = 0;
        while (n < dataLen)
        {
            float x = gain * amp * Mathf.Sin(phase);
            int i = 0;
            while (i < channels)
            {
                data[n * channels + i] += x;
                i++;
            }
            while (sample + n >= nextTick)
            {
                nextTick += samplesPerTick;
                amp = 1.0F;
                if (++accent > signatureHi)
                {
                    accent = 1;
                    amp *= 2.0F;
                }
                // Debug.Log("Tick: " + accent + "/" + signatureHi);
            }
            phase += amp * phasePlus;
            amp *= ampPlus;
            n++;
        }
    }

    public IEnumerator playNote(List<int> notes, List<double> durations)
    {   
        double timePressed = nextTick;
        yield return new WaitUntil(() => nextTick > timePressed);
        int i = 0;
        foreach(int note in notes)
        {
            double timeLoopStarted = sample;
            C4Button.GetComponent<Synthesizer_v2>().buttons[note].GetComponent<Image>().color = Color.blue;
            C4Button.GetComponent<Synthesizer_v2>().buttons[note].GetComponent<Synthesizer_v2>().audio_source.mute = false;
            Debug.Log(nextTick-samplesPerTick-timeLoopStarted);
            yield return new WaitUntil(() => timeLoopStarted < sample-0.5*samplesPerTick*durations[i]);
            double timeLoop2 = sample;
            C4Button.GetComponent<Synthesizer_v2>().buttons[note].GetComponent<Image>().color = Color.white;
            C4Button.GetComponent<Synthesizer_v2>().buttons[note].GetComponent<Synthesizer_v2>().audio_source.mute = true;
            Debug.Log(timeLoop2 + " vs " + (nextTick-samplesPerTick) + " = " + (nextTick-samplesPerTick-timeLoop2));
            yield return new WaitUntil(() => timeLoop2 + 640 < nextTick-samplesPerTick);
            i++;
            // while((nextTick - duration*samplesPerTick) > sample)
            // {
            // note.GetComponent<Synthesizer_v2>().audio_source.mute = false;
            // }
            // note.GetComponent<Synthesizer_v2>().audio_source.mute = true;
        }
        GameObject.Find("PlaySample").GetComponent<PlaySample>().track1playing = false;
    }
}
