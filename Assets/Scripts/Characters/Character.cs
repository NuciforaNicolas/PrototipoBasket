using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] protected float moveSpeed, rotationSpeed, jumpForce;
    [SerializeField] Transform ballSlot;
    [SerializeField] protected Ball ballRef;
    [SerializeField] float timeToTakeBall;
    [SerializeField] protected bool canTakeBall;
    protected Rigidbody rb;
    protected Animator anim;
    protected Vector3 moveDir;
    
    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        canTakeBall = true;
    }

    protected virtual void Move() { }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ball") && canTakeBall)
        {
            anim.SetBool("hasBall", true);
            ballRef = collision.gameObject.GetComponent<Ball>();
            ballRef.GetBall(ballSlot);
            canTakeBall = false;
        }
    }

    protected IEnumerator CanTakeBallCountDown()
    {
        yield return new WaitForSeconds(timeToTakeBall);
        canTakeBall = true;
    }

    protected void ReleaseBall()
    {
        if (!ballRef) return;
        ballRef.ReleaseBall();
        ballRef = null;
        anim.SetBool("hasBall", false);
        StartCoroutine("CanTakeBallCountDown");
    }
}
