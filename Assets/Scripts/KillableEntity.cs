using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class KillableEntity : MonoBehaviour
{
    [Tooltip("Current health of the entity")]
    public float health = 100f;
    public float maxHealth = 100f;
    public bool isDead { get; private set; }

    public bool takeBulletDamage = true;
    public bool takeEnemyDamage = true;

    public Sound deathSound;
    public Sound damageSound;

    public ParticleSystem damageParticles;
    public ParticleSystem deathParticles;

    public event Action<KillableEntity> OnDeath;
    public event Action<KillableEntity> OnRevive;

    void Awake()
    {
        Revive();
    }

    public float DamageAt(Vector3 position, float damage)
    {
        // Play damage sound and particles
        if (damageSound != null)
        {
            AudioManager.Instance.Play(damageSound.name);
        }
        if (damageParticles != null)
        {
            //damageParticles.Play(new ParticleSystem.EmitParams() { position = position });

            damageParticles.gameObject.transform.position = position;
            damageParticles.Play();
        }

        return Damage(damage);
    }
    // returns damage dealt
    public float Damage(float damage)
    {
        if (isDead)
        {
            return 0;
        }

        float damageDealt = 0;
        if (health - damage <= 0)
        {
            damageDealt = health;
            health = 0;
            Kill();
        }
        else
        {
            health -= damage;
            damageDealt = damage;
        }

        return damageDealt;
    }

    public void Heal(float amount)
    {
        health += amount;
        if (health > maxHealth)
        {
            health = maxHealth;
        }
    }

    public void Kill()
    {
        isDead = true;
        if (deathSound != null)
        {
            AudioManager.Instance.Play(deathSound.name);
        }

        if (deathParticles != null)
        {
            GetComponent<SpriteRenderer>().enabled = false;
            deathParticles.Play();
        }

        // Clear damage particles
        if (damageParticles != null)
        {
            damageParticles.Stop();
            damageParticles.Clear();
        }

        // if there are any events, fire them, otherwise destroy the object
        if (OnDeath != null)
        {
            OnDeath(this);
        }
        else
        {
            // Destroy only if there is no death particles (otherwise it will be destroyed by the particle system)
            if (deathParticles == null)
            {
                Destroy(gameObject);
            }

        }

    }

    public void Revive()
    {
        isDead = false;
        health = maxHealth;

        if (OnRevive != null)
        {
            OnRevive(this);
        }

        // reset particles
        /* if (damageParticles != null)
            damageParticles.Clear();

        if (deathParticles != null)
            deathParticles.Clear(); */
    }

    public void SetMaxHealth(float amount)
    {
        maxHealth = amount;
    }

    public void SetHealth(float amount)
    {
        health = amount;
    }

    /* void OnDestroy()
    {
        // call death event
        if (!isDead && OnDeath != null)
        {
            OnDeath(this);
        }
    } */

}
