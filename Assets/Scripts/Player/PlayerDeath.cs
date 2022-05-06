using System.Collections;
using System.Collections.Generic;
using UnityEngine;





public class PlayerDeath : MonoBehaviour
{
    void Awake()
    {
        GetComponent<KillableEntity>().OnDeath += OnDeath;
        GetComponent<KillableEntity>().OnDamage += OnDamage;
    }

    void OnDeath(KillableEntity ke)
    {
        Debug.Log("Player died");
    }

    void OnDamage(KillableEntity ke)
    {
        Debug.Log("Player damaged");
        ScreenShake.Instance.Shake();
    }



}
