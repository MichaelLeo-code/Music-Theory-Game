using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AskChord : MonoBehaviour, IPointerDownHandler
{
    public List<GameObject> buttonsLoc = new List<GameObject>();
    public List<int> notes = new List<int>();
    public GameObject C4Button;
    public GameObject CSharpButton;

    private char[] alphabet = "CDEFGAB".ToCharArray();
    private string[] typeName =  new string[5]{"Major", "Minor", "Major 7", "7", "Minor 7"};
    
    public int typeNo;
    public int noteNo;
    private int blackKeyNo = 0;
    private int whiteKeyNo = 0;
    private int countCircles = 0;

    public void OnPointerDown(PointerEventData eventData)
    {
        notes.Clear();
        foreach(GameObject note in buttonsLoc){
            note.GetComponent<Image>().color = Color.white;
        }
        typeNo = Random.Range(0,4);
        noteNo = Random.Range(0,6);
        GameObject.Find("Speech_bubble").GetComponent<Text>().text = alphabet[noteNo].ToString() + " " + typeName[typeNo].ToString();
        switch (typeNo)
        {
            case 0:
            notes.AddRange(new int[] {noteNo,noteNo+4,noteNo+7});
            break;

            case 1:
            notes.AddRange(new int[] {noteNo,noteNo+3,noteNo+7});
            break;

            case 2:
            notes.AddRange(new int[] {noteNo,noteNo+4,noteNo+7, noteNo+11});
            break;

            case 3:
            notes.AddRange(new int[] {noteNo,noteNo+4,noteNo+7, noteNo+10});
            break;

            case 4:
            notes.AddRange(new int[] {noteNo,noteNo+3,noteNo+7, noteNo+10});
            break;
        }
        IEnumerator coroutine =  gameObject.GetComponent<AskChord>().playNotes(notes);
        StartCoroutine(coroutine);
    }
    
    public void GenerateButtonsLoc()
    {
        for(int i = 0; i < 12; i++)
        {   
            if(countCircles==2)
            {
                buttonsLoc.Add(C4Button.GetComponent<Synthesizer_v2>().buttons[whiteKeyNo]);
                whiteKeyNo++;
                countCircles++;
            }else if(countCircles==6){
                buttonsLoc.Add(C4Button.GetComponent<Synthesizer_v2>().buttons[whiteKeyNo]);
                countCircles = 0;
                whiteKeyNo++;
            }else{
                buttonsLoc.Add(C4Button.GetComponent<Synthesizer_v2>().buttons[whiteKeyNo]);
                buttonsLoc.Add(CSharpButton.GetComponent<Synthesizer_v2_BlackKeys>().buttons[blackKeyNo]);
                blackKeyNo++;
                whiteKeyNo++;
                countCircles++;
            }
        }
    }

    IEnumerator playNotes(List<int> notes)
    {
        foreach(int note in notes){
            buttonsLoc[note].GetComponent<Image>().color = Color.blue;
            yield return new WaitForSeconds(0.3f);
        }
    }
}
