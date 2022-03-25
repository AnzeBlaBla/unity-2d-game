using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float speed = 10f;
    public float lifeTime = 2f;
    public float damage = 1f;
    public Vector2 direction;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }
    void Update()
    {
        // rotate so bullet is facing the right direction
        transform.rotation = Quaternion.FromToRotation(Vector3.up, direction);

        transform.Translate(Vector3.up * speed * Time.deltaTime);
    }
}
