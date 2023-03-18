using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Synthesizer_v3 : MonoBehaviour
{
    // NoteName - 0 to 11
    // NoteIndex - 0 to infinity

    public GameObject prefabW;
    public GameObject prefabB;
    public double frequencyStart;

    public int Starting_octave;
    public int NotesToInsenitate_quantity;

    private double[] freqLibW = {29.3678, 32.0392, 35.9628, 19.6006, 42.7672, 48.0046, 53.8833};
    private double[] freqLibB = {88.2015, 33.9444, 58.8674, 45.3103, 50.8591};
    private char[] alphabet = "CDEFGAB".ToCharArray();

    public List<GameObject> buttons = new List<GameObject>();
    public RaycastHit2D[] rays = new RaycastHit2D[10];
    public Dictionary<int, Piano_Key> myTouches = new Dictionary<int, Piano_Key>();
    public int countTouches = 0;
    public int myTouchLength = 0;
    // Start is called before the first frame update
    void Start()
    {
        GenerateButtons();
    }

    void GenerateButtons()
    {
        int noteNum = 0;
        int noteName = 0;
        int octaveNum = Starting_octave-1;
        // double currentFreq = 261.6256;
        double currentFreq = frequencyStart;
        int noteIndex = 0;
        for(int i = 0; i < NotesToInsenitate_quantity; i++)
        {   
            noteNum = (noteNum>6) ? 0:noteNum;
            noteName = (noteName>11) ? 0:noteName;
            currentFreq += freqLibW[(noteNum)] * Mathf.Pow(2, octaveNum-4);
            octaveNum = (noteNum==0) ? octaveNum+1:octaveNum;
            GameObject nextKey = Instantiate(prefabW, new Vector3((i+1)*170+120,300,0), Quaternion.identity) as GameObject;
            nextKey.GetComponentInChildren<Text>().text = alphabet[noteNum].ToString();
            nextKey.transform.SetParent(gameObject.transform, false);

            nextKey.GetComponent<Piano_Key>().frequency = (float) currentFreq;
            // nextKey.GetComponent<Piano_Key>().NoteName = noteName;

            if((i-(octaveNum-Starting_octave)*7)==1 || (i-(octaveNum-Starting_octave)*7)==5)
            {
                // nextKey.GetComponent<Piano_Key>().NoteIndex = noteIndex;
                noteIndex ++;
                noteName ++;
            }else
            {
                // nextKey.GetComponent<Piano_Key>().NoteIndex = noteIndex;
                noteIndex += 2;
                noteName +=2;
            }
            buttons.Add(nextKey);
            noteNum ++;
        }
    }

    void Update()
    {
        // foreach(Touch touch in Input.touches)
        // {
        //     if(writable[touch.fingerId]==true)
        //     {
        //         writable[touch.fingerId] = false;
        //         rays[touch.fingerId] = Physics2D.Raycast(Input.GetTouch(touch.fingerId).position, -Vector2.up);
        //         if (rays[touch.fingerId].collider != null) rays[touch.fingerId].collider.GetComponentInParent<Piano_Key>().fade = 1;
        //     }
        //     if (Input.GetTouch(touch.fingerId).phase == TouchPhase.Ended)
        //     {   
        //         writable[touch.fingerId] = true;
        //         if (rays[touch.fingerId].collider != null) rays[touch.fingerId].collider.GetComponentInParent<Piano_Key>().fade = 2;
        //     }
        // }


        // foreach(Touch touch in Input.touches)
        // {
        //     int i = touch.fingerId;
        //     //Debug.Log(touch.fingerId);
        //     RaycastHit2D ray = Physics2D.Raycast(touch.position, -Vector2.up);
        //     if (ray.collider != null) {
        //         if (touch.phase == TouchPhase.Began) {
        //             countTouches++;
        //             ray.collider.GetComponentInParent<Piano_Key>().fade = 1;
        //         } else if (touch.phase == TouchPhase.Ended) {
        //             countTouches--;
        //             ray.collider.GetComponentInParent<Piano_Key>().fade = 2;
        //         }
        //     }
        // }
        // Debug.Log("counted " + countTouches);
        // Debug.Log("TouchCount " + Input.touchCount);
        HashSet<int> activeFingers = new HashSet<int>();
        foreach(Touch touch in Input.touches)
        {
            int id = touch.fingerId;
            if (touch.phase != TouchPhase.Ended && touch.phase != TouchPhase.Canceled) 
                activeFingers.Add(id);
            RaycastHit2D ray = Physics2D.Raycast(touch.position, -Vector2.up);
            if (ray.collider != null) {
                Piano_Key piano_Key = ray.collider.GetComponentInParent<Piano_Key>();
                if (myTouches.ContainsKey(id) && !myTouches[id].Equals(piano_Key)) {
                    myTouches[id].fade = 2;
                }
                if (touch.phase == TouchPhase.Began) {
                    countTouches++;
                    piano_Key.fade = 1;
                    myTouches.Add(id, piano_Key);
                } else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled) {
                    countTouches--;
                    ray.collider.GetComponentInParent<Piano_Key>().fade = 2;
                    myTouches.Remove(id);
                }
            } else {
                if (myTouches.ContainsKey(id)) myTouches[id].fade = 2;
            }
        }
        if (activeFingers.Count > 0) Debug.Log("Active:" + string.Join(",", activeFingers));
        List<int> toRemove = new List<int>();
        foreach (int fingerId in myTouches.Keys) {
            if (!activeFingers.Contains(fingerId)) {
                toRemove.Add(fingerId);
            }
        }
        if (toRemove.Count > 0) Debug.Log("To remove:" + string.Join(",", toRemove));
        foreach (int fingerId in toRemove)
        {
            myTouches[fingerId].fade = 2;
            myTouches.Remove(fingerId);
        }
        if (myTouches.Count > 0) Debug.Log("myTouches:" + string.Join(",", myTouches.Keys));
        
        // Debug.Log("TouchCount " + Input.touchCount);
        // Debug.Log(rays.Count);
    }
}
