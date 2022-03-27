using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChargeUpDisplay : MonoBehaviour
{
    PlayerShooting ps;
    public Image bar;
    float minChargeUp;
    float maxChargeUp;
    
    void Start()
    {
        ps = GameObjectRegistry.Instance.player.GetComponent<PlayerShooting>();
        minChargeUp = ps.chargeUpPoints[1].time;
        maxChargeUp = ps.chargeUpPoints[ps.chargeUpPoints.Count - 1].time;
    }
    void Update()
    {
        bar.fillAmount = (ps.currentChargeUp - minChargeUp) / (maxChargeUp - minChargeUp);
    }
}
