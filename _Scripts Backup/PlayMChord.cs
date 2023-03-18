using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayMChord : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public GameObject MChordButton;
    
    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Down");
        foreach(GameObject button in MChordButton.GetComponent<MChord>().selectedButtons)
        {
            button.GetComponent<Synthesizer_v2>().audio_source.mute = false;
            button.GetComponent<Image>().color = Color.yellow;
        }
        foreach(GameObject button in MChordButton.GetComponent<MChord>().selectedButtonsBlack)
        {
            button.GetComponent<Synthesizer_v2_BlackKeys>().audio_source.mute = false;
            button.GetComponent<Image>().color = Color.yellow;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("Up");
        foreach(GameObject button in MChordButton.GetComponent<MChord>().selectedButtons)
        {
            button.GetComponent<Synthesizer_v2>().audio_source.mute = true;
        }
        foreach(GameObject button in MChordButton.GetComponent<MChord>().selectedButtonsBlack)
        {
            button.GetComponent<Synthesizer_v2_BlackKeys>().audio_source.mute = true;
        }
        if(!MChordButton.GetComponent<MChord>().isClicked)
        {
            foreach(GameObject button in MChordButton.GetComponent<MChord>().selectedButtons)
            {
            button.GetComponent<Image>().color = Color.white;
            }
            foreach(GameObject button in MChordButton.GetComponent<MChord>().selectedButtonsBlack)
            {
            button.GetComponent<Image>().color = Color.black;
            }
        }
    }
}
