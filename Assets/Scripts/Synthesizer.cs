using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Synthesizer : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{ 
    private GameObject button;
    private AudioSource audio_source;

    public KeyCode key;
    public double frequency = 440.0;
    public double phase_reset;
    private double increment;
    public double phase;
    private double sampling_frequency = 48000.0;

    public float gain;

    void Start()
    {
        audio_source = GetComponent<AudioSource> ();
        Button button = GetComponent<Button> ();
    }

    void Update () {
         if (Input.GetKeyDown(key)) {
             audio_source.Play();
         } else if (Input.GetKeyUp(key)) {
             audio_source.Stop();
         }
    }
    
    public void OnPointerDown(PointerEventData eventData){
        audio_source.Play();
    }

    public void OnPointerUp(PointerEventData eventData){
        audio_source.Stop();
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
