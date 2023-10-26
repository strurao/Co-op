using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;
using Photon.Realtime;

public class cshVRSelect : MonoBehaviourPunCallbacks
{
    public Image ProgressBar;
    public LineRenderer rayLine;
    public Transform RHandRayPos;
    public PhotonView PV;

    private bool isSelectMode;
    public string trashName = null;
    public bool isSelect;
    public bool isS = false;
    public bool IsOn = true;

    private float barTime = 0.0f;
    private const float selectTime = 5.0f;

    void Start()
    {
        rayLine.enabled = false;
        isSelectMode = false;
        isSelect = false;
        ProgressBar.fillAmount = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {

        // 키보드 입력으로 일단 만들어 보기
        // 선택(S) 물건 선택
        if (isS)
        {
            if (!isSelectMode)
                isSelectMode = true;
            else
            {
                isSelectMode = false;
                rayLine.enabled = false;
            }
            isS = false;
        }

        if (isSelectMode)
        {
            Debug.Log("S(선택) : VR Select for Hand RayCast");

            RaycastHit hit;
            int layerMask = 1; 

            rayLine.SetPosition(0, RHandRayPos.position);                           //손에서 나가는 레이를 시각화
            rayLine.SetPosition(1, RHandRayPos.position + (-RHandRayPos.right));
            rayLine.enabled = true;
            rayLine.SetColors(Color.white, Color.white);

            if (Physics.Raycast(RHandRayPos.position, -RHandRayPos.right, out hit, Mathf.Infinity, layerMask))
            {
                if (barTime <= selectTime && hit.transform.tag != "Untagged")
                {
                    ProgressBar.enabled = true;
                    IsOn = true;
                    rayLine.SetColors(Color.blue, Color.blue);
                    Debug.DrawRay(RHandRayPos.position, -RHandRayPos.right * hit.distance, Color.blue);
                    Debug.Log(hit.transform.tag);
                }
                
            }
            else
            {
                ProgressBar.enabled = false;
                barTime = 0.0f;
                ProgressBar.fillAmount = 0.0f;
                Debug.DrawRay(RHandRayPos.position, -RHandRayPos.right * 1000, Color.red);
                rayLine.SetColors(Color.white, Color.white);

            }


            if (IsOn)
            {
                if (barTime <= selectTime)
                {
                    barTime += Time.deltaTime;
                }
                ProgressBar.fillAmount = barTime / selectTime;

                if (ProgressBar.fillAmount >= 1.0)
                {
                    IsOn = false;
                    barTime = 0.0f;
                    ProgressBar.fillAmount = 0.0f;
                    PV.RPC("Interaction", RpcTarget.All, hit.transform.tag);
                }
            }
        }
    }
    [PunRPC]
    private void Interaction(string tag)
    {
        return;
    }
        
}