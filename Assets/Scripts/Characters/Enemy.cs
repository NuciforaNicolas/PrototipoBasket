using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    [SerializeField] Transform ballPosition;
    [SerializeField] Transform basketPos;
    [SerializeField] float distanceToShoot, stunForce, timeToRespawn;
    [SerializeField] Transform spawnPosition;
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
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        canShoot = true;
    }

    public void Stun()
    {
        ReleaseBall();
        anim.SetBool("isRunning", false);
        canMove = false;
        canShoot = false;
        canTakeBall = false;
        rb.constraints = RigidbodyConstraints.None;
        rb.AddForce(Vector3.up * stunForce, ForceMode.Impulse);
        StartCoroutine("RespawnEnemy");
    }

    IEnumerator RespawnEnemy()
    {
        yield return new WaitForSeconds(timeToRespawn);
        transform.position = spawnPosition.position;
        transform.rotation = spawnPosition.rotation;
        ResetCharacter();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Player>().PushBack();
        }
    }
}
