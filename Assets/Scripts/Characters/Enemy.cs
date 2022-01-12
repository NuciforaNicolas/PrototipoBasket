using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    [SerializeField] Transform ballPosition;
    [SerializeField] Transform basketPos;
    [SerializeField] float distanceToShoot;
    Vector3 targetToReach;
    bool isShooting;

    protected override void Awake()
    {
        base.Awake();
        ballPosition = GameObject.FindGameObjectWithTag("Ball").transform;
        canShoot = true;
    }

    private void FixedUpdate()
    {
        if (!GameManager.instance.CanPlay()) return;

        if (!ballRef)
        {
            targetToReach = ballPosition.position;
            isShooting = false;
        }
        else
        {
            targetToReach = basketPos.position;
        }

        if(ballRef && canShoot && (Vector3.Distance(transform.position, basketPos.position) <= distanceToShoot))
        {
            canShoot = false;
            isShooting = true;
            ShootBall();  
        }

        if(!isShooting && canMove)
            Move();

        FaceTarget();
    }

    protected override void Move()
    {
        base.Move();
        anim.SetBool("isRunning", true);
        var targetToReachXZ = new Vector3(targetToReach.x, transform.position.y, targetToReach.z);
        transform.position = Vector3.MoveTowards(transform.position, targetToReachXZ, moveSpeed * Time.fixedDeltaTime); 
    }

    void FaceTarget()
    {
        var targetToReachXZ = new Vector3(targetToReach.x, transform.position.y, targetToReach.z);
        var targetRotation = Quaternion.LookRotation(targetToReachXZ - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
    }

    public override void ResetCharacter()
    {
        base.ResetCharacter();
        canShoot = true;
    }
}
