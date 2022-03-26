using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float speed = 10f;
    public float lifeTime = 2f;
    public float damage = 1f;
    public Vector2 direction;

    ParticleSystem ps;

    bool exists = true;

    void Awake()
    {
        ps = GetComponent<ParticleSystem>();
    }

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }
    void Update()
    {
        if(!exists)
        {
            return;
        }
        // rotate so bullet is facing the right direction
        transform.rotation = Quaternion.FromToRotation(Vector3.up, direction);

        transform.Translate(Vector3.up * speed * Time.deltaTime);
    }

    // When bullet collides with something
    public void Hit()
    {
        // TODO: play hit sound and particle effect
        AudioManager.Instance.Play("BulletHit");

        ps.Play();

        // hide bullet sprite
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        exists = false;
        
        Destroy(gameObject, 0.25f);
    }
}
