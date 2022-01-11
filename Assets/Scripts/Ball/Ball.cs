using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    Animator anim;
    Rigidbody rb;
    Collider collider;
    [SerializeField] GameObject targetObj;

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

    private void Update()
    {
        ShootBall(Vector3.zero, Vector3.zero, 0);
    }

    public void ShootBall(Vector3 origin, Vector3 target, float time)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, 100f, 1 << 6))
        {
            targetObj.transform.position = hit.point + Vector3.up * 0.1f;
            //Vector3 Vo = CalculateVelocity(origin, target, time);
            var _time = 2f;
            Vector3 Vo = CalculateVelocity(transform.position, hit.point, _time);

            if(Input.GetKeyDown(KeyCode.Space))
            {
                ReleaseBall();
                rb.velocity = Vo;
            }
        }
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
}
