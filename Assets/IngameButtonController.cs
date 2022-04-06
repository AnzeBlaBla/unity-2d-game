using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 using UnityEngine.Events;

public class IngameButtonController : MonoBehaviour
{
    public UnityEvent OnClick;
    KillableEntity ke;

    public float pressDelay = 2f;

    void Awake()
    {
        ke = GetComponent<KillableEntity>();

        ke.OnDamage += OnDamage;
    }

    void OnDamage(KillableEntity entity)
    {
        entity.SetHealth(1000); // heal back
        //Debug.Log("Button pressed");
        StartCoroutine(WaitButtonPressed());
    }

    IEnumerator WaitButtonPressed()
    {
        yield return new WaitForSeconds(pressDelay);
        //Debug.Log("Logic would run here");

        OnClick.Invoke();
    }

}
