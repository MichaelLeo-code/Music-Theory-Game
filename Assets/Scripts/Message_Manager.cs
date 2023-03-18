using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;

public class Message_Manager : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private bool pointerDown = false;
    private bool isPressable = true;
    private bool talk = true;
    
    private float timeValue = 10.00f;
    private bool timerStart = false;
    private int correctNoteCounter = 0;

    private int tempMessageNum;
    private int switchRound = 0;
    string tempText;

    private float pointerDownTimer = 0.0f;
    private float talkTime = 0.75f;
    public int message_Num = 0;
    public int LevelType;
    public GameObject CButton;
    public GameObject Interval_manager;
    public GameObject MChord;
    private GameObject Animation_text;
    public GameObject Animation_button;
    public GameObject Cat;
    public GameObject canvas;
    public GameObject ps;
    public GameObject animator_Piano;

    //Variables for chords' generation
    private string[] typeName =  new string[5]{"Major", "Minor", "Major 7", "7", "Minor 7"};
    public List<int> notes = new List<int>();
    public int typeNo;

    private string[] intervalNames = new string[12]{"Minor 2nd", "Major 2nd", "Minor 3rd",
    "Major 3nd", "Perfect 4th", "Augmented 4th / Diminished 5th", "Perfect 5th",
    "Minor 6th", "Major 6th", "Minor 7th",
    "Major 7th", "Octave"};
    private string[] scaleNames = new string[12]{"C", "C#", "D", "D#", "E",
    "F", "F#", "G", "G#", "A", "A# ", "B"};
    private string[] Congratulation_texts = new string[4]{"PERFECT", "SUPERB", "SPLENDID!", "WONDERFUL"};
    public int scaleName;
    public bool minor;
    public int intervalName;
    private int previousName;
    private bool Bemol = false;
    // private string[] typeName =  new string[5]{"Major", "Minor", "Major 7", "7", "Minor 7"};
    // Start is called before the first frame update
    void Start()
    {
        if(MChord!=null)
        {
            MChord.GetComponent<MChord>().MCHORDisPressable = false;
            MChord.GetComponent<MChord>().PLAYisPressable = false;
        }
        if(MChord!=null)
        {
            Animation_text = GameObject.Find("Congratulations_Text");
        }
        if (Static.practise) message_Num = 28; 
        gameObject.GetComponentInChildren<Text>().text = "Touch this text and hold it for a while to see next message";
        // gameObject.GetComponent<Button>().onClick.AddListener(Clicked);
        if (LevelType==2) foreach(GameObject button in CButton.GetComponent<Synthesizer_v2>().buttons_all_in_order)
        {
            button.GetComponentInChildren<Text>().color = new Color32(0,0,0,0);
        }
    }

    public void OnPointerDown(PointerEventData eventData) {
        pointerDown = true;
    }

    public void OnPointerUp(PointerEventData eventData) {
        pointerDown = false;
        pointerDownTimer = 0;
    }

    void Update()
	{
        if(MChord!=null)
        {
            if(MChord.GetComponent<MChord>().PLAYisPressable) Animation_button.GetComponent<Animator>().SetBool("isOn", true);
            else Animation_button.GetComponent<Animator>().SetBool("isOn", false);
        }

        gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(1000, canvas.GetComponent<RectTransform>().rect.height+75);
        if(isPressable) ps.SetActive(true);
        else ps.SetActive(false);

		if(pointerDown && isPressable)
		{
			pointerDownTimer += Time.deltaTime;
			if (pointerDownTimer >= 0.5F)
			{
                pointerDown = false;
				ChooseDialog();
			}
		}
        if(timerStart)
		{
			timeValue -= Time.deltaTime;
            gameObject.GetComponentInChildren<Text>().text = scaleNames[scaleName] + " " + ((Mathf.Floor(timeValue))).ToString() + "." + (Mathf.Floor((timeValue*10)%10)).ToString() + (Mathf.Floor((timeValue*100)%10)).ToString();
            if (Interval_manager.GetComponent<Intervals_Highlight>().selectedButtons.Count != 0)
			{
                Interval_manager.GetComponent<Intervals_Highlight>().CheckNote();
                Interval_manager.GetComponent<Intervals_Highlight>().Clear_All();
                if(!Interval_manager.GetComponent<Intervals_Highlight>().correct)
                {
                    gameObject.GetComponentInChildren<Text>().text = "You lost, try again";
                    timerStart = false;
                    timeValue = 10.0f;
                    isPressable = true;
                    message_Num = 33;
                    return;
                }
                correctNoteCounter++;
                message_Num = 35;
                Clicked_Notes();
			}
			if (timeValue <= 0.0F)
			{
                gameObject.GetComponentInChildren<Text>().text = "You lost, try again";
                timerStart = false;
                message_Num = 33;
                isPressable = true;
                timeValue = 10f;
			}else if(correctNoteCounter==10)
            {
                gameObject.GetComponentInChildren<Text>().text = "You won, congratulations! Your time: " + (10 - (Mathf.Round(timeValue*100))/100).ToString();
                timerStart = false;
                message_Num = 36;
                isPressable = true;
                timeValue = 10f;
            }
		}
	}

    public void ChooseDialog()
    {
        switch(LevelType)
                {
                    case 0:
                    Clicked_Scales();
                    break;

                    case 1:
                    Clicked_Intervals();
                    break;

                    case 2:
                    Clicked_Notes();
                    break;

                    case 3:
                    Clicked_Chords();
                    break;
                }
    }

    void Clicked_Scales()
    {
        switch(message_Num)
        {
            case 0:
            gameObject.GetComponentInChildren<Text>().text = "I can see how much eagernees you have.";
            break;

            case 1:
            gameObject.GetComponentInChildren<Text>().text = "Today we are learining scales on piano.";
            break;

            // case > 1:
            case int n when (n >= 2):
            previousName = scaleName;
            scaleName = Random.Range(0,12);
            while(previousName==scaleName){
                scaleName = Random.Range(0,12);
                Debug.Log("Again");
            }
            minor = Random.Range(0, 2) == 1;
            gameObject.GetComponentInChildren<Text>().text = scaleNames[scaleName] + (minor ? " Minor":" Major");
            break;
        }
        Debug.Log(message_Num + "!!");
        message_Num++;
    }

    void Clicked_Intervals()
    {
        StartCoroutine(Wait2Sec(talkTime));
        switch(message_Num)
        {
            case 0:
            gameObject.GetComponentInChildren<Text>().text = "That's good.";
            break;

            case 1:
            gameObject.GetComponentInChildren<Text>().text = "Interval is the distance in pitch between any two notes.";
            break;

            case 2:
            gameObject.GetComponentInChildren<Text>().text = "For example, if you play C and then G, it would be Perfect Fifth interval!";
            break;

            case 3:
            gameObject.GetComponentInChildren<Text>().text = "I won't bother you too much with them today.";
            break;

            case 4:
            gameObject.GetComponentInChildren<Text>().text = "The only three intervals we would need yet are Minor and Major Thirds, as well Perfect Fifth";
            break;

            case 5:
            gameObject.GetComponentInChildren<Text>().text = "But first we have to find out what's half step. It's simply the next key on your keyboard";
            break;

            case 6:
            gameObject.GetComponentInChildren<Text>().text = "For example C and C#, E and F, B and C, A and Bb";
            break;

            case 7:
            talk = false;
            gameObject.GetComponentInChildren<Text>().text = "Let's do a very quick practise round. I highlight the note, you press the tone that's half step up";
            animator_Piano.GetComponent<Animator>().SetBool("Up", true);
            break;

            case 8:  
            Interval_manager.GetComponent<Intervals_Highlight>().Clear_All();
            Debug.Log("Clear");
            isPressable = false;
            Interval_manager.GetComponent<Intervals_Highlight>().isClicked = true;
            previousName = scaleName-1;
            scaleName = Random.Range(0,12);
            while(previousName==scaleName){
                scaleName = Random.Range(0,12);
                Debug.Log("Again");
            }
            CButton.GetComponent<Synthesizer_v2>().buttons_all_in_order[scaleName].GetComponent<Image>().color = new Color32(153,175,255,255);
            Debug.Log("Color changed");
            if (scaleName==11) scaleName = 0;
            else scaleName++;
            Debug.Log("Scale name is " + scaleName);
            StartCoroutine(Next());
            message_Num--;
            break;

            case 9:
            talk = true;
            StopAllCoroutines();
            StartCoroutine(Wait2Sec(talkTime));
            Interval_manager.GetComponent<Intervals_Highlight>().Clear_All();
            isPressable = true;
            Interval_manager.GetComponent<Intervals_Highlight>().isClicked = false;
            gameObject.GetComponentInChildren<Text>().text = "Now a whole step is 2 half steps";
            break;

            case 10:
            gameObject.GetComponentInChildren<Text>().text = "Now, Perfect Fifth consists of 7 half steps or 3.5 whole steps";
            break;

            case 11:
            gameObject.GetComponentInChildren<Text>().text = "Try to count it, from any note. Select the Perfect Fifth interval and I'll tell you whether you are correct";
            intervalName = 6;
            isPressable = false;
            Interval_manager.GetComponent<Intervals_Highlight>().isClicked = true;
            talk = false;
            StartCoroutine(Next());
            break;

            case 12:
            Cat.GetComponent<Animator>().SetTrigger("Ex");
            talk = true;
            isPressable = true;
            Interval_manager.GetComponent<Intervals_Highlight>().isClicked = false;
            gameObject.GetComponentInChildren<Text>().text = "You're so great!";
            Interval_manager.GetComponent<Intervals_Highlight>().Clear_All();
            message_Num++;
            break;
            
            case 13:
            gameObject.GetComponentInChildren<Text>().text = "No, try once again";
            Interval_manager.GetComponent<Intervals_Highlight>().Clear_All();
            message_Num=11;
            correctNoteCounter = 0;
            StartCoroutine(Next());
            break;

            case 14:
            gameObject.GetComponentInChildren<Text>().text = "Now I name the note and you build a Perfect Fifth interval from it. Shall we start?";
            break;

            case 15:
            intervalName = 6;
            previousName = scaleName;
            scaleName = Random.Range(0,12);
            while(previousName==scaleName){
                scaleName = Random.Range(0,12);
                Debug.Log("Again");
            }
            gameObject.GetComponentInChildren<Text>().text = scaleNames[scaleName];
            isPressable = false;
            Interval_manager.GetComponent<Intervals_Highlight>().Clear_All();
            Interval_manager.GetComponent<Intervals_Highlight>().isClicked = true;
            message_Num--;
            StartCoroutine(Next());
            break;

            case 16:
            Interval_manager.GetComponent<Intervals_Highlight>().Clear_All();
            isPressable = true;
            Interval_manager.GetComponent<Intervals_Highlight>().isClicked = false;
            gameObject.GetComponentInChildren<Text>().text = "Awesome!";
            animator_Piano.GetComponent<Animator>().SetBool("Up", false);
            break;

            case 17:
            gameObject.GetComponentInChildren<Text>().text =
            "The Major and Minor Third intervals differ a bit: they have a \"sound quality\"";
            break;

            case 18:
            gameObject.GetComponentInChildren<Text>().text =
            "It means that you can kind of feel these two.";
            break;

            case 19:
            gameObject.GetComponentInChildren<Text>().text =
            "The minor interval sounds sad and melancholic, and major sounds like a happy one.";
            break;

            case 20:
            gameObject.GetComponentInChildren<Text>().text =
            "Minor Third consists of 3 half steps or 1.5 whole steps";
            break;

            case 21:
            gameObject.GetComponentInChildren<Text>().text =
            "Major Third consists of 4 half steps or 2 whole steps";
            break;

            case 22:
            gameObject.GetComponentInChildren<Text>().text = "Ought'a practise them!";
            animator_Piano.GetComponent<Animator>().SetBool("Up", true);
            break;

            case 23:
            gameObject.GetComponentInChildren<Text>().text = "I name the note and the interval, you build it.";
            correctNoteCounter = 0;
            talk = false;
            break;

            case 24:
            Interval_manager.GetComponent<Intervals_Highlight>().isClicked = true;
            isPressable = false;
            intervalName = Random.Range(2, 4);
            previousName = scaleName;
            scaleName = Random.Range(0,12);
            while(previousName==scaleName){
                scaleName = Random.Range(0,12);
                Debug.Log("Again");
            }
            gameObject.GetComponentInChildren<Text>().text = intervalNames[intervalName] + " starting from note " + scaleNames[scaleName];
            Interval_manager.GetComponent<Intervals_Highlight>().Clear_All();
            Interval_manager.GetComponent<Intervals_Highlight>().isClicked = true;
            message_Num--;
            StartCoroutine(Next());
            break;

            case 25:
            Interval_manager.GetComponent<Intervals_Highlight>().Clear_All();
            Interval_manager.GetComponent<Intervals_Highlight>().isClicked = false;
            isPressable = true;
            gameObject.GetComponentInChildren<Text>().text = "That's about it. There're other intervals, but we won't need them at this moment.";
            break;

            case 26:
            gameObject.GetComponentInChildren<Text>().text =
            "You can come back here if you would like to revise your theory, or you can go straight to learning chords or to the practise level";
            break;

            case 27:
            StartCoroutine(Wait2Sec_Blink(talkTime));
            gameObject.GetComponentInChildren<Text>().text =
            "See you around!";
            break;

            case 28:
            talk = true;
            animator_Piano.GetComponent<Animator>().SetBool("Up", true);
            Interval_manager.GetComponent<Intervals_Highlight>().isClicked = false;
            isPressable = true;
            gameObject.GetComponentInChildren<Text>().text = "Practise time!";
            break;

            case 29:
            Interval_manager.GetComponent<Intervals_Highlight>().isClicked = true;
            isPressable = false;
            intervalName = Random.Range(2, 5);
            if(intervalName == 4) intervalName = 6;
            previousName = scaleName;
            scaleName = Random.Range(0,12);
            while(previousName==scaleName){
                scaleName = Random.Range(0,12);
                Debug.Log("Again");
            }
            gameObject.GetComponentInChildren<Text>().text = intervalNames[intervalName] + " starting from note " + scaleNames[scaleName];
            Interval_manager.GetComponent<Intervals_Highlight>().Clear_All();
            Interval_manager.GetComponent<Intervals_Highlight>().isClicked = true;
            message_Num--;
            StartCoroutine(Next());
            break;

            // case > 1:
            case int n when (n >= 2):
            previousName = intervalName;
            intervalName = Random.Range(0,12);
            while(previousName==intervalName){
                intervalName = Random.Range(0,12);
                Debug.Log("Again");
            }
            gameObject.GetComponentInChildren<Text>().text = intervalNames[intervalName];
            break;
        }
        Debug.Log(message_Num + "!!");
        message_Num++;
    }

    void Clicked_Notes()
    {
        switch(message_Num)
        {
            case 0:
            gameObject.GetComponentInChildren<Text>().text = "Cool, you learn fast!";
            break;

            case 1:
            gameObject.GetComponentInChildren<Text>().text = "Let's start from the very beginning";
            break;

            case 2:
            gameObject.GetComponentInChildren<Text>().text = "It helps a lot to know the notes you are playing";
            break;

            case 3:
            gameObject.GetComponentInChildren<Text>().text = "So that's what we are doing today - learning note names!";
            break;

            case 4:
            gameObject.GetComponentInChildren<Text>().text = "At this moment we will deal with white notes only";
            break;

            case 5:
            gameObject.GetComponentInChildren<Text>().text = "You need to learn their positions, and black notes are here to help you locate white notes.";
            break;

            case 6:
            animator_Piano.GetComponent<Animator>().SetBool("Up", true);
            gameObject.GetComponentInChildren<Text>().text = "The first note we will learn is C note";
            // foreach(GameObject button in CButton.GetComponent<Synthesizer_v2>().buttons_all_in_order)
            // {
            //         button.GetComponentInChildren<Text>().color = new Color32(50,50,50,255);
            // }
            CButton.GetComponent<Synthesizer_v2>().buttons_all_in_order[0].GetComponentInChildren<Text>().color = new Color32(50,50,50,255);
            CButton.GetComponent<Synthesizer_v2>().buttons_all_in_order[0].GetComponentInChildren<Text>().text = "C";
            CButton.GetComponent<Synthesizer_v2>().buttons_all_in_order[0].GetComponent<Image>().color = new Color32(153,175,255,255);
            break;

            case 7:
            Interval_manager.GetComponent<Intervals_Highlight>().isClicked = true;
            CButton.GetComponent<Synthesizer_v2>().buttons_all_in_order[0].GetComponent<Image>().color = Color.white;
            gameObject.GetComponentInChildren<Text>().text = "Now you try - press that C note, and then press \"Check\" button.";
            isPressable = false;
            break;

            case 8:
            Interval_manager.GetComponent<Intervals_Highlight>().isClicked = false;
            Interval_manager.GetComponent<Intervals_Highlight>().Clear_All();
            int[] buttons = new int[4]{2, 4, 5, 7};
            foreach(int i in buttons){
                CButton.GetComponent<Synthesizer_v2>().buttons_all_in_order[i].GetComponent<Image>().color = new Color32(153,175,255,255);
                CButton.GetComponent<Synthesizer_v2>().buttons_all_in_order[i].GetComponentInChildren<Text>().color = new Color32(50,50,50,255);
            }
            gameObject.GetComponentInChildren<Text>().text = "The following notes go just like the alphabet does - after C we have D, E, F and G";
            isPressable = true;
            break;

            case 9:
            buttons = new int[4]{2, 4, 5, 7};
            foreach(int i in buttons){
                CButton.GetComponent<Synthesizer_v2>().buttons_all_in_order[i].GetComponent<Image>().color = new Color32(255,255,255,255);
            }
            gameObject.GetComponentInChildren<Text>().text = "Let's practise them";
            break;

            case 17:
            isPressable = true;
            Interval_manager.GetComponent<Intervals_Highlight>().Clear_All();
            Interval_manager.GetComponent<Intervals_Highlight>().isClicked = false;
            buttons = new int[5]{0, 2, 4, 5, 7};
            foreach(int i in buttons){
                CButton.GetComponent<Synthesizer_v2>().buttons_all_in_order[i].GetComponentInChildren<Text>().color = new Color32(0,0,0,0);
            }
            gameObject.GetComponentInChildren<Text>().text =
            "The task is a bit harder now. The names won't be shown. Don't worry, if you'll forget something, we'll give you a hint";
            Interval_manager.GetComponent<Intervals_Highlight>().hint = true;
            break;

            //Practise CDEFG
            case int n when (n >= 10 && n<=30):
            Interval_manager.GetComponent<Intervals_Highlight>().isClicked = true;
            isPressable = false;
            int[] notesLoc = new int[5]{0, 2, 4, 5, 7};
            previousName = scaleName;
            scaleName = notesLoc[Random.Range(0,5)];
            while(previousName==scaleName){
                scaleName = notesLoc[Random.Range(0,5)];
                Debug.Log("Again");
            }
            gameObject.GetComponentInChildren<Text>().text = scaleNames[scaleName];
            break;

            case 31:
            Interval_manager.GetComponent<Intervals_Highlight>().Clear_All();
            gameObject.GetComponentInChildren<Text>().text = "Do you feel like you are ready for the test? \n Press C note if yes or on any other note if not";
            StartCoroutine(Next());
            break;

            case 32:
            isPressable = true;
            Interval_manager.GetComponent<Intervals_Highlight>().isClicked = false;
            Interval_manager.GetComponent<Intervals_Highlight>().Clear_All();
            gameObject.GetComponentInChildren<Text>().text = "Allright, let's practise a bit more";
            message_Num = 18;
            break;

            case 33:
            isPressable = true;
            Interval_manager.GetComponent<Intervals_Highlight>().hint = false;
            gameObject.GetComponentInChildren<Text>().text =
            ">:)\nYou'll have to press 10 notes correctly in 10 seconds. You won't have to press \"Check\" button every time, but you'll have no chance for mistake";
            break;

            case 34:
            Interval_manager.GetComponent<Intervals_Highlight>().Clear_All();
            Interval_manager.GetComponent<Intervals_Highlight>().CheckButton.GetComponent<Image>().color = Color.white;
            gameObject.GetComponentInChildren<Text>().text = "10.00";
            Interval_manager.GetComponent<Intervals_Highlight>().animator.GetComponent<Text>().text = "Ready";
            Interval_manager.GetComponent<Intervals_Highlight>().animator.GetComponent<Animator>().SetTrigger("Ready");
            int countICycles = 0;
            IEnumerator WaitSec(string text){
                yield return new WaitForSeconds(1f);
                Interval_manager.GetComponent<Intervals_Highlight>().animator.GetComponent<Text>().text = text;
                countICycles++;
                if(countICycles<2) StartCoroutine(WaitSec("Go!"));
                if(countICycles==2){
                    yield return new WaitForSeconds(1f);
                    timerStart = true;
                    Clicked_Notes();
                }
            }
            StartCoroutine(WaitSec("Set"));
            break;

            case 35:
            // gameObject.GetComponentInChildren<Text>().text = (10-Time.deltaTime).ToString();
            Debug.Log("in switch 35");
            Interval_manager.GetComponent<Intervals_Highlight>().isClicked = true;
            isPressable = false;
            notesLoc = new int[5]{0, 2, 4, 5, 7};
            previousName = scaleName;
            scaleName = notesLoc[Random.Range(0,5)];
            while(previousName==scaleName){
                scaleName = notesLoc[Random.Range(0,5)];
                Debug.Log("Again");
            }
            break;

            case 36:

            break;

            // case > 1:
            case int n when (n >= 7):
            previousName = scaleName;
            scaleName = Random.Range(0,12);
            while(previousName==scaleName){
                scaleName = Random.Range(0,12);
                Debug.Log("Again");
            }
            if(scaleName == 1 || scaleName == 3 || scaleName == 6 || scaleName == 8 || scaleName == 10)
            {
                string nameLoc = "error";
                Debug.Log("if clause");
                Bemol = Random.Range(0, 2) == 1;
                if(Bemol)
                {
                    nameLoc = (scaleNames[scaleName+1] + "b");
                    gameObject.GetComponentInChildren<Text>().text = nameLoc;
                }else{
                    gameObject.GetComponentInChildren<Text>().text = scaleNames[scaleName];
                }
            }else{
                gameObject.GetComponentInChildren<Text>().text = scaleNames[scaleName];
            }
            break;
        }
        Debug.Log(message_Num + "!!");
        message_Num++;
    }

    void Clicked_Chords()
    {
        switch(message_Num)
        {
            case 0:
            gameObject.GetComponentInChildren<Text>().text = "I can see how much eagernees you have.";
            break;

            case 1:
            gameObject.GetComponentInChildren<Text>().text = "The last thing we are gonna learn are chords";
            break;

            case 2:
            gameObject.GetComponentInChildren<Text>().text = "Can you imagine yourself beeing able to play any chord you want on piano, huh?";
            break;

            case 3:
            gameObject.GetComponentInChildren<Text>().text = "We are gonnna learn the most common chords - major and minor triads.";
            break;

            case 4:
            gameObject.GetComponentInChildren<Text>().text = "They are called triads for a reason: these chords consist of 3 notes.";
            break;

            case 5:
            gameObject.GetComponentInChildren<Text>().text = "And three notes are nothing else but two intervals :)";
            break;

            case 6:
            gameObject.GetComponentInChildren<Text>().text = "You see, we were learning intervals not in vain, we are gonna use them so much here!";
            break;

            case 7:
            gameObject.GetComponentInChildren<Text>().text = "As I said, there are major and minor type. Again, major chords sound joyful, and minor sound sad and gloomy.";
            break;

            case 8:
            gameObject.GetComponentInChildren<Text>().text = "Triads cosist of Perfect Fifth and one of the Thirds, which determines the mood.";
            break;

            case 9:
            gameObject.GetComponentInChildren<Text>().text = "So, to build a major triad, we pick the first (tonic) note and use Major Third and Perfect Fifth.";
            break;

            case 10:
            gameObject.GetComponentInChildren<Text>().text = "To build a minor triad, we use Minor Third and Perfect Fifth.";
            break;

            case 11:
            animator_Piano.GetComponent<Animator>().SetBool("Up", true);
            MChord.GetComponent<MChord>().Clear_All();
            if(switchRound==0) gameObject.GetComponentInChildren<Text>().text = "Let's try it, for example let's build C Major triad.";
            else gameObject.GetComponentInChildren<Text>().text = "Now let's try the minor sound. This time I'll still guide you, next time you'll go independent.";
            break;

            case 12:
            StopAllCoroutines();
            isPressable = false;
            MChord.GetComponent<MChord>().isClicked = true;
            if(switchRound==0)
            {
                scaleName = 0;
                // previousName = scaleName;
                // scaleName = Random.Range(0,12);
                // while(previousName==scaleName){
                //     scaleName = Random.Range(0,12);
                //     Debug.Log("Again");
                // }
                gameObject.GetComponentInChildren<Text>().text = "First, select the C note which is the tonic note.";
            }else
            {
                scaleName = 9;
                gameObject.GetComponentInChildren<Text>().text = "We build A Minor chord. Start with selecting A note.";
            }
            StartCoroutine(NextChord());
            break;

            case 13:
            StopAllCoroutines();
            if(switchRound==0) gameObject.GetComponentInChildren<Text>().text = "Build Major Third interval from it.";
            else gameObject.GetComponentInChildren<Text>().text = "Build Minor Third interval from it.";
            StartCoroutine(NextChord());
            break;

            case 14:
            StopAllCoroutines();
            gameObject.GetComponentInChildren<Text>().text = "Add Perfect Fifth to it.";
            StartCoroutine(NextChord());
            break;

            case 15:
            MChord.GetComponent<MChord>().isClicked = false;
            MChord.GetComponent<MChord>().PLAYisPressable = true;
            gameObject.GetComponentInChildren<Text>().text = "Now hit \"play\" button and see how it sounds all together.";
            StartCoroutine(NextChord());
            break;

            case 16:
            StopAllCoroutines();
            Debug.Log("TempessageNum is " + tempMessageNum);
            if(tempMessageNum==12) MChord.GetComponent<MChord>().Remove_Number(0);
            else if(tempMessageNum==13) MChord.GetComponent<MChord>().Remove_Number(1);
            else if(tempMessageNum==14) MChord.GetComponent<MChord>().Remove_Number(2);
            gameObject.GetComponentInChildren<Text>().text = "Nope, try again.";
            message_Num = tempMessageNum;
            StartCoroutine(NextChord());
            break;

            case 17:
            MChord.GetComponent<MChord>().Clear_All();
            gameObject.GetComponentInChildren<Text>().text = "You shouldn't have touched the selected notes again, start over";
            message_Num = 11;
            isPressable = false;
            MChord.GetComponent<MChord>().isClicked = false;
            StartCoroutine(Wait2SecAndStart(1.5f));
            break;

            case 18:
            Cat.GetComponent<Animator>().SetTrigger("Ex");
            MChord.GetComponent<MChord>().isClicked = false;
            isPressable = true;
            MChord.GetComponent<MChord>().PLAYisPressable = false;
            switchRound = 1;
            message_Num = 10;
            gameObject.GetComponentInChildren<Text>().text = "Sounds neat! And that was C Major chord! It has got its name from the tonic note C.";
            break;

            case 19:
            Cat.GetComponent<Animator>().SetTrigger("Ex");
            MChord.GetComponent<MChord>().isClicked = false;
            isPressable = true;
            MChord.GetComponent<MChord>().PLAYisPressable = false;
            gameObject.GetComponentInChildren<Text>().text = "That's what I call music! It was A Minor chord.";
            break;

            case 20:
            gameObject.GetComponentInChildren<Text>().text = "Now I'll name chords, you'll select the notes needed and would hit play.";
            break;

            case 21:
            gameObject.GetComponentInChildren<Text>().text = "After you played your chord, I'd say if it was correct.";
            break;
             
            case 22:
            MChord.GetComponent<MChord>().Clear_All();
            StopAllCoroutines();
            MChord.GetComponent<MChord>().PLAYisPressable = true;
            MChord.GetComponent<MChord>().isClicked = true;
            isPressable = false;
            GenerateRandomChord();
            tempText = gameObject.GetComponentInChildren<Text>().text;
            StartCoroutine(CheckChord());
            break;

            case 23:
            gameObject.GetComponentInChildren<Text>().text = "Nope, try again. It was " + tempText;
            StartCoroutine(CheckChord());
            break;

            case 28:
            animator_Piano.GetComponent<Animator>().SetBool("Up", true);
            Interval_manager.GetComponent<Intervals_Highlight>().isClicked = false;
            isPressable = true;
            gameObject.GetComponentInChildren<Text>().text = "Chord practise time!";
            message_Num = 21;
            break;
        }
        Debug.Log(message_Num + "!!");
        message_Num++;
    }

    public void Skip_Button()
    {
        message_Num = 0;
    }

    private IEnumerator Next()
    {
        yield return new WaitWhile(() => Interval_manager.GetComponent<Intervals_Highlight>().selectedButtonNames.Count == 0);
        // yield return new WaitUntil(() => Interval_manager.GetComponent<Intervals_Highlight>().selectedButtonNames != null);
        if (LevelType==2)
        {
            Debug.Log("sth");
            if(Interval_manager.GetComponent<Intervals_Highlight>().selectedButtonNames[0] == 0)
            {
                message_Num = 33;
                Clicked_Notes();
            }else{
                Clicked_Notes();
                Debug.Log("sth 3");
            }
        }else if(LevelType==1)
        {
            if (message_Num==8)
            {
                Debug.Log("Next()");
                if(correctNoteCounter>7) message_Num++;
                Interval_manager.GetComponent<Intervals_Highlight>().CheckNote();
                if(Interval_manager.GetComponent<Intervals_Highlight>().correct) correctNoteCounter++;
                else Clicked_Intervals();
                Debug.Log("Next() finished");
            }else if(message_Num==12 || message_Num==15)
            {
                Debug.Log(scaleName + " Scale name");
                if(message_Num==15)
                {   
                    int temp = 7;
                    if(scaleName>4) temp = -5;
                    if(Interval_manager.GetComponent<Intervals_Highlight>().selectedButtonNames[0] != scaleName
                     && Interval_manager.GetComponent<Intervals_Highlight>().selectedButtonNames[0]!=(scaleName+temp))
                    {
                        Debug.Log(Interval_manager.GetComponent<Intervals_Highlight>().selectedButtonNames[0] + " and " + scaleName);
                        Interval_manager.GetComponent<Intervals_Highlight>().CheckButton.GetComponent<Image>().color = Color.red;
                        message_Num = 15;
                        Clicked_Intervals();
                        yield break;
                    }
                }
                yield return new WaitUntil(() => Interval_manager.GetComponent<Intervals_Highlight>().selectedButtonNames.Count == 2);
                Debug.Log("Next()");
                if(correctNoteCounter>6) message_Num++;
                Interval_manager.GetComponent<Intervals_Highlight>().CheckInterval();
                if(!Interval_manager.GetComponent<Intervals_Highlight>().correct)
                {
                    message_Num = (message_Num == 12) ? 13 : 15;
                    Clicked_Intervals();
                }else correctNoteCounter++;
            }else if (message_Num == 24 || message_Num==29)
            {
                int temp = 3;
                if(intervalName==3) temp = 4;
                else if(intervalName==6) temp = 7;
                if(intervalName==2 && scaleName>8) temp = -9;
                if(intervalName==3 && scaleName>7) temp = -8;
                if(intervalName==6 && scaleName>4) temp = -5;
                if(Interval_manager.GetComponent<Intervals_Highlight>().selectedButtonNames[0] != scaleName
                && Interval_manager.GetComponent<Intervals_Highlight>().selectedButtonNames[0]!=(scaleName+temp))
                {
                    Debug.Log(Interval_manager.GetComponent<Intervals_Highlight>().selectedButtonNames[0] + " and " + scaleName);
                    Interval_manager.GetComponent<Intervals_Highlight>().CheckButton.GetComponent<Image>().color = Color.red;
                    // message_Num = 24;
                    Clicked_Intervals();
                    yield break;
                }
                yield return new WaitUntil(() => Interval_manager.GetComponent<Intervals_Highlight>().selectedButtonNames.Count == 2);
                Debug.Log("Next()");
                if(correctNoteCounter>6) message_Num++;
                Interval_manager.GetComponent<Intervals_Highlight>().CheckInterval();
                if(!Interval_manager.GetComponent<Intervals_Highlight>().correct)
                {
                    // message_Num = 24;
                    Clicked_Intervals();
                }else correctNoteCounter++;
            }else Debug.Log("else");
        }
    }

    private IEnumerator NextChord()
    {
        tempMessageNum = message_Num;
        // Debug.Log(tempMessageNum);
        yield return new WaitWhile(() => MChord.GetComponent<MChord>().selectedButtonNames.Count == 0);
        if(message_Num==13)
        {
            Debug.Log("case 1, MessageNum " + message_Num);
            if(MChord.GetComponent<MChord>().selectedButtonNames[0] == scaleName)
            {
                Debug.Log("MessageNum " + message_Num);
                Animation_button.GetComponent<Animator>().SetTrigger("Green");
                Clicked_Chords();
            }else
            {
                Animation_button.GetComponent<Animator>().SetTrigger("Red");
                message_Num = 16;
                Debug.Log("wrong");
                Clicked_Chords();
            }
        }else if(message_Num==14)
        {
            Debug.Log("case 2, MessageNum " + message_Num);
            // int tempScaleName0 = MChord.GetComponent<MChord>().selectedButtonNames[0];
            StartCoroutine(DontTouch(1));
            yield return new WaitUntil(() => MChord.GetComponent<MChord>().selectedButtonNames.Count == 2);
            // if(tempScaleName0!=MChord.GetComponent<MChord>().selectedButtonNames[0])
            // {
            //     message_Num = 17;
            //     Clicked_Chords();
            // }else
            int numToCheck = (switchRound==1) ? 0:4;
            if(MChord.GetComponent<MChord>().selectedButtonNames[1] == numToCheck){
                Animation_button.GetComponent<Animator>().SetTrigger("Green");
                Clicked_Chords();
            }
            else
            {
                message_Num = 16;
                Debug.Log("wrong");
                Animation_button.GetComponent<Animator>().SetTrigger("Red");
                Clicked_Chords();
            }
        }else if(message_Num==15)
        {
            Debug.Log("case 3, MessageNum " + message_Num);
            StartCoroutine(DontTouch(2));
            yield return new WaitUntil(() => MChord.GetComponent<MChord>().selectedButtonNames.Count == 3);
            // if(tempScaleName0!=MChord.GetComponent<MChord>().selectedButtonNames[0])
            // {
            //     message_Num = 17;
            //     Clicked_Chords();
            // }else 
            int numToCheck = (switchRound==1) ? 4:7;
            if(MChord.GetComponent<MChord>().selectedButtonNames[2] == numToCheck){
                Animation_button.GetComponent<Animator>().SetTrigger("Green");
                Clicked_Chords();
            }
            else
            {
                Animation_button.GetComponent<Animator>().SetTrigger("Red");
                message_Num = 16;
                Debug.Log("wrong");
                Clicked_Chords();
            }
        }else if(message_Num==16){
            yield return new WaitUntil(() => MChord.GetComponent<MChord>().PlayMChordButton.GetComponent<PlayMChord>().isPressed);
            Animation_button.GetComponent<Animator>().SetTrigger("Green");
            yield return new WaitUntil(() => !MChord.GetComponent<MChord>().PlayMChordButton.GetComponent<PlayMChord>().isPressed);
            message_Num = (switchRound==0)? 18:19;
            Clicked_Chords();
        }else{
            Debug.Log("error");
            // Debug.Log(scaleName + " and " + MChord.GetComponent<MChord>().selectedButtonNames[0]);
        }
    }

    private IEnumerator DontTouch(int quantity)
    {
        yield return new WaitUntil(() => MChord.GetComponent<MChord>().selectedButtonNames.Count == quantity-1);
        message_Num = 17;
        Clicked_Chords();
    }

    private IEnumerator Wait2Sec(float sec)
    {
        if(talk) Cat.GetComponent<Animator>().SetBool("Talk", true);
        yield return new WaitForSeconds(sec);
        Cat.GetComponent<Animator>().SetBool("Talk", false);
    }

    private IEnumerator Wait2Sec_Blink(float sec)
    {
        if(talk) Cat.GetComponent<Animator>().SetBool("Blink", true);
        yield return new WaitForSeconds(sec);
        Cat.GetComponent<Animator>().SetBool("Blink", false);
    }

    private IEnumerator Wait2SecAndStart(float sec)
    {
        yield return new WaitForSeconds(sec);
        Clicked_Chords();
    }

    private IEnumerator CheckChord()
    {
        yield return new WaitUntil(() => MChord.GetComponent<MChord>().PlayMChordButton.GetComponent<PlayMChord>().isPressed);
        yield return new WaitUntil(() => !MChord.GetComponent<MChord>().PlayMChordButton.GetComponent<PlayMChord>().isPressed);
        bool isEqual = Enumerable.SequenceEqual(MChord.GetComponent<MChord>().selectedButtonNames.OrderBy(e => e), notes.OrderBy(e => e));
        if(isEqual)
        {
            Animation_button.GetComponent<Animator>().SetTrigger("Green");
            message_Num--;
            Animation_text.GetComponent<Text>().text = Congratulation_texts[UnityEngine.Random.Range(0,4)];
            Animation_text.GetComponent<Animator>().SetTrigger("Congratulate");
            message_Num = 22;
            Clicked_Chords();
        }else{
            Animation_button.GetComponent<Animator>().SetTrigger("Red");
            message_Num = 23;
            Clicked_Chords();
        }
        Debug.Log(isEqual);
    }

    public void GenerateRandomChord()
    {
        notes.Clear();
        typeNo = Random.Range(0,2);

        previousName = scaleName;
        scaleName = Random.Range(0,12);
        while(previousName==scaleName){
            scaleName = Random.Range(0,12);
            Debug.Log("Again");
        }
        if(scaleName == 1 || scaleName == 3 || scaleName == 6 || scaleName == 8 || scaleName == 10)
        {
            string nameLoc = "error";
            Debug.Log("if clause");
            Bemol = Random.Range(0, 2) == 1;
            if(Bemol)
            {
                nameLoc = (scaleNames[scaleName+1] + "b");
                gameObject.GetComponentInChildren<Text>().text = nameLoc + " " + typeName[typeNo];
            }else{
                gameObject.GetComponentInChildren<Text>().text = scaleNames[scaleName] + " " + typeName[typeNo];
            }
        }else{
            gameObject.GetComponentInChildren<Text>().text = scaleNames[scaleName] + " " + typeName[typeNo];
        }
        
        switch (typeNo)
        {
            case 0:
            notes.AddRange(new int[] {scaleName, addInterval(4), addInterval(7)});
            break;

            case 1:
            notes.AddRange(new int[] {scaleName, addInterval(3), addInterval(7)});
            break;
        }
    }

    private int addInterval(int interval)
    {
        int intervalCut;
        if((scaleName+interval)>11)
        {
            intervalCut = scaleName + interval - 12;
        }else
        {
            intervalCut = scaleName + interval;
        }
        return intervalCut;
    }
}