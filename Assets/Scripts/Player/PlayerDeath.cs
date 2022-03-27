using System.Collections;
using System.Collections.Generic;
using UnityEngine;





public class PlayerDeath : MonoBehaviour
{
    void Awake()
    {
        GetComponent<KillableEntity>().OnDeath += OnDeath;
    }

    void OnDeath(KillableEntity ke)
    {
        Debug.Log("Player died");
    }

}
