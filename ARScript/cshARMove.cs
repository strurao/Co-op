﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class cshARMove : MonoBehaviourPun
{
    private Animator m_animator;
    private Vector3 m_velocity;
    private bool m_isGrounded = true;
    private bool m_jumpOn = false;

    public cshJoystick sJoystick;
    public float m_moveSpeed = 2.0f;
    public float m_jumpForce = 5.0f;
    public AudioSource Move;

   
    void Start()
    {
        if (!photonView.IsMine)
            return;
        m_animator = GetComponent<Animator>();
        sJoystick = GameObject.FindWithTag("background").transform.GetChild(0).gameObject.GetComponentInChildren<cshJoystick>();
    }

    void Update()
    {
        if (!photonView.IsMine)
            return;
        PlayerMove();
        //m_animator.SetBool("Jump", !m_isGrounded);
    }

    public void OnVirtualPadJump()
    {

        if (this == null) { return; }
        const float rayDistance = 0.2f;
        var ray = new Ray(transform.localPosition + new Vector3(0.0f, 0.1f, 0.0f), Vector3.down);
        if (Physics.Raycast(ray, rayDistance))
        {
            m_jumpOn = true;
        }
    }

    private void PlayerMove()
    {
        CharacterController controller = GetComponent<CharacterController>();
        float gravity = 20.0f;

        if (controller.isGrounded)
        {
            float h = sJoystick.GetHorizontalValue();
            float v = sJoystick.GetVerticalValue();
            m_velocity = new Vector3(h, 0, v);
            m_velocity = m_velocity.normalized;

            if (m_velocity != Vector3.zero && !Move.isPlaying)
                Move.Play();

            //m_animator.SetFloat("Move", m_velocity.magnitude);

            if (m_jumpOn)
            {
                m_velocity.y = m_jumpForce;
                m_jumpOn = false;
            }
            else if (m_velocity.magnitude > 0.5)
            {
                transform.LookAt(transform.position + m_velocity);
            }
        }

        m_velocity.y -= gravity * Time.deltaTime;
        controller.Move(m_velocity * m_moveSpeed * Time.deltaTime);

        m_isGrounded = controller.isGrounded;
    }

}
