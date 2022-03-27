using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DeathScreen : MonoBehaviour
{
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

        gameObject.SetActive(false);
    }

    void OnDeath(KillableEntity ke)
    {
        Time.timeScale = 0f;

        gameObject.SetActive(true);
        float aliveTime = TimeDisplay.Instance.timeAlive;
        timeText.text = string.Format(formatText, aliveTime);
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        gameObject.SetActive(false);
        GameManager.Instance.Restart();
    }
}
