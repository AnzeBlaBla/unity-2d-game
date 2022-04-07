using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType
{
    Spinner,
    Chonker,
    BobJr
}


// This is a rotating enemy that moves towards the player
public class EnemyController : MonoBehaviour
{
    [Header("General")]
    public EnemyType enemyType;

    public enum EnemyMovementType
    {
        Stationary,
        FollowPlayer,
    }
    [Header("Movement")]
    public EnemyMovementType movementType = EnemyMovementType.FollowPlayer;
    public bool spin = true;
    public float spinSpeed = 500f;
    public float movementSpeed = 5f;

    [Header("Damage")]
    public float damage = 1f;
    public float damageInterval = 0.1f;
    public float initialDamageDelay = 0.1f;

    float lastDamageTime = 0f;
    GameObject player;
    Rigidbody2D rb;
    [HideInInspector]
    public float currentMovementSpeed;

    List<KillableEntity> currentTargets = new List<KillableEntity>();
    void Awake()
    {
        currentMovementSpeed = movementSpeed;

        rb = GetComponent<Rigidbody2D>();

        KillableEntity ke = GetComponent<KillableEntity>();
        if (ke != null)
        {
            ke.OnDeath += (KillableEntity k) =>
            {
                this.enabled = false;
                GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
                // disable all 2d colliders
                foreach (Collider2D c in GetComponents<Collider2D>())
                {
                    c.enabled = false;
                }

                // stop playing base sound
                AudioSource source = GetComponent<AudioSource>();
                if (source != null)
                {
                    source.Stop();
                }
            };
        }
    }
    void Start()
    {
        player = GameObjectRegistry.Instance.player;
    }

    void Update()
    {

        if (spin)
        {
            //transform.Rotate(Vector3.forward, Time.deltaTime * spinSpeed);
            rb.angularVelocity = spinSpeed;
        }

        if (Time.time - lastDamageTime > damageInterval && currentTargets.Count > 0)
        {
            //Debug.Log("Damaging" + currentTargets.Count + " targets");
            List<KillableEntity> targets = new List<KillableEntity>(currentTargets);
            foreach (KillableEntity ke in targets)
            {
                // draw raycast from out position to the object we hit and get position of hit
                Vector3 direction = ke.gameObject.transform.position - transform.position;
                direction.Normalize();
                RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, Mathf.Infinity, LayerMask.GetMask("Player"));

                //Debug.DrawRay(transform.position, direction * 100f, Color.blue);

                if (hit.point == Vector2.zero)
                {
                    //Debug.Log("Did not hit anything");
                    ke.Damage(damage);
                    return;
                }
                //Debug.Log("Hit " + hit.collider.gameObject.name + " dist " + hit.distance + " at " + hit.point);
                ke.DamageAt(hit.point, damage);
            }
            lastDamageTime = Time.time;
        }
    }

    void FixedUpdate()
    {
        if (movementType == EnemyMovementType.FollowPlayer)
        {

            Vector3 direction;
            direction = player.transform.position - transform.position;
            direction.Normalize();

            rb.velocity = direction * currentMovementSpeed;

            if (!spin)
            {
                // rotate towards player (2d)
                Vector3 lookDirection = player.transform.position - transform.position;
                lookDirection.Normalize();
                float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }
        }
    }



    // start damaging
    void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("Player hit " + collision.gameObject.name);
        KillableEntity ke = collision.gameObject.GetComponent<KillableEntity>();

        if (ke != null && ke.takeEnemyDamage && !currentTargets.Contains(ke))
        {
            currentTargets.Add(ke);
            lastDamageTime = Time.time + initialDamageDelay - damageInterval; // start damage after initial delay
        }
    }
    // stop damaging
    void OnTriggerExit2D(Collider2D collision)
    {
        KillableEntity ke = collision.gameObject.GetComponent<KillableEntity>();
        if (ke != null)
        {
            if (currentTargets.Contains(ke))
            {
                currentTargets.Remove(ke);
            }
        }
    }



    // Static stuff

    public static void StopAllSounds()
    {
        // stop sounds on all enemies
        var enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var enemy in enemies)
        {
            enemy.GetComponent<AudioSource>().Stop();
        }
    }

    public static void PauseAllSounds()
    {
        // stop sounds on all enemies
        var enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var enemy in enemies)
        {
            enemy.GetComponent<AudioSource>().Pause();
        }
    }

    public static void ResumeAllSounds()
    {
        // stop sounds on all enemies
        var enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var enemy in enemies)
        {
            enemy.GetComponent<AudioSource>().UnPause();
        }
    }
}
