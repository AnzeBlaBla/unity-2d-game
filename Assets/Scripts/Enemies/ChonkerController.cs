using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChonkerController : MonoBehaviour
{
    public float launchInterval = 1f;
    public float launchIntervalVariance = 0.5f;

    public float launchSpeed = 10f;
    public float launchTime = 0.1f;


    Rigidbody2D rb;
    EnemyController ec;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        ec = GetComponent<EnemyController>();
    }

    void Start()
    {
        StartCoroutine(LaunchLoop());
    }

    IEnumerator LaunchLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(launchInterval + Random.Range(-launchIntervalVariance, launchIntervalVariance));
            yield return Launch();
        }
    }

    IEnumerator Launch()
    {
        AudioManager.Instance.Play("ChonkerLaunch");
        ec.currentMovementSpeed = launchSpeed;
        yield return StartCoroutine(StopLaunching());
    }

    IEnumerator StopLaunching()
    {
        yield return new WaitForSeconds(launchTime);
        ec.currentMovementSpeed = ec.movementSpeed;
    }
}
