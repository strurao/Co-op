using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class cshButtonClick : MonoBehaviourPun
{
    public Text txt;
    public string[] Message;
    public AudioSource Click;
    public Text[] explanetxt;


    string user = "VRUser";
    float delay = 0.0f;
    int chatIdx = 0;
    PhotonView Pv;
    cshVRSetting cshVRSetting;
    Color originColor;



    private void Start()
    {
        Pv = GameObject.FindWithTag("ChatManager").GetComponent<PhotonView>();
        cshVRSetting = GameObject.FindWithTag("VRUser").GetComponent<cshVRSetting>();
        txt.text = "1";

        for (int i = 0; i < Message.Length; i++)
            explanetxt[i].text += (i + 1) + " : " + Message[i];
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Communicate")
        {
            Click.Play();
            originColor = other.GetComponent<Image>().color;
            other.gameObject.GetComponent<Image>().color = Color.blue;
        }
        switch (other.name)
        {
            // Communication Mode
            case "chat":
                ++chatIdx;
                txt.text = (chatIdx % Message.Length + 1).ToString();
                boldtext(chatIdx);
                break;

            case "send":
                if (delay > 1.0f)
                {   
                    colorBtnClicked(user, Message[chatIdx % Message.Length]);
                    delay = 0.0f;
                }
                break;

            // Settings Mode
            case "bgm":
                if (delay > 1.0f)
                {
                    cshVRSetting.ChangeBGM();
                    delay = 0.0f;
                }
                break;

            case "sky":
                if (delay > 1.0f)
                {
                    cshVRSetting.ChangeSkybox();
                    delay = 0.0f;
                }
                break;

        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Communicate")
            other.gameObject.GetComponent<Image>().color = originColor;
    }
    void Update()
    {
        delay += Time.deltaTime;
    }

    public void colorBtnClicked(string user, string msg)
    {
        Debug.Log(msg);
        Pv.RPC("ReceiveMsg", RpcTarget.All, user, msg);
    }

    void boldtext(int idx)
    {
        for (int i = 0; i < Message.Length; i++)
        {
            if (i == idx % Message.Length)
                explanetxt[i].fontStyle = FontStyle.Bold;
            else
                explanetxt[i].fontStyle = FontStyle.Normal;
        }
    }
}
