using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class cshVRCommunication : MonoBehaviourPun
{
    public GameObject EyePos;
    public bool isCcalled = false;

    public GameObject VR3dCanvas;
    public GameObject ChatUIParent;
   

    private void Start()
    {
        /*
        ChatUIParent = GameObject.FindWithTag("VRChatP");
        VR3dCanvas = GameObject.Find("VR3dCanvasP");
        */
    }
    // Update is called once per frame
    void Update()
    {
        if (!isCcalled)
        {
            //ChatUIParent.transform.GetChild(0).gameObject.SetActive(false);
            VR3dCanvas.transform.GetChild(0).gameObject.SetActive(false);
            ChatUIParent.transform.GetChild(1).gameObject.SetActive(false);
            return;
        }
        //ChatUIParent.transform.GetChild(0).gameObject.SetActive(true);
        VR3dCanvas.transform.GetChild(0).gameObject.SetActive(true);
        ChatUIParent.transform.GetChild(1).gameObject.SetActive(true);
    }

 
}