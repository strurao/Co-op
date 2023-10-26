using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cshVRCharactorSyncRotate : MonoBehaviour
{
    public Transform target; //Center Eye Anchor
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 project = Vector3.Project(target.forward, Vector3.up);
        Vector3 look = target.forward - project;
        look = look.normalized;

        transform.LookAt(transform.position + look);
    }
}
