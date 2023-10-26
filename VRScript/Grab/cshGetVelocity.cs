using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cshGetVelocity : MonoBehaviour
{
    public Vector3 oldPosition; 
    Vector3 currentPosition;
    public Vector3 velocity; //cshGrabbable 에서 이 속성을 가져가서 rb.velocity로 관성운동 구현.

    float runningTime = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        oldPosition = transform.position;
        InvokeRepeating("MyDistance",0.2f,0.2f);

    }

    // Update is called once per frame
    void Update()
    {
        currentPosition = transform.position;
        var distance = (currentPosition - oldPosition);
        velocity = distance / Time.deltaTime; 
       
    }

    void MyDistance()
    {
        oldPosition = currentPosition;
    }
}
