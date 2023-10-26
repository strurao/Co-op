using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon;

public class cshDestroy : MonoBehaviourPun
{
    public PhotonView pv;
    void Start()
    {
        pv = GameObject.FindWithTag("ChatManager").GetComponent<PhotonView>();
    }

    public void grabDestroy()
    {
        if (!photonView.IsMine)
            photonView.RequestOwnership();
 
        Invoke("MyDestroy", 10.0f);
    }

    public void ClickDestroy()
    {
        pv.RPC("SaveGathercnt", RpcTarget.All); //gathercount++;

        if (!photonView.IsMine)
            photonView.RequestOwnership();

        //RequestOwnership() 이 수행하는 시간이 필요하여 Invoke를 사용함.
        Invoke("MyDestroy", 0.1f);
    }

    public void PinchDestroy()
    {
        pv.RPC("SaveGathercnt", RpcTarget.All); //gathercount++;

        if (!photonView.IsMine)
            photonView.RequestOwnership();

        //RequestOwnership() 이 수행하는 시간이 필요하여 Invoke를 사용함.
        Invoke("MyDestroy", 2.0f);
    }



    public void MyDestroy()
    {
        PhotonNetwork.Destroy(gameObject);
    }

}
