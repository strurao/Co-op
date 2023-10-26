using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class cshARCharactorCreate : MonoBehaviour
{
    public bool isMcalled = false; // 외부에서 호출하는 플래그변수 
    public GameObject background; // 조이스틱
    public GameObject ARCharactor;

    private void Update()
    {
        if (!isMcalled)
        { // 외부에서 isMacalled가 true로 변경되기 전까지 아무런 일도 하지 않는다.
            ARCharactor.SetActive(false);
            background.transform.GetChild(0).gameObject.SetActive(false);
        }
        else
        {
            ARCharactor.SetActive(true);
            background.transform.GetChild(0).gameObject.SetActive(true);
        }
    }
}
