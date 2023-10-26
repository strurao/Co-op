using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cshVRMoveV2 : MonoBehaviour
{
    private LineRenderer lineRenderer;
    public AudioSource Move;
    public Transform LHandRayPos;
    public Transform HeadRayPos;
    public Transform p1; // p0, p2값의 평균 단, y값은 p0와 같아 자연스러운 베지어커브를 만들기 위함.
    public Transform p2; // 레이 도착 위치, 여기서는 Ray와 Plane의 충돌 지점
    public GameObject Aura; // 파티클 효과
    GameObject myAura; // 프리팹으로 생성한 파이클효과
    RaycastHit hit; // 지면과 레이의 충돌위치 계산을 위한 변수

    public bool isMcalled = false; // 함수가 불렸는지 확인하기 위한 플래그 변수
    public bool Hand = true; // 손, 머리 Ray 시작위치 설정
    bool isInstantiated = false; // Update문에서 한번만 실행시키기 위한 flag 변수

    // Ray시작위치에 따른 위치정보 저장
    Vector3 StartVec;
    Vector3 ForVec;
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false; //시작시 레이는 비활성화
    }

    // Update is called once per frame
    void Update()
    {
        //함수가 불리지않으면 return, back이 불릴시
        if (!isMcalled)
        {
            lineRenderer.enabled = false; // 이동을 완료하면 라인 시각화를 비활성화
            Destroy(myAura); // 아우라 제거
            return;
        }

        //손에서 레이가 나갈것인지 머리에서 나갈것인지 선택
        if (Hand)
        {
            StartVec = LHandRayPos.position;
            ForVec = LHandRayPos.right;
        }
        else
        {
            StartVec = HeadRayPos.position;
            ForVec = HeadRayPos.forward;
        }

        int layerMask = 1 << LayerMask.NameToLayer("MovePos");

        //Ray를 Plane을 향해 쏠때
        if (Physics.Raycast(StartVec, ForVec, out hit, Mathf.Infinity, layerMask))
        {
            //Debug.Log(hit.point); // 충돌지점의 좌표
            Debug.DrawRay(StartVec, ForVec, Color.green); //레이 시각화 (game씬에서는 안보임)

            // p0, p1, p2 세가지 좌표로 베지어 커브구성 p0은 시작위치, p2는 ray와 충돌하는 Plane위치, p1는 두값의 평균 (단, y값은 p0과 동일) 

           // if (hit.collider.tag == "MovePos")
            p2.position = hit.transform.position;

            //p2.position = hit.position;
            p1.position = new Vector3((StartVec.x + p2.position.x) / 2, StartVec.y, (StartVec.z + p2.position.z) / 2);
            DrawQuadraticBezierCurve(StartVec, p1.position, p2.position); //베지어커브 함수 호출

            // 파티클 생성
            if (!isInstantiated)
            {
                myAura = Instantiate(Aura, new Vector3(p2.position.x, 0.2f, p2.position.z), Quaternion.identity); // 파티클을 목적지에 생성
                isInstantiated = true; // flag 변수 변환
            }
            if (myAura != null)
                myAura.transform.position = new Vector3(p2.position.x, 0.2f, p2.position.z);


            lineRenderer.enabled = true; // 라인 시각화 활성화
            lineRenderer.startColor = Color.yellow;
            lineRenderer.endColor = Color.white;
        }
        else
        {
            Destroy(myAura); // Ray 충돌이 일어나지 않으면 파티클 삭제
            isInstantiated = false; // flag 변수 변환
            lineRenderer.enabled = false; // 라인 시각화 비활성화
        }
    }

    //Drawing 베지어커브
    void DrawQuadraticBezierCurve(Vector3 point0, Vector3 point1, Vector3 point2)
    {
        lineRenderer.positionCount = 10;
        float t = 0f;
        Vector3 B = new Vector3(0, 0, 0);
        for (int i = 0; i < lineRenderer.positionCount; i++)
        {
            B = (1 - t) * (1 - t) * point0 
                + 2 * (1 - t) * t * point1 
                + t * t * point2;
            lineRenderer.SetPosition(i, B);
            t += (1 / (float)lineRenderer.positionCount);
        }
        lineRenderer.enabled = false;
    }

    // M 손동작 입력시 호출됨
    public void VRMoveON()
    {
        if (isMcalled)
        {
            Move.Play();
            // 2. 두번째 인식시 VR유저의 위치를 레이 위치로 바꿈 이때 y값은 기존 VR유저의 y값 유지
            transform.position =
                 new Vector3(p2.position.x, transform.position.y, p2.position.z);

            lineRenderer.enabled = false; //3. 이동을 완료하면 라인 시각화를 비활성화
            //isMcalled = false; //4.Update문 진입 차단
            Destroy(myAura); //5. 아우라 제거
        }
        else
            isMcalled = true; // 1.처음 모션인식시 Update문 진입
    }
}
