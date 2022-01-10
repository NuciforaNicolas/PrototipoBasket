using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    public Vector2 inputDir { get; set; }

    private void Awake()
    {
        base.Awake();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        var horizontalMove = inputDir.x;
        var verticalMove = inputDir.y;
        var movePosition = transform.position + (Vector3.ClampMagnitude(new Vector3(horizontalMove, 0, verticalMove), 1.0f) * moveSpeed * Time.fixedDeltaTime);
        rb.MovePosition(movePosition);
        if (inputDir != Vector2.zero)
        {
            var targetRotation = Quaternion.LookRotation(movePosition - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
            anim.SetBool("isRunning", true);
        }
        else
            anim.SetBool("isRunning", false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
        if(collision.gameObject.CompareTag("Enemy"))
        {
            ReleaseBall();
        }
    }
}
