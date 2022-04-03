using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DeathScreen : MonoBehaviour
{
    public GameObject overlay;
    public GameObject timeTextObject;

    KillableEntity playerKe;
    TextMeshProUGUI timeText;
    string formatText;
    void Start()
    {
        playerKe = GameObjectRegistry.Instance.player.GetComponent<KillableEntity>();
        timeText = timeTextObject.GetComponent<TextMeshProUGUI>();

        formatText = timeText.text;

        playerKe.OnDeath += OnDeath;

        overlay.SetActive(false);
    }

    void OnDeath(KillableEntity ke)
    {
        Time.timeScale = 0f;

        AudioManager.Instance.StopAllSounds(true);

        // stop sounds on all enemies
        var enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var enemy in enemies)
        {
            enemy.GetComponent<AudioSource>().Stop();
        }


        overlay.SetActive(true);
        float aliveTime = TimeDisplay.Instance.timeAlive;
        timeText.text = string.Format(formatText, aliveTime);
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        overlay.SetActive(false);
        GameManager.Instance.Restart();
    }
}
