using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Basket : MonoBehaviour
{
    public delegate void Goal(bool fromPlayer);
    public static Goal goal;

    public enum BasketType
    {
        player, enemy
    }
    public BasketType basket;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Ball"))
        {
            goal?.Invoke(basket == BasketType.player ? true : false);
        }
    }
}
