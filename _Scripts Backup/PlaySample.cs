using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlaySample : MonoBehaviour, IPointerDownHandler
{
    public GameObject C4Button;
    public List<GameObject> buttonsLocWhite;
    public List<int> track1;
    public List<double> durations1;
    public List<int> track2;
    public List<double> durations2;
    public bool track1playing = false;
    public GameObject Metronome;

    IEnumerator Start(){
        buttonsLocWhite = C4Button.GetComponent<Synthesizer_v2>().buttons;
        yield return new WaitUntil(()=> buttonsLocWhite != null);
        track1 = new List<int>();
        track1.AddRange(new int[] {7, 7, 11, 11, 12, 12, 11, 10, 10, 9, 9, 8, 8, 7});
        durations1.AddRange(new double[] {1,1,1,1,1,1,2,1,1,1,1,1,1,2});
        track2 = new List<int>();
        track2.AddRange(new int[] {0, 4, 7, 4, 5, 3, 7, 3, 5, 2, 4, 1, 2, 0});
        durations2.AddRange(new double[] {1.6,1.6,1.6,1.6,1.6,1.6,3.6, 1.6,1.6,1.6,1.6,1.6,1.6,1.6,3.6,});
    }

    public void OnPointerDown(PointerEventData eventData){
        CallCoroutine();
        Debug.Log("Pressed");
    }

    // Update is called once per frame
    void Update()
    {
        // nextTick = Metronome.GetComponent<Metronome>().nextTick;
        // if(buttonsLocWhite != null) TestVoid();
    }

    public void CallCoroutine()
    {   
        if(track1playing==false)
        {
        IEnumerator coroutine1 =  Metronome.GetComponent<Metronome>().playNote(track1, durations1);
        IEnumerator coroutine2 =  Metronome.GetComponent<Metronome>().playNote(track2, durations2);
        Metronome.GetComponent<Metronome>().StartCoroutine(coroutine1);
        Metronome.GetComponent<Metronome>().StartCoroutine(coroutine2);
        track1playing = true;
        }
    }
}
