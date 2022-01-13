using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] protected float moveSpeed, rotationSpeed, shootForce, pushForce, timeToEnableMovement;
    [SerializeField] Transform ballSlot, shootSlot;
    [SerializeField] float timeToTakeBall, timeToTarget;
    [SerializeField] protected Ball ballRef;
    protected bool canTakeBall, canShoot, canMove;
    protected Rigidbody rb;
    protected Animator anim;
    protected Vector3 moveDir, targetPos;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        canTakeBall = true;
        canMove = true;
    }

    protected virtual void Move() { }

    protected virtual void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ball") && canTakeBall)
        {
            GetBall(collision.gameObject);
        }
    }

    public virtual void PushBack()
    {
        ReleaseBall();
        canMove = false;
        canTakeBall = false;
        rb.AddForce(-transform.forward * pushForce, ForceMode.Impulse);
        StartCoroutine(CanTakeBallCountDown());
        StartCoroutine(EnableMovement());
    }

    protected IEnumerator CanTakeBallCountDown()
    {
        yield return new WaitForSeconds(timeToTakeBall);
        canTakeBall = true;
    }

    protected virtual void GetBall(GameObject ball)
    {
        anim.SetBool("hasBall", true);
        ballRef = ball.gameObject.GetComponent<Ball>();
        ballRef.GetBall(ballSlot);
        canTakeBall = false;
        canShoot = true;
    }

    protected virtual void ReleaseBall()
    {
        if (!ballRef) return;
        ballRef.ReleaseBall();
        ballRef = null;
        anim.SetBool("hasBall", false);
        canShoot = false;
        StartCoroutine("CanTakeBallCountDown");
    }


    protected virtual void ShootBall()
    {
        if(ballRef && canMove)
            StartCoroutine("ShootBallCR");
    }

    protected virtual IEnumerator ShootBallCR()
    {
        anim.SetTrigger("throw");
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
        var target = transform.position + (transform.forward * shootForce);
        ballRef?.ShootBall(shootSlot.position, target, timeToTarget);
        ReleaseBall();
    }

    protected virtual IEnumerator EnableMovement()
    {
        yield return new WaitForSeconds(timeToEnableMovement);
        rb.velocity = Vector3.zero;
        canMove = true;
    }

    public virtual void ResetCharacter()
    {
        rb.velocity = Vector3.zero;
        canTakeBall = true;
        canMove = true;
    }
}
