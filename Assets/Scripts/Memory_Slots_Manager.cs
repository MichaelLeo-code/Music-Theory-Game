using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Memory_Slots_Manager : MonoBehaviour
{
    public GameObject[] MChordButtons;
    public GameObject[] MChord_Images;
    public GameObject[] Play_Images;
    public GameObject C4Button;

    public Sprite white;

    private int indexLoc = 0;
    private Color highlightColor = new Color(1,1,0);

    public void OneOfAKind(int index)
    {
        indexLoc = index;
        bool isClicked = MChordButtons[index].GetComponent<MChord>().isClicked;
        bool anyButtonIsClicked = false;
        foreach(GameObject MChordButton in MChordButtons)
        {
            if(MChordButton.GetComponent<MChord>().isClicked) anyButtonIsClicked = true;
            if(MChordButtons[index].GetComponent<MChord>().isClicked) anyButtonIsClicked = false;
            MChordButton.GetComponent<MChord>().isClicked = false;
            // MChordButton.GetComponent<Image>().color = Color.white;
        }
        foreach(GameObject MChord_Image in MChord_Images) MChord_Image.GetComponent<Image>().sprite = white;
        Debug.Log("WTF");
        foreach(GameObject MChord_Image in Play_Images) MChord_Image.GetComponent<Image>().sprite = white;
        MChordButtons[index].GetComponent<MChord>().isClicked = !isClicked;
        if(anyButtonIsClicked)
        {
            foreach(GameObject button in C4Button.GetComponent<Synthesizer_v2>().buttons_all_in_order)
            {
                if(button.GetComponent<Synthesizer_v2>().isBlack)
                {
                    highlightColor = Color.black;
                }else
                {
                    highlightColor = Color.white;
                }
                // highlightColor = (isClicked) ? Color.yellow : Color.white;
                button.GetComponent<Image>().color = highlightColor;
                button.GetComponent<Synthesizer_v2>().isSelected = false;
            }
        }
    }

    public void HighlightButtonManager(int buttonIndex, int buttonName)
    {
        if(MChordButtons[indexLoc].GetComponent<MChord>().isClicked)
        {
            MChordButtons[indexLoc].GetComponent<MChord>().HighlightButton(buttonIndex, buttonName);
        }
    }

    void Start()
    {
        if(MChordButtons.Length==5)
        {
            foreach(GameObject button in MChord_Images)
            {
                button.GetComponent<Image>().sprite = white;
            }
            foreach(GameObject button in Play_Images)
            {
                button.GetComponent<Image>().sprite = white;
            }
        }
    }
}
