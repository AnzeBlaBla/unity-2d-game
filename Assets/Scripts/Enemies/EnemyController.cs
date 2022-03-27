using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This is a rotating enemy that moves towards the player
public class EnemyController : MonoBehaviour
{

    public enum EnemyMovementType
    {
        Stationary,
        FollowPlayer,
    }
    public EnemyMovementType movementType = EnemyMovementType.FollowPlayer;
    public bool spin = true;
    public float spinSpeed = 500f;
    public float movementSpeed = 5f;

    public float damage = 1f;
    public float damageInterval = 0.1f;
    GameObject player;
    Rigidbody2D rb;

    List<KillableEntity> currentTargets = new List<KillableEntity>();
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        KillableEntity ke  = GetComponent<KillableEntity>();
        if (ke != null)
        {
            ke.OnDeath += (KillableEntity k) =>
            {
                this.enabled = false;
                GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
                GetComponent<Collider2D>().enabled = false;
            };
        }
    }
    void Start()
    {
        player = GameObjectRegistry.Instance.player;

        StartCoroutine(DamageTick());
    }

    void Update()
    {
        if (spin)
        {
            transform.Rotate(Vector3.forward, Time.deltaTime * spinSpeed);
        }

        if (movementType == EnemyMovementType.FollowPlayer)
        {

            Vector3 direction;
            direction = player.transform.position - transform.position;
            direction.Normalize();

            // Old movement
            //transform.position += direction * Time.deltaTime * movementSpeed;

            // New, physics based movement
            rb.velocity = direction * movementSpeed;

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

    IEnumerator DamageTick()
    {
        while (true)
        {
            // make a copy of the current targets (because it could change while we're iterating)
            List<KillableEntity> targets = new List<KillableEntity>(currentTargets);
            foreach (KillableEntity ke in targets)
            {
                ke.Damage(damage);
            }
            yield return new WaitForSeconds(damageInterval);
        }
    }
    // start damaging
    void OnCollisionEnter2D(Collision2D collision)
    {
        KillableEntity ke = collision.gameObject.GetComponent<KillableEntity>();
        if (ke != null)
        {
            if (ke.takeEnemyDamage)
            {
                currentTargets.Add(ke);
            }
        }
    }
    // stop damaging
    void OnCollisionExit2D(Collision2D collision)
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
}
