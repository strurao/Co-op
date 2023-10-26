using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class cshARPlayer : MonoBehaviourPun
{
    private GameObject target;
    private Camera ARCamera;

    // Start is called before the first frame update
    void Start()
    {
        // 네트워크 환경에서 현재 제어중인 사용자(AR/VR)인지 유무 판단
        if (!photonView.IsMine)
        {
            Camera[] cameras;
            cameras = transform.gameObject.GetComponentsInChildren<Camera>();
            foreach (Camera c in cameras)
            {
                c.enabled = false;
            }
        }
        else
        {
            target = GameObject.Find("TargetCube");
            ARCamera = GameObject.Find("ARCamera").GetComponent<Camera>();
            //btnGen.interactable = true;
        }
    }

    private void FixedUpdate()
    {
        if (!photonView.IsMine)
            return;
    }

    // Update is called once per frame
    void Update()
    {
        if (!photonView.IsMine)
        {
            //AR Canvas 끄기
            transform.GetChild(0).transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).gameObject.SetActive(false);
            transform.GetChild(2).gameObject.SetActive(false);
            //transform.GetChild(3).gameObject.SetActive(false);
            return;
        }
        else
        {
            // 이미지 타깃이 인식이 되면, 배경 객체를 화면에 그리고
            // 그렇지 않다면 그리지 않게 만드는 과정
            // ARObject/ControlTile/Player 레이어를 AR카메라의 Culling Mask와 연동하여 설정


            
            // cullingMask에 Layer 추가
            if (target.GetComponent<Renderer>().enabled)
            {
                ARCamera.cullingMask |= 1 << LayerMask.NameToLayer("Map");
              
            }
            // cullingMask에 Layer 제거
            else
            {
                ARCamera.cullingMask &= ~(1 << LayerMask.NameToLayer("Map"));
               
            }
        }
    }
}
