using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cshARChat : MonoBehaviour
{
    public bool isCcalled = false;
    public GameObject msgList;
    public Text explanetxt;

    string[] msg;
   
    // Start is called before the first frame update
    void Start()
    {
        msg = msgList.GetComponent<cshInceptionv3ImageClassifierApply>().msg;

        for (int i = 0; i < msg.Length; i++)
            explanetxt.text += (i+1) +" : " + msg[i] + "\n";
        //ChatParent = GameObject.FindWithTag("ARChatP");
    }
    private void Update()
    {
        if (!isCcalled)
            explanetxt.transform.parent.gameObject.SetActive(false);
        else
            explanetxt.transform.parent.gameObject.SetActive(true);
    }
}
