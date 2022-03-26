using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This is a rotating enemy that moves towards the player
public class EnemyController : MonoBehaviour
{
    public float rotationSpeed = 500f;
    public float movementSpeed = 5f;
    public float keepDistance = 1f;
    public float damageInterval = 0.1f;
    public float damage = 1f;
    float lastDamageTime = 0f;
    GameObject player;
    void Start()
    {
        player = GameObjectRegistry.Instance.player;
    }

    void Update()
    {
        // if player died
        if(player == null)
        {
            return;
        }
        transform.Rotate(Vector3.forward, Time.deltaTime * rotationSpeed);
        Vector3 direction;
        if (Vector3.Distance(player.transform.position, transform.position) > keepDistance)
        {
            direction = player.transform.position - transform.position;
            direction.Normalize();
            
        } else {
            // damage player
            if (Time.time - lastDamageTime > damageInterval)
            {
                lastDamageTime = Time.time;
                player.GetComponent<KillableEntity>().Damage(damage);
            }
            // move away from player
            direction = transform.position - player.transform.position;
            direction.Normalize();
        }
        transform.position += direction * Time.deltaTime * movementSpeed;
    }
}
