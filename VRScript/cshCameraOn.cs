using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cshCameraOn : MonoBehaviour
{
    Camera camera;
   
    // Start is called before the first frame update
    void Start()
    {
        camera = transform.gameObject.GetComponent<Camera>();
        if (!camera.enabled)
            camera.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
       
    }
}
