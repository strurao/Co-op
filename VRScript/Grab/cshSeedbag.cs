using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class cshSeedbag : MonoBehaviour
{
    public Transform seedPos;
    public GameObject VRUser;

    bool stoptoInstantiate = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("LeftHand")&&!stoptoInstantiate && VRUser.GetComponent<cshVRGather>().isIcalled)
        {
            PhotonNetwork.Instantiate("Seed", seedPos.position, Quaternion.identity);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Seed"))
            stoptoInstantiate = true;
        else
            stoptoInstantiate = false;
    }
}
