using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float speed = 10f;
    public float lifeTime = 2f;
    public float damage = 1f;
    public Vector2 direction { get; private set; }

    ParticleSystem ps;
    Rigidbody2D rb;

    bool exists = true;

    void Awake()
    {
        ps = GetComponent<ParticleSystem>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    public void SetDirection(Vector2 newDirection)
    {
        direction = newDirection;

        // rotate so bullet is facing the right direction
        transform.rotation = Quaternion.FromToRotation(Vector3.up, direction);

        //transform.Translate(Vector3.up * speed * Time.deltaTime);
        // New, physics based movement
        rb.velocity = direction * speed;
    }

    // When bullet collides with something
    public void Hit()
    {
        // TODO: play hit sound and particle effect
        AudioManager.Instance.Play("BulletHit");

        ps.Play();
    }

    public void Kill()
    {
        // hide bullet sprite
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        exists = false;
        rb.velocity = Vector2.zero;

        // No need, because particle system will destroy the object
        //Destroy(gameObject, 0.25f);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!exists)
        {
            return;
        }
        KillableEntity ke = collision.gameObject.GetComponent<KillableEntity>();
        if (ke != null)
        {
            if(!ke.takeBulletDamage || ke.isDead)
                return;
            
            Hit();
            float damageDone = ke.Damage(damage, true);
            //Debug.Log("Bullet hit " + collision.gameObject.name + " for " + damageDone + " damage");
            if (damageDone == damage)
            {
                Kill();
            }
            else
            {
                damage -= damageDone;
            }
        }
    }


    public static void SpawnBullet(GameObject bulletPrefab, Vector3 location, Vector2 direction, float speed = 10f, float damage = 1f)
    {
        GameObject bullet = Instantiate(bulletPrefab, new Vector3(location.x, location.y, ZPositions.bullet), Quaternion.identity);
        BulletController bc = bullet.GetComponent<BulletController>();
        bc.speed = speed;
        bc.damage = damage;
        bc.SetDirection(direction);
    }
}
