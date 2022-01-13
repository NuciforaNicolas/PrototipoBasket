using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallIndicator : MonoBehaviour
{
    [SerializeField] GameObject ballIndicator;
    [SerializeField] GameObject target;
    Renderer renderer;
 
    private void Awake()
    {
        renderer = GetComponent<Renderer>();
    }

    private void Update()
    {
        if(!renderer.isVisible)
        {
            if (!ballIndicator.activeSelf) ballIndicator.SetActive(true);
            var dir = target.transform.position - transform.position;
            if(Physics.Raycast(transform.position, dir, out var hit, 100, 1 << 7))
            { 
                if(hit.collider)
                {
                    ballIndicator.transform.position = hit.point;
                }
            }
        }
        else
        {
            if (ballIndicator.activeSelf) ballIndicator.SetActive(false);
        }
    }
}
