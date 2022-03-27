using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HPDisplay : MonoBehaviour
{
    KillableEntity playerKe;
    TextMeshProUGUI text;
    string formatText;
    void Start()
    {
        playerKe = GameObjectRegistry.Instance.player.GetComponent<KillableEntity>();
        text = GetComponent<TextMeshProUGUI>();
        
        formatText = text.text;
    }
    void Update()
    {
        text.text = string.Format(formatText, playerKe.health);
    }
}
