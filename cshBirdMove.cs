using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cshBirdMove : MonoBehaviour
{
    GameObject Bird;
    GameObject ScareCrow;
    public AudioSource AudioSource;
    // Start is called before the first frame update
    void Start()
    {
        Bird = GameObject.Find("Birds");
        ScareCrow = GameObject.Find("ScareCrowPos");
    }

    // Update is called once per frame
    void Update()
    {
        BirdMove();
    }
    void BirdMove()
    {

        if (!ScareCrow.GetComponent<cshCollider>().coll)
            return;
        AudioSource.Play();
        Bird.transform.Translate(Vector3.forward * Time.deltaTime * 3);
        Bird.transform.Translate(Vector3.up * Time.deltaTime);
        Bird.transform.Rotate(-Vector3.up * Time.deltaTime * 10.0f);
    }
}
