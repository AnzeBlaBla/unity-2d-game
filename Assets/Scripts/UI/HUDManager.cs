using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class HUDManager : Singleton<HUDManager>
{
    public GameObject container;
    /* HP Display */
    KillableEntity playerKe;
    public TextMeshProUGUI HPText;
    string HPFormatText;


    /* Charge up */
    PlayerShooting ps;
    public Image bar;
    float minChargeUp;
    float maxChargeUp;


    /* Time text */
    public TextMeshProUGUI timeText;
    string timeFormatText;
    float gameStartTime;

    public float timeAlive;

    void Awake()
    {
        HPFormatText = HPText.text;
    }

    void Start()
    {
        ps = GameObjectRegistry.Instance.player.GetComponent<PlayerShooting>();
        minChargeUp = ps.chargeUpPoints[1].time;
        maxChargeUp = ps.chargeUpPoints[ps.chargeUpPoints.Count - 1].time;

        gameStartTime = Time.time;
        timeFormatText = timeText.text;

        playerKe = GameObjectRegistry.Instance.player.GetComponent<KillableEntity>();
        HPText = GetComponent<TextMeshProUGUI>();
    }
    void Update()
    {
        if (Time.timeScale == 0f || !container.activeSelf)
        {
            return;
        }
        timeAlive = Mathf.RoundToInt((Time.time - gameStartTime) * 100f) / 100f;
        timeText.text = string.Format(timeFormatText, Mathf.FloorToInt(timeAlive));

        HPText.text = string.Format(HPFormatText, playerKe.health);

        bar.fillAmount = (ps.currentChargeUp - minChargeUp) / (maxChargeUp - minChargeUp);
    }
    public void ResetTime()
    {
        gameStartTime = Time.time;
    }

}
