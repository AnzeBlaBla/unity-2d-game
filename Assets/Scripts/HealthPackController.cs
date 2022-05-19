using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPackController : MonoBehaviour
{
    public float health = 1f;

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Collided with " + other.name);
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<KillableEntity>().health += health;
            Destroy(gameObject);
            AudioManager.Instance.Play("HealthPickup");
        }
    }

}
