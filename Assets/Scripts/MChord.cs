using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MChord : MonoBehaviour
{
    private bool isSelected;
    private List<GameObject> buttonsLoc;
    // private List<GameObject> buttonsLocWhite;
    // private List<GameObject> buttonsLocBlack;
    private Color highlightColor = new Color(1,1,0);

    public bool isClicked = false;
    public bool MCHORDisPressable = true;
    public bool PLAYisPressable = true;
    public int index;
    public GameObject C4Button;
    public GameObject PlayMChordButton;
    public GameObject MChord_Image;
    public GameObject Play_Image;
    public List<GameObject> selectedButtons = new List<GameObject>();
    public List<int> selectedButtonNumbers = new List<int>();
    public List<int> selectedButtonNames = new List<int>();

    public Sprite white;
    public Sprite green;

    // public List<GameObject> selectedButtonsBlack = new List<GameObject>();

    void Start()
    {
        C4Button = gameObject.GetComponentInParent<Memory_Slots_Manager>().C4Button;
        buttonsLoc = C4Button.GetComponent<Synthesizer_v2>().buttons_all_in_order;
        // gameObject.GetComponentInChildren<Text>().text = "Slot " + (index+1);
        gameObject.GetComponent<Button>().onClick.AddListener(Clicked);
        // buttonsLocBlack = CSharpButton.GetComponent<Synthesizer_v2_BlackKeys>().buttons;
    }
    
    void Clicked()
    {
        if(MCHORDisPressable)
        {
            GameObject.Find("Memory Slots").GetComponent<Memory_Slots_Manager>().OneOfAKind(index);
            // isClicked = !isClicked;
            // gameObject.GetComponent<Image>().color = (isClicked) ? Color.red :Color.white;
            MChord_Image.GetComponent<Image>().sprite = (isClicked) ? green : white;
            foreach(GameObject button in selectedButtons)
            {
                if(isClicked)
                {
                    highlightColor = Color.yellow;
                }else if(button.GetComponent<Synthesizer_v2>().isBlack)
                {
                    highlightColor = Color.black;
                }else
                {
                    highlightColor = Color.white;
                }
                // highlightColor = (isClicked) ? Color.yellow : Color.white;
                button.GetComponent<Image>().color = highlightColor;
                button.GetComponent<Synthesizer_v2>().isSelected = !button.GetComponent<Synthesizer_v2>().isSelected;
            }
        }  
    }



    public void HighlightButton(int buttonIndex, int buttonName)
    {
        if(isClicked)
        {
            if(buttonsLoc[buttonIndex].GetComponent<Synthesizer_v2>().isSelected)
            {
                if(buttonsLoc[buttonIndex].GetComponent<Synthesizer_v2>().isBlack)
                {
                    highlightColor = Color.black;
                }else
                {
                    highlightColor = Color.white;
                }
                buttonsLoc[buttonIndex].GetComponent<Image>().color = highlightColor;
                selectedButtons.Remove(buttonsLoc[buttonIndex]);
                selectedButtonNumbers.Remove(buttonIndex);
                selectedButtonNames.Remove(buttonName);
                // Debug.Log(buttonIndex);
            }else{
                highlightColor = Color.yellow;
                buttonsLoc[buttonIndex].GetComponent<Image>().color = highlightColor;
                selectedButtons.Add(buttonsLoc[buttonIndex]);
                selectedButtonNumbers.Add(buttonIndex);
                // Debug.Log(buttonsLoc[buttonIndex].GetComponent<Synthesizer_v2>().isSelected);
                selectedButtonNames.Add(buttonName);
            }
            buttonsLoc[buttonIndex].GetComponent<Synthesizer_v2>().isSelected = !buttonsLoc[buttonIndex].GetComponent<Synthesizer_v2>().isSelected;
        }
    }

    public void Clear_All()
    {
        foreach(GameObject button in buttonsLoc)
        {
            if(button.GetComponent<Synthesizer_v2>().isBlack)
            {
                highlightColor = Color.black;
            }else
            {
                highlightColor = Color.white;
            }
            button.GetComponent<Image>().color = highlightColor;
            button.GetComponent<Synthesizer_v2>().isSelected = false;
            // CheckButton.GetComponent<Image>().color = Color.white;
        }
        selectedButtons.Clear();
        selectedButtonNames.Clear();
        selectedButtonNumbers.Clear();
    }

    public void Remove_Number(int number)
    {
        if(selectedButtons[number].GetComponent<Synthesizer_v2>().isBlack)
        {
            highlightColor = Color.black;
        }else
        {
            highlightColor = Color.white;
        }
        selectedButtons[number].GetComponent<Image>().color = highlightColor;
        selectedButtons[number].GetComponent<Synthesizer_v2>().isSelected = false;
            // CheckButton.GetComponent<Image>().color = Color.white;
        selectedButtonNames.Remove(selectedButtonNames[number]);
        selectedButtonNumbers.Remove(selectedButtonNumbers[number]);
        selectedButtons.Remove(selectedButtons[number]);
    }
}
