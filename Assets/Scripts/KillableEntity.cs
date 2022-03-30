using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class KillableEntity : MonoBehaviour
{
    [HideInInspector]
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

    void Awake()
    {
        Revive();
    }
    public void Damage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            health = 0;
            Kill();
        }
        //Debug.Log("Damage: " + damage + " Health: " + health);
        if (damageSound != null)
        {
            AudioManager.Instance.Play(damageSound.name);
        }
        if (damageParticles != null)
        {
            damageParticles.Play();
        }
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
    }

    public void SetMaxHealth(float amount)
    {
        maxHealth = amount;
    }

    public void SetHealth(float amount)
    {
        health = amount;
    }

}
