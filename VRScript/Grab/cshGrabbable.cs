using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class cshGrabbable : OVRGrabbable

{
    AudioSource Seed;

    private Material mymtrl;
    Color Originmymtrl;
    Color lerpedColor = Color.white;
    private bool stopChange = false;
   
    private void Start()
    {
        base.Start();
        mymtrl = gameObject.GetComponent<MeshRenderer>().material;
        Originmymtrl = mymtrl.color;
        Seed = GetComponent<AudioSource>();
    }

    public override void GrabBegin(OVRGrabber hand, Collider grabPoint)
    {
        base.GrabBegin(hand, grabPoint);
        FixColor(); // grab중이면 색상이 더이상 변경되지 않음.
        stopChange = true;
    }
    public override void GrabEnd(Vector3 linearVelocity, Vector3 angularVelocity)
    {
        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        rb.useGravity = true;
        rb.isKinematic = m_grabbedKinematic;
         
        //rb.angularVelocity = transform.parent.GetComponent<Rigidbody>().angularVelocity;
        m_grabbedBy = null;
        m_grabbedCollider = null;
        Debug.Log(rb.velocity);
        //rb.velocity = GameObject.FindWithTag("LeftHand").GetComponent<cshGetVelocity>().velocity;
        
        gameObject.GetComponent<SphereCollider>().isTrigger = false;
        gameObject.GetComponent<cshDestroy>().grabDestroy(); //삭제 동기화를 위한 코드, 다중상속이 안돼 Pun기능을 활용하기 어려워서 작성하게됨.
        Seed.Play();
    }

    //grab이 끝났을경우 Invoke로 지연후 destroy하기 위한 oneline function
   

    private void Update()
    {
        if (stopChange) // grab중이거나 grab이 끝나면 더이상 색상변경 이벤트는 진행하지 않는다.
            return;

        // 두 색상값을 계속 lerp시키며 변경한다.
        PingPongColor(Originmymtrl);
    }
    
    //grab중이거나 끝났을때 색상을 고정시키기 위한 oneline function
    public void FixColor()
    {
        mymtrl.color = Originmymtrl;
    }

    //두가지 색으로 핑퐁
    void PingPongColor(Color changeColor)
    {
        lerpedColor = Color.Lerp(Color.white, changeColor, Mathf.PingPong(Time.time, 1));
        mymtrl.color = lerpedColor;
    }
}
