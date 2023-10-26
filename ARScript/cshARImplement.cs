using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class cshARImplement : MonoBehaviourPun
{
    public bool isIcalled = false;
    public Camera ARCamera;
    public GameObject ARCharactor;
    public AudioSource ScareCrow;
    GameObject SpawnEffect;

    //bool move = false;
    
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        //if (!isIcalled) {
        //    for (int i = 0; i < ScareCrow.transform.childCount; i++)
        //    {
        //        ScareCrow.transform.GetChild(i).gameObject.SetActive(false);
        //    }
        //    return;
        //}
        //
        //for (int i = 0; i < ScareCrow.transform.childCount; i++)
        //{
        //    ScareCrow.transform.GetChild(i).gameObject.SetActive(true);
        //}

        //ImplementScareCrow();


        if (!isIcalled)
            return;
    }

    private void ImplementScareCrow()
    {
  
        if (Input.GetMouseButtonDown(0)) // if left button pressed...
        {
            Ray ray = ARCamera.ScreenPointToRay(Input.mousePosition); // AR 카메라로 설정
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit)) // AR 모드일때
            {
                if (hit.transform.gameObject.tag == "ScareCrowPos")
                {
                    //move = true;
                    PhotonNetwork.Instantiate("scarecrow_2", hit.transform.position, Quaternion.identity);
                    Destroy(hit.transform.gameObject);
                }
            }
        }
    }

    public void ScareCrowImple()
    {
        PhotonNetwork.Instantiate("scarecrow_2", ARCharactor.transform.position, transform.rotation);

        SpawnEffect = PhotonNetwork.Instantiate("SpawnEffect", 
            new Vector3(ARCharactor.transform.position.x, ARCharactor.transform.position.y + 0.2f, ARCharactor.transform.position.z), 
            Quaternion.Euler(transform.rotation.x + 270, transform.rotation.y, transform.rotation.z));
        ScareCrow.Play();
        Invoke("MyDestroy", 3.0f);
    }

    void MyDestroy()
    {
        photonView.RequestOwnership();
        PhotonNetwork.Destroy(SpawnEffect);
    }
}
