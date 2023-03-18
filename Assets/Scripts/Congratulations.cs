using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Congratulations : MonoBehaviour
{
    public int speed = 100;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    public void Congratulate()
    {
        gameObject.SetActive(true);
        gameObject.transform.localPosition = new Vector3(0, 10, 0);
    }
}
