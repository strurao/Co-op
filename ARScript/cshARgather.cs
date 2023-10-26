using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class cshARgather : MonoBehaviourPun
{
    private Camera ARCamera;
    public bool isGCalled = false;
    public GameObject gathertxt;
    public int gatherCnt = 0;
    cshVRGather cshVRGather = new cshVRGather();


    PhotonView pv;
    int layerMask;
    // Start is called before the first frame update
    void Start()
    {
        ARCamera = GameObject.Find("ARCamera").GetComponent<Camera>();
        layerMask = 1 << LayerMask.NameToLayer("Corn");

        pv = GameObject.FindWithTag("ChatManager").GetComponent<PhotonView>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isGCalled)
        {
            gathertxt.transform.parent.gameObject.SetActive(false);
            gatherCnt = 0;
            return;
        }
       
       
        Gather();
    }
    private void Gather()
    {
        if(!gathertxt.transform.parent.gameObject.activeSelf)
            cshVRGather.CountCorn(true);

        gathertxt.transform.parent.gameObject.SetActive(true);
        if (Input.GetMouseButtonDown(0)) // if left button pressed...
        { 
            Ray ray = ARCamera.ScreenPointToRay(Input.mousePosition); // AR 카메라로 설정
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask)) // AR 모드일때
            {
               
                hit.transform.gameObject.GetComponent<cshDestroy>().ClickDestroy();    
                gatherCnt++;
                //pv.RPC("SaveGathercnt", RpcTarget.All); //gathercount++;
                gathertxt.GetComponent<Text>().text = "Gather Count : " + gatherCnt.ToString();
               

            }
        }
    }
    public void myDestroy(GameObject gameObject)
    {
        pv.RPC("SaveGathercnt", RpcTarget.All);
        if (!gameObject.GetComponent<PhotonView>().IsMine)
            gameObject.GetComponent<PhotonView>().RequestOwnership();
        
        PhotonNetwork.Destroy(gameObject);
        
    }
}
