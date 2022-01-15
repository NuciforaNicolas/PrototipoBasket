using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Ball : MonoBehaviour
{
    Animator anim;
    Rigidbody rb;
    Collider collider;
    [SerializeField] ParticleSystem fireParticle;
    [SerializeField] Transform goodShotPos;
    [SerializeField] float delta, errorCorrection;
    public delegate void GoodShot();
    public static GoodShot goodShot;

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
        if ((Mathf.Round(((goodShotPos.position - target).magnitude) * 10f) * 0.1f) >= (delta - errorCorrection) && (Mathf.Round(((goodShotPos.position - target).magnitude) * 10f) * 0.1f) <= (delta + errorCorrection))
            goodShot?.Invoke();
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

        // Calcoliamo velocità iniziale X e Y
        float Vxz = Sxz / time;
        float Vy = (Sy / time) + (0.5f * Mathf.Abs(Physics.gravity.y) * time);

        Vector3 result = targetDistanceXZ.normalized;
        result *= Vxz;
        result.y = Vy;
        return result;
    }

    public void StartParticle()
    {
        if (fireParticle.isPlaying) return;
        fireParticle.Play();
    }

    public void StopParticle()
    {
        if (fireParticle.isStopped) return;
        fireParticle.Stop();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Floor") || collision.gameObject.CompareTag("Enemy"))
        {
            StopParticle();
        }
    }
}
