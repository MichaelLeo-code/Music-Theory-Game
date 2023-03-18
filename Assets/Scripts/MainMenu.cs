using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject Buttons_Layer_2;
    public GameObject Buttons_Layer_3;

    public GameObject Button_Notes;
    public GameObject Button_Intervals;
    public GameObject Button_Chords;

    public GameObject Button_Theory;
    public GameObject Button_Practise;
    public GameObject Button_EarTrainer;

    public Sprite white;
    public Sprite green;
    public Sprite red;
    public Sprite black;

    private int buttonNo;

    // public void Add_Side_Menu_Layer_2()
    // {
    //     transform.localPosition = new Vector3(-300, 0, 0);
    //     Buttons_Layer_2.SetActive(true);
    //     Buttons_Layer_2.transform.localPosition = new Vector3(300, 0, 0);
    // }

    // public void Add_Side_Menu_Layer_3(int buttonNoLoc)
    // {
    //     transform.localPosition = new Vector3(-600, 0, 0);
    //     Buttons_Layer_2.transform.localPosition = new Vector3(0, 0, 0);
    //     Buttons_Layer_3.SetActive(true);
    //     Buttons_Layer_3.transform.localPosition = new Vector3(600, 0, 0);
    //     buttonNo = buttonNoLoc;
    // }

    public void ChooseAction(int actionNo)
    {
        if(actionNo==1)
        {
            SceneManager.LoadScene(buttonNo);
            Static.practise = false;
        }else if(actionNo==2)
        {
            SceneManager.LoadScene(buttonNo);
            Static.practise = true;
        }else if(actionNo==3)
        {
            SceneManager.LoadScene(4);
        }else if(actionNo==6)
        {
            SceneManager.LoadScene(6);
        }
    }

    public void HighlightGreen(GameObject image)
    {
        Debug.Log("chage");
        image.GetComponent<Image>().sprite = green;
        Debug.Log("chage");
    }

    public void HighlightWhite_1()
    {
        GameObject.Find("Image_Notes").GetComponent<Image>().sprite = white;
        GameObject.Find("Image_Intervals").GetComponent<Image>().sprite = white;
        GameObject.Find("Image_Chords").GetComponent<Image>().sprite = white;
        Button_Notes.SetActive(true);
        Button_Intervals.SetActive(true);
        Button_Chords.SetActive(true);
    }

    public void HighlightWhite_2(int buttonNoLoc)
    {
        GameObject.Find("Image_Theory").GetComponent<Image>().sprite = white;
        if(buttonNoLoc!=1) GameObject.Find("Image_Practise").GetComponent<Image>().sprite = white;
        else GameObject.Find("Image_Practise").GetComponent<Image>().sprite = black;
        // GameObject.Find("Image_EarTrainer").GetComponent<Image>().sprite = white;
        GameObject.Find("Image_Notes").GetComponent<Image>().sprite = white;
        GameObject.Find("Image_Intervals").GetComponent<Image>().sprite = white;
        GameObject.Find("Image_Chords").GetComponent<Image>().sprite = white;
        Button_Theory.SetActive(true);
        if(buttonNoLoc!=1) Button_Practise.SetActive(true);
        else Button_Practise.SetActive(false);
        Button_EarTrainer.SetActive(true);
        buttonNo = buttonNoLoc;
    }
}
