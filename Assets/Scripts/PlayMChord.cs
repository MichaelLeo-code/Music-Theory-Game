using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;

public class PlayMChord : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public GameObject MChordButton;
    public GameObject AskButton;
    public GameObject CButton;

    private int Starting_octave;
    public bool isPressed = false;
    
    void Start()
    {
        Starting_octave = CButton.GetComponent<Synthesizer_v2>().Starting_octave;
        AskButton = GameObject.Find("Ask");
    }

    public void OnPointerDown(PointerEventData eventData)
    {   
        isPressed = true;
        // Debug.Log("Down");
        if(MChordButton.GetComponent<MChord>().PLAYisPressable)
        {
            MChordButton.GetComponent<MChord>().Play_Image.GetComponent<Image>().sprite = MChordButton.GetComponent<MChord>().green;
            foreach(GameObject button in MChordButton.GetComponent<MChord>().selectedButtons)
            {
                button.GetComponent<Synthesizer_v2>().audio_source.mute = false;
                button.GetComponent<Image>().color = Color.yellow;
            }
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPressed = false;
        // Debug.Log("Up");
        if(MChordButton.GetComponent<MChord>().PLAYisPressable)
        {
            MChordButton.GetComponent<MChord>().Play_Image.GetComponent<Image>().sprite = MChordButton.GetComponent<MChord>().white;
            foreach(GameObject button in MChordButton.GetComponent<MChord>().selectedButtons)
            {
                button.GetComponent<Synthesizer_v2>().audio_source.mute = true;
            }
            if(!MChordButton.GetComponent<MChord>().isClicked)
            {
                foreach(GameObject button in MChordButton.GetComponent<MChord>().selectedButtons)
                {
                    if(button.GetComponent<Synthesizer_v2>().isSelected)
                    {
                        button.GetComponent<Image>().color = Color.yellow;
                    }else if(button.GetComponent<Synthesizer_v2>().isBlack)
                    {
                        button.GetComponent<Image>().color = Color.black;
                    }else
                    {
                        button.GetComponent<Image>().color = Color.white;
                    }
                }
            }
        }
        // bool isEqual = false;
        // for(int octaveNum = Starting_octave; octaveNum < 6; octaveNum++)
        // {
        //     isEqual = Enumerable.SequenceEqual(MChordButton.GetComponent<MChord>().selectedButtonNumbers.OrderBy(e => e), AskButton.GetComponent<AskChord>().notes.OrderBy(e => e));
        //     if(isEqual==false)
        //     {
        //         List<int> notesLoc = new List<int>();
        //         foreach(int note in AskButton.GetComponent<AskChord>().notes)
        //         {
        //             notesLoc.Add(note+12);
        //         }
        //         isEqual = Enumerable.SequenceEqual(MChordButton.GetComponent<MChord>().selectedButtonNumbers.OrderBy(e => e), notesLoc.OrderBy(e => e));
        //     }
        // }

        // CHECK CHORD
        // bool isEqual = Enumerable.SequenceEqual(MChordButton.GetComponent<MChord>().selectedButtonNames.OrderBy(e => e), AskButton.GetComponent<AskChord>().notes.OrderBy(e => e));
        // Debug.Log(isEqual);
    }
}
