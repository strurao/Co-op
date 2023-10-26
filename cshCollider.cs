using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cshCollider : MonoBehaviour
{
    public bool coll = false;
    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "ScareCrow")
        {
            coll = true;
        }
    }
}
