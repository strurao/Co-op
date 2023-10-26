using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class cshCorn : OVRGrabbable

{
    GameObject vrUser;
    Material mymtrl;
    Color Originmymtrl;
    Color lerpedColor = Color.white;
    

    bool stopChange = false;
    public AudioSource Corn;

    private void Start()
    {
        base.Start();
        vrUser = GameObject.FindWithTag("VRUser");
        mymtrl = gameObject.GetComponent<MeshRenderer>().material;
        Originmymtrl = mymtrl.color;
    }

    public override void GrabBegin(OVRGrabber hand, Collider grabPoint)
    {
        base.GrabBegin(hand, grabPoint);
        stopChange = true;
    }
    public override void GrabEnd(Vector3 linearVelocity, Vector3 angularVelocity)
    {
        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        rb.useGravity = true;
        rb.isKinematic = m_grabbedKinematic;
        rb.velocity = GameObject.FindWithTag("LeftHand").GetComponent<cshGetVelocity>().velocity;
        //rb.angularVelocity = transform.parent.GetComponent<Rigidbody>().angularVelocity;
        m_grabbedBy = null;
        m_grabbedCollider = null;
        Corn.Play();
        gameObject.GetComponent<cshDestroy>().PinchDestroy(); //삭제 동기화를 위한 코드, 다중상속이 안돼 Pun기능을 활용하기 어려워서 작성하게됨.
    }

    //grab이 끝났을경우 Invoke로 지연후 destroy하기 위한 oneline function


    private void Update()
    {
        if (stopChange)
        {
            FixColor();
            return;
        }

        PingPongColor(Color.white);

    }

    //grab중이거나 끝났을때 색상을 처음 색으로 고정시키기 위한 oneline function
    void FixColor()
    {
        mymtrl.color = Originmymtrl;
    }

    void PingPongColor(Color changeColor)
    {
        lerpedColor = Color.Lerp(Originmymtrl, changeColor, Mathf.PingPong(Time.time, 1));
        mymtrl.color = lerpedColor;
    }
}