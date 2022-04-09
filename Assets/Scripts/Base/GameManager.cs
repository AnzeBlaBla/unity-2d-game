using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    public GameObject spawnPosition;
    public GameObject mainMenu;
    GameObject player;

    public bool playing = false;

    void Start()
    {
        player = GameObjectRegistry.Instance.player;

        player.GetComponent<KillableEntity>().OnDeath += OnDeath;
    }

    void OnDeath(KillableEntity ke)
    {
        playing = false;
    }

    public void StartGame()
    {
        mainMenu.SetActive(false);

        PlatformUI.Instance.HUDContainer.SetActive(true);

        RestartGame();
    }

    void ClearGame()
    {
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
    }

    void SpawnPlayer()
    {
        player.transform.position = spawnPosition.transform.position;
        player.transform.rotation = spawnPosition.transform.rotation;

        player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        player.GetComponent<Rigidbody2D>().angularVelocity = 0f;

        player.GetComponent<PlayerShooting>().currentChargeUp = 0f;

        player.GetComponent<PlayerMovement>().Reset();
    }

    public void RestartGame()
    {
        playing = true;

        ClearGame();

        SpawnPlayer();

        HUDManager.Instance.ResetTime();

        EnemySpawner.Instance.StartSpawning();


        // queue the revive on the next frame
        StartCoroutine(RevivePlayer());
    }

    IEnumerator RevivePlayer()
    {
        yield return null;
        player.GetComponent<KillableEntity>().Revive();
    }

    public void OpenSettings()
    {
        PlatformUI.Instance.settingsContainer.SetActive(true);
    }


    public void Quit()
    {
        Application.Quit();
    }

    public void MainMenu()
    {
        playing = false;
        
        PlatformUI.Instance.HUDContainer.SetActive(false);
        ClearGame();
        EnemySpawner.Instance.StopSpawning();
        SpawnPlayer();

        mainMenu.SetActive(true);

        StartCoroutine(RevivePlayer());
    }

}
