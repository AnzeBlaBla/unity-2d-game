using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeDisplay : Singleton<TimeDisplay>
{
    TextMeshProUGUI text;
    string formatText;

    float gameStartTime;

    public float timeAlive;
    void Start()
    {
        gameStartTime = Time.time;

        text = GetComponent<TextMeshProUGUI>();
        
        formatText = text.text;
    }
    void Update()
    {
        if(Time.timeScale == 0f)
        {
            return;
        }
        timeAlive = Mathf.RoundToInt((Time.time - gameStartTime) * 100f) / 100f;
        text.text = string.Format(formatText, Mathf.FloorToInt(timeAlive));
    }
}
