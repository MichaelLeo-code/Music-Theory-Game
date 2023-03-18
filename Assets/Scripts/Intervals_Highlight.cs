using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Intervals_Highlight : MonoBehaviour
{
    private bool isSelected;
    private int alpha = 0;
    private List<GameObject> buttonsLoc;
    private string[] Congratulation_texts = new string[4]{"PERFECT", "SUPERB", "SPLENDID!", "WONDERFUL"};
    // private List<GameObject> buttonsLocWhite;
    // private List<GameObject> buttonsLocBlack;
    private Color highlightColor = new Color(1,1,0);

    public bool isClicked = false;
    public bool hint = false;
    public bool correct = true;
    public GameObject C4Button;
    public GameObject MessageManager;
    public GameObject CheckButton;
    public GameObject animator;
    public List<GameObject> selectedButtons = new List<GameObject>();
    public List<int> selectedButtonNumbers = new List<int>(); //0 to infinity
    public List<int> selectedButtonNames = new List<int>(); // 0 to 11
    public List<int> notesChord = new List<int>();

    public GameObject Animation_button;

    public List<int> scaleNeededButtons = new List<int>();
    // public List<GameObject> selectedButtonsBlack = new List<GameObject>();

    void Start()
    {
        buttonsLoc = C4Button.GetComponent<Synthesizer_v2>().buttons_all_in_order;
        // buttonsLocBlack = CSharpButton.GetComponent<Synthesizer_v2_BlackKeys>().buttons;
    }
    
    // void Clicked()
    // {
    //     // isClicked = !isClicked;
    //     foreach(GameObject button in selectedButtons)
    //     {
    //         if(isClicked)
    //         {
    //             highlightColor = Color.yellow;
    //         }else if(button.GetComponent<Synthesizer_v2>().isBlack)
    //         {
    //             highlightColor = Color.black;
    //         }else
    //         {
    //             highlightColor = Color.white;
    //         }
    //         // highlightColor = (isClicked) ? Color.yellow : Color.white;
    //         button.GetComponent<Image>().color = highlightColor;
    //         button.GetComponent<Synthesizer_v2>().isSelected = !button.GetComponent<Synthesizer_v2>().isSelected;
    //     }
    // }



    public void HighlightButton(int buttonIndex, int buttonName)
    {
        if(isClicked)
        {   
            if(selectedButtons.Count==1 && MessageManager.GetComponent<Message_Manager>().LevelType==2)
            {
               Clear_All();
            }
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

    public void CheckInterval()
    {
        if(isClicked)
        {
            if(selectedButtonNumbers.Count != 2) CheckButton.GetComponent<Image>().color = Color.red;
            if(MessageManager.GetComponent<Message_Manager>().intervalName+1 == (selectedButtonNumbers[1]-selectedButtonNumbers[0]))
            {
                // CheckButton.GetComponent<Image>().color = Color.white;
                Debug.Log("true");
                correct = true;
                animator.GetComponent<Text>().text = Congratulation_texts[UnityEngine.Random.Range(0,4)];
                Animation_button.GetComponent<Animator>().SetTrigger("Green");
                animator.GetComponent<Animator>().SetTrigger("Congratulate");
                MessageManager.GetComponent<Message_Manager>().ChooseDialog();
            }else{
                // CheckButton.GetComponent<Image>().color = Color.red;
                Animation_button.GetComponent<Animator>().SetTrigger("Red");
                Debug.Log("false");
                correct = false;
                // Debug.Log(MessageManager.GetComponent<Message_Manager>().intervalName);
                // Debug.Log(Math.Abs(selectedButtonNumbers[0]-selectedButtonNumbers[1]));
            }
            // MessageManager.GetComponent<Message_Manager>().intervalName
        }
    }

    public void CheckNote()
    {
        if(isClicked)
        {
            if(selectedButtonNumbers.Count != 1) CheckButton.GetComponent<Image>().color = Color.red;
            if(MessageManager.GetComponent<Message_Manager>().scaleName == selectedButtonNames[0])
            {
                // CheckButton.GetComponent<Image>().color = Color.white;
                Debug.Log("true");
                correct = true;
                alpha = 0;
                animator.GetComponent<Text>().text = Congratulation_texts[UnityEngine.Random.Range(0,4)];
                animator.GetComponent<Animator>().SetTrigger("Congratulate");
                Animation_button.GetComponent<Animator>().SetTrigger("Green");
                MessageManager.GetComponent<Message_Manager>().ChooseDialog();
            }else{
                // CheckButton.GetComponent<Image>().color = Color.red;
                Debug.Log("false");
                Animation_button.GetComponent<Animator>().SetTrigger("Red");
                correct = false;
                if(hint)
                {
                    if(alpha<250) alpha = alpha + 10;
                }
                // Debug.Log(MessageManager.GetComponent<Message_Manager>().intervalName);
                // Debug.Log(Math.Abs(selectedButtonNumbers[0]-selectedButtonNumbers[1]));
            }
            if(hint){
                int[] buttonsToHint = new int[5]{0, 2, 4, 5, 7};
                foreach(int i in buttonsToHint)
                    {
                        C4Button.GetComponent<Synthesizer_v2>().buttons_all_in_order[i].GetComponentInChildren<Text>().color =
                        new Color32(0,0,0, (byte) alpha);
                    }
            }
            // MessageManager.GetComponent<Message_Manager>().intervalName
        }
    }

    public void CheckScales()
    {
        if(selectedButtonNumbers.Count != 8) CheckButton.GetComponent<Image>().color = Color.red;
        scaleNeededButtons.Clear();
        scaleNeededButtons.AddRange(MessageManager.GetComponent<Message_Manager>().minor ? new int[] {0, 2, 3, 5, 7, 8, 10, 0} : new int[] {0, 2, 4, 5, 7, 9, 11, 0});
        bool isEqual = Enumerable.SequenceEqual(scaleNeededButtons.OrderBy(e => e), selectedButtonNames.OrderBy(e => e));
        if(isEqual)
        {
            CheckButton.GetComponent<Image>().color = Color.green;
            Debug.Log("true");
            MessageManager.GetComponent<Message_Manager>().ChooseDialog();
        }else{
            CheckButton.GetComponent<Image>().color = Color.red;
            Debug.Log("false");
            // Debug.Log(MessageManager.GetComponent<Message_Manager>().intervalName);
            // Debug.Log(Math.Abs(selectedButtonNumbers[0]-selectedButtonNumbers[1]));
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
                    selectedButtons.Clear();
                    selectedButtonNames.Clear();
                    selectedButtonNumbers.Clear();
                    // CheckButton.GetComponent<Image>().color = Color.white;
                }
    }

    public void CheckChords(int typeNo, int noteNo)
    {
        notesChord.Clear();
        switch (typeNo)
        {
            case 0:
            notesChord.AddRange(new int[] {noteNo, addInterval(4, noteNo), addInterval(7, noteNo)});
            break;

            case 1:
            notesChord.AddRange(new int[] {noteNo,noteNo+3,noteNo+7});
            break;

            case 2:
            notesChord.AddRange(new int[] {noteNo,noteNo+4,noteNo+7, noteNo+11});
            break;

            case 3:
            notesChord.AddRange(new int[] {noteNo,noteNo+4,noteNo+7, noteNo+10});
            break;

            case 4:
            notesChord.AddRange(new int[] {noteNo,noteNo+3,noteNo+7, noteNo+10});
            break;
        }
    }

    private int addInterval(int interval, int noteNo)
    {
        int intervalCut;
        if((noteNo+interval)>11)
        {
            intervalCut = noteNo + interval - 12;
        }else
        {
            intervalCut = noteNo + interval;
        }
        return intervalCut;
    }
}
