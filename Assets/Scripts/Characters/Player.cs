using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : Character
{
    public Vector2 inputDir { get; set; }
    [SerializeField] Canvas superBarCanvas;
    [SerializeField] Image superBarImage;
    [SerializeField] float timeToFill, tuckleForce, timeToEnableMovement;
    [SerializeField] bool canMove;
    float t = 0;
    Vector3 velocityCache;

    private void Awake()
    {
        base.Awake();
        superBarImage.fillAmount = 0;
        superBarCanvas.enabled = false;
        canMove = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!GameManager.instance.CanPlay()) return;
        if (canMove)
            Move();
    }

    protected override void Move()
    {
        base.Move();
        var horizontalMove = inputDir.x;
        var verticalMove = inputDir.y;
        var movePosition = transform.position + (Vector3.ClampMagnitude(new Vector3(horizontalMove, 0, verticalMove), 1.0f) * moveSpeed * Time.fixedDeltaTime);
        rb.MovePosition(movePosition);
        if (inputDir != Vector2.zero)
        {
            var targetRotation = Quaternion.LookRotation(movePosition - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
            anim.SetBool("isRunning", true);
            FillSuperBar();
        }
    }

    public void StopMove()
    {
        inputDir = Vector3.zero;
        anim.SetBool("isRunning", false);
        if (ballRef && canShoot) 
        {
            canShoot = false;
            ShootBall();
        }
        else if(!ballRef && (t >= timeToFill))
        {
            canMove = false;
            SuperTuckle();
        }

        ResetSuperBar();
    }

    void FillSuperBar()
    {
        if (t == 0f) superBarCanvas.enabled = true;

        t += Time.deltaTime;
        if (t <= timeToFill)
        {
            superBarImage.fillAmount = Mathf.Lerp(0, 1, t / timeToFill);
        }
        else
        {
            superBarImage.color = Color.green;
            ballRef?.StartParticle();
        }
    }

    void ResetSuperBar()
    {
        t = 0;
        superBarImage.fillAmount = 0;
        superBarImage.color = Color.white;
        superBarCanvas.enabled = false;
    }

    void SuperTuckle()
    {
        anim.SetTrigger("superTackle");
        rb.AddForce(transform.forward * tuckleForce, ForceMode.Impulse);
        StartCoroutine("EnableMovement");
    }

    IEnumerator EnableMovement()
    {
        yield return new WaitForSeconds(timeToEnableMovement);
        canMove = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);

        if (collision.gameObject.CompareTag("Enemy"))
        {
            ReleaseBall();
        }
    }
}
