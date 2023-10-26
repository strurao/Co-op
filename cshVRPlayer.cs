using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class cshVRPlayer : MonoBehaviourPun
{
    // Start is called before the first frame update
    void Start()
    {
        if (!photonView.IsMine)
        {
            // VR 카메라 FALSE
            GetComponentInChildren<OVRCameraRig>().disableEyeAnchorCameras = true;

            Camera[] cameras;
            cameras = transform.gameObject.GetComponentsInChildren<Camera>();
            foreach (Camera c in cameras)
            {
                c.enabled = false;
            }
            GameObject.FindWithTag("VRChatP").SetActive(false);

            // VR 사용자가 네트워크 상에서 자신이 아닐 경우
            // 오큘러스 충돌을 방지하기 위해 관련된 속성들을 모두 비활성화
            //GetComponent<CharacterController>().enabled = false;
            //GetComponent<OVRPlayerController>().enabled = false;
            GetComponentInChildren<OVRCameraRig>().enabled = false;
            GetComponentInChildren<OVRManager>().enabled = false;
            GetComponentInChildren<OVRHeadsetEmulator>().enabled = false;

            GetComponent<cshVRShareCamera>().enabled = false;
            GetComponent<cshVRCommunication>().enabled = false;
            GetComponent<cshVRMoveV2>().enabled = false;
            GetComponent<cshVRSetting>().enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!photonView.IsMine)
        {
            return;
        }
    }

    private void FixedUpdate()
    {
        if (!photonView.IsMine)
            return;
    }
}
