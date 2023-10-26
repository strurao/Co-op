using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class cshVRShareCamera : MonoBehaviour
{
    GameObject ARrenderParent;
    public bool isScalled = false;
    Image img;
    private void Start()
    {
        ARrenderParent = GameObject.FindWithTag("ARrender");
        img = ARrenderParent.GetComponent<Image>();
        img.enabled = false;
    }
    private void Update()
    {
        if (isScalled)
        {
            ARrenderParent.transform.GetChild(0).gameObject.SetActive(true);
            img.enabled = true;
        }
        else 
        {
            ARrenderParent.transform.GetChild(0).gameObject.SetActive(false);
            img.enabled = false;
        }
    }
}

