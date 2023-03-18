using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MChord : MonoBehaviour
{
    private bool isSelected;
    private List<GameObject> buttonsLocWhite;
    private List<GameObject> buttonsLocBlack;
    private Color highlightColor = new Color(1,1,0);

    public bool isClicked = false;
    public GameObject C4Button;
    public GameObject CSharpButton;
    public GameObject PlayMChordButton;
    public List<GameObject> selectedButtons = new List<GameObject>();
    public List<GameObject> selectedButtonsBlack = new List<GameObject>();

    void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(Clicked);
        buttonsLocWhite = C4Button.GetComponent<Synthesizer_v2>().buttons;
        buttonsLocBlack = CSharpButton.GetComponent<Synthesizer_v2_BlackKeys>().buttons;
    }
    
    void Clicked()
    {
        isClicked = !isClicked;
        gameObject.GetComponent<Image>().color = (isClicked) ? Color.red:Color.white;
        foreach(GameObject button in selectedButtons)
        {
            highlightColor = (isClicked) ? Color.yellow : Color.white;
            button.GetComponent<Image>().color = highlightColor;
        }
        foreach(GameObject button in selectedButtonsBlack)
        {
            highlightColor = (isClicked) ? Color.yellow : Color.black;
            button.GetComponent<Image>().color = highlightColor;
        }
    }



    public void HighlightButton(int buttonIndex)
    {
        if(isClicked)
        {
            if(buttonsLocWhite[buttonIndex].GetComponent<Synthesizer_v2>().isSelected)
            {
                highlightColor = Color.white;
                buttonsLocWhite[buttonIndex].GetComponent<Image>().color = highlightColor;
                selectedButtons.Remove(buttonsLocWhite[buttonIndex]);
                Debug.Log(buttonIndex);
            }else{
                highlightColor = Color.yellow;
                buttonsLocWhite[buttonIndex].GetComponent<Image>().color = highlightColor;
                selectedButtons.Add(buttonsLocWhite[buttonIndex]);
                Debug.Log(buttonsLocWhite[buttonIndex].GetComponent<Synthesizer_v2>().isSelected);
            }
            buttonsLocWhite[buttonIndex].GetComponent<Synthesizer_v2>().isSelected = !buttonsLocWhite[buttonIndex].GetComponent<Synthesizer_v2>().isSelected;
        }
    }

    public void HighlightButtonBlack(int buttonIndex)
    {
        if(isClicked)
        {
            if(buttonsLocBlack[buttonIndex].GetComponent<Synthesizer_v2_BlackKeys>().isSelected)
            {
                highlightColor = Color.black;
                buttonsLocBlack[buttonIndex].GetComponent<Image>().color = highlightColor;
                selectedButtonsBlack.Remove(buttonsLocBlack[buttonIndex]);
                Debug.Log(buttonIndex);
            }else{
                highlightColor = Color.yellow;
                buttonsLocBlack[buttonIndex].GetComponent<Image>().color = highlightColor;
                selectedButtonsBlack.Add(buttonsLocBlack[buttonIndex]);
                Debug.Log(buttonsLocBlack[buttonIndex].GetComponent<Synthesizer_v2_BlackKeys>().isSelected);
            }
            buttonsLocBlack[buttonIndex].GetComponent<Synthesizer_v2_BlackKeys>().isSelected = !buttonsLocBlack[buttonIndex].GetComponent<Synthesizer_v2_BlackKeys>().isSelected;
        }
    }
}
