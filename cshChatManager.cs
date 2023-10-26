using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class cshChatManager : MonoBehaviourPunCallbacks
{
    public int gathercnt = 0;
    public GameObject cornBox;

    GameObject[] scroll_rects = null;
    Text ChatLog;
    GameObject arUser;

    // Start is called before the first frame update
    private void Start()
    {
        ChatLog = GetComponent<Text>();
    }

    [PunRPC]
    public void ReceiveMsg(string user, string msg)
    {
       
        ChatLog.text += user + " : " + msg + "\n";

        if (scroll_rects == null)
            scroll_rects = GameObject.FindGameObjectsWithTag("ChatUI");

        foreach (GameObject scroll_rect in scroll_rects)
        {
            scroll_rect.GetComponent<ScrollRect>().verticalNormalizedPosition = 0.0f;
        }
    }

    [PunRPC]
    public void SaveGathercnt()
    {
        gathercnt++;
        cornBox.GetComponent<cshPickedCorns>().CornSetactive(gathercnt);
    }

    [PunRPC]
    public void isMcalled(bool isMcalled)
    {
        arUser = GameObject.FindWithTag("ARUser");
        arUser.GetComponent<cshARCharactorCreate>().isMcalled = isMcalled;
    }
}

