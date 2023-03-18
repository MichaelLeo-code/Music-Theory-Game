using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Piano_Key : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public int NoteName;    //0 to 11
    public int NoteIndex;   //0 to infinity
    public double frequency;

    public int fade = 0;

    // For the OnAudioFilterRead
    private double increment;
    public double phase;
    private double sampling_frequency = 48000.0;
    public float gain;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // fade = 1;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // gain = 0;
        // fade = 2;
    }

    // Update is called once per frame
    void Update()
    {
        // if(fade)
        // {
        //     if(gain > 0) gain -= 0.01f;
        //     else{
        //         gain = 0;
        //         fade = false;
        //     }
        // }
    }

    public float descent;

    void OnAudioFilterRead(float[] data, int channels)
    {
        increment = frequency * 2.0 * Mathf.PI / sampling_frequency;

        for (int i = 0; i < data.Length; i += channels)
        {
            phase += increment;
            if(gain>0 && fade==2) gain -= 0.00001f;
            else if(gain<0.1f && fade==1) gain += 0.0001f;
            data[i] = (float) (gain * Mathf.Sin((float)phase));

            if(channels == 2)
            {
                data[i + 1] = data[i];
            }

            if (phase > (Mathf.PI * 2))
            {
                phase = 0;
            }
        }
    }
}
