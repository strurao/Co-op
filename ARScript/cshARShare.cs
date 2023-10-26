using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class cshARShare : MonoBehaviourPun
{
    public bool isScalled = false;
    // Update is called once per frame
    void Update()
    {
        if (!photonView.IsMine)
            return;
        if (isScalled)
            ShareVideo();
        else
            ShareVideoStop();
    }

    private void ShareVideo()
    {
        GameObject.FindWithTag("VRrender").transform.GetChild(0).gameObject.SetActive(true);     
    }
    private void ShareVideoStop()
    {
        GameObject.FindWithTag("VRrender").transform.GetChild(0).gameObject.SetActive(false);
    }
}
