using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cshVRGather : MonoBehaviour
{
    public bool isGcalled = false;
    public bool isIcalled = false;
    GameObject[] corns;
 

    // Update is called once per frame
    void Update()
    {
        if (!isGcalled)
            return;

        else
        {
            CountCorn(isGcalled);
            isGcalled = false;
        }
    }

    public void CountCorn(bool isGcalled)
    {
        corns = GameObject.FindGameObjectsWithTag("Corn"); // 씬 안에있는 모든 잡기가능한 객체를 배열로저장
        foreach (GameObject corn in corns) // 저장한 잡기가능 객체를 전체 제어
        {
            if (corns.Length != 0 && corn != null)
            {
                corn.GetComponent<CapsuleCollider>().enabled = isGcalled;
                //corn.GetComponent<CapsuleCollider>().enabled = isGcalled;
            }
        }
      
    }
}
