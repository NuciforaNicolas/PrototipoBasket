using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : Character
{
    public Vector2 inputDir { get; set; }
    [SerializeField] Canvas superBarCanvas;
    [SerializeField] Image superBarImage;
    [SerializeField] float timeToFill, tuckleForce;
    bool isSuperPlayer, isSuperTuckle;
    float t = 0;

    private void Awake()
    {
        base.Awake();
        superBarImage.fillAmount = 0;
        superBarCanvas.enabled = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!GameManager.instance.CanPlay() || !canMove) return;
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
        if (!GameManager.instance.CanPlay()) return;

        inputDir = Vector3.zero;
        anim.SetBool("isRunning", false);
        if (ballRef && canShoot) 
        {
            //canShoot = false;
            ShootBall();
        }
        else if(!ballRef && (t >= timeToFill))
        {
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
        else if(!isSuperPlayer)
        {
            ActivateSuperPlayer();
        }
    }

    void ActivateSuperPlayer()
    {
        isSuperPlayer = true;
        superBarImage.color = Color.green;
        ballRef?.StartParticle();
    }

    void ResetSuperBar()
    {
        
        t = 0;
        superBarImage.fillAmount = 0;
        superBarImage.color = Color.white;
        superBarCanvas.enabled = false;
        if(isSuperPlayer)
            isSuperPlayer = false;
    }

    void SuperTuckle()
    {
        canMove = false;
        isSuperTuckle = true;
        anim.SetTrigger("superTackle");
        rb.AddForce(transform.forward * tuckleForce, ForceMode.Impulse);
        StartCoroutine(EnableMovement());
    }

    protected override IEnumerator EnableMovement()
    {
        yield return base.EnableMovement();
        isSuperTuckle = false;
    }

    protected override void OnCollisionStay(Collision other)
    {
        base.OnCollisionStay(other);
        if(other.gameObject.CompareTag("Ball"))
        {
            if (isSuperPlayer)
                ballRef?.StartParticle();
            else
                ballRef?.StopParticle();
        }
    }

    protected void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
        {
            var enemyRef = collision.gameObject.GetComponent<Enemy>();
            if (isSuperTuckle)
                enemyRef.Stun();
            else
                enemyRef.PushBack();
        }
    }

    public override void ResetCharacter()
    {
        base.ResetCharacter();
        ResetSuperBar();
    }

    public override void PushBack()
    {
        base.PushBack();
        ResetSuperBar();
    }
}
