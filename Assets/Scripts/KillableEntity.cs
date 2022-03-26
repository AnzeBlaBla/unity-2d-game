using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class KillableEntity : MonoBehaviour
{
    public float health = 100f;
    public float maxHealth = 100f;
    public bool isDead { get; private set; }

    public bool takeBulletDamage = true;

    public Sound deathSound;
    public Sound damageSound;

    public ParticleSystem damageParticles;
    public ParticleSystem deathParticles;

    public event Action<KillableEntity> OnDeath;
    public void Damage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Kill();
        }
        Debug.Log("Damage: " + damage + " Health: " + health);
        if(damageSound != null)
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
            deathParticles.Play();
        }

        // if there are any events, fire them, otherwise destroy the object
        if (OnDeath != null)
        {
            OnDeath(this);
        } else {
            if(deathParticles != null)
            {
                Destroy(gameObject, deathParticles.main.duration);
            } else {
                Destroy(gameObject);
            }
            
        }

    }

    public void Revive()
    {
        isDead = false;
    }

    public void SetMaxHealth(float amount)
    {
        maxHealth = amount;
    }

    public void SetHealth(float amount)
    {
        health = amount;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("Trigger: " + takeBulletDamage + ", " + collision.gameObject.tag);
        if (collision.gameObject.tag == "Bullet")
        {
            if (takeBulletDamage)
            {
                BulletController bc = collision.gameObject.GetComponent<BulletController>();
                Damage(bc.damage);
                bc.Hit();
            }
        }
    }

}
