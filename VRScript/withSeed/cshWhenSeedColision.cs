using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class cshWhenSeedColision : MonoBehaviourPun
{
    public GameObject[] Corns;
    public AudioSource Rain;


    GameObject RainParticle;
    bool ScareCrow = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Seed") && isScareCrow())
        {
            Destroy(other.gameObject, 1.0f);
            RainParticle = PhotonNetwork.Instantiate("Rain02",
                new Vector3(transform.position.x, transform.position.y + 2.0f, transform.position.z), Quaternion.Euler(90,0,0));
            
            Invoke("InstantiacteCorn", 3.0f);
            Rain.Play();
            
        }
    }
    void InstantiacteCorn() // Invoke로 지연호출을 위한 함수
    {
        //photonView.RequestOwnership();
        //PhotonNetwork.Destroy(RainParticle);
        //PhotonNetwork.Instantiate("corn", transform.position, Quaternion.identity);
        
        foreach(GameObject corn in Corns)
        {
            corn.gameObject.SetActive(true);
        }
        gameObject.layer = LayerMask.NameToLayer("MovePos");
    }

    bool isScareCrow() {
        if (GameObject.FindWithTag("ScareCrow") != null)
            ScareCrow = true;

        return ScareCrow;
    }


}
