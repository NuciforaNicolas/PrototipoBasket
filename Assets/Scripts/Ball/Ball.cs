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

    public void ShootBall(Vector3 origin, Vector3 target, float time)
    {
        transform.position = origin;
        Vector3 Vo = CalculateVelocity(origin, target, time);
        rb.velocity = Vo;
    }

    Vector3 CalculateVelocity(Vector3 origin, Vector3 target, float time)
    {
        // Definiamo la distanza dal punto di origine al target
        var targetDistance = target - origin;
        var targetDistanceXZ = targetDistance;
        targetDistanceXZ.y = 0;

        // Scomponiamo in X e Y la distanza usando dei float
        float Sy = targetDistance.y;
        float Sxz = targetDistanceXZ.magnitude;

        // Calcoliamo velocit� iniziale X e Y
        float Vxz = Sxz / time;
        float Vy = (Sy / time) + (0.5f * Mathf.Abs(Physics.gravity.y) * time);

        Vector3 result = targetDistanceXZ.normalized;
        result *= Vxz;
        result.y = Vy;
        return result;
    }
}
