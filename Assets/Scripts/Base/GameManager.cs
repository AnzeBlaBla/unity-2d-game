using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    public GameObject spawnPosition;
    GameObject player;

    void Start()
    {
        player = GameObjectRegistry.Instance.player;
        Restart();
    }

    public void Restart()
    {
        TimeDisplay.Instance.ResetTime();
        // kill all enemies
        var enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var enemy in enemies)
        {
            Destroy(enemy);
        }
        var bullets = GameObject.FindGameObjectsWithTag("Bullet");
        foreach (var bullet in bullets)
        {
            Destroy(bullet);
        }

        player.transform.position = spawnPosition.transform.position;
        player.transform.rotation = spawnPosition.transform.rotation;

        player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        player.GetComponent<Rigidbody2D>().angularVelocity = 0f;

        player.GetComponent<PlayerShooting>().currentChargeUp = 0f;



        player.GetComponent<PlayerMovement>().Reset();

        EnemySpawner.Instance.Restart();

        // queue the revive on the next frame
        StartCoroutine(RevivePlayer());
    }

    IEnumerator RevivePlayer()
    {
        yield return null;
        player.GetComponent<KillableEntity>().Revive();
    }

}
