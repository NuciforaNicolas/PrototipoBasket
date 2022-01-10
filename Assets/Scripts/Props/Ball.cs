using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    Animator anim;
    Rigidbody rb;
    Collider collider;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
    }

    public void GetBall(Transform slot)
    {
        anim.SetBool("hasBall", true);
        transform.parent = slot;
        transform.position = slot.transform.position;
        rb.isKinematic = true;
        collider.isTrigger = true;
    }

    public void ReleaseBall()
    {
        anim.SetBool("hasBall", false);
        transform.parent = null;
        rb.isKinematic = false;
        collider.isTrigger = false;
    }
}
