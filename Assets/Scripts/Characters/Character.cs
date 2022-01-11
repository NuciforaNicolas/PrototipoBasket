using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] protected float moveSpeed, rotationSpeed, shootForce;
    [SerializeField] Transform ballSlot, shootSlot;
    [SerializeField] protected Ball ballRef;
    [SerializeField] float timeToTakeBall, timeToTarget;
    [SerializeField] protected bool canTakeBall;
    protected Rigidbody rb;
    protected Animator anim;
    protected Vector3 moveDir, targetPos;

    [SerializeField] GameObject debugtarget;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        canTakeBall = true;
    }

    private void Update()
    {
        var pos = gameObject.transform.forward * shootForce;
        pos.y = 1.0f;
        debugtarget.transform.position = transform.position + pos;
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


    protected void ShootBall()
    {
        StartCoroutine("ShootBallCR");
    }

    protected virtual IEnumerator ShootBallCR()
    {
        anim.SetTrigger("throw");
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
        var target = transform.position + (transform.forward * shootForce);
        ballRef.ShootBall(shootSlot.position, target, timeToTarget);
        ReleaseBall();
    }
}
