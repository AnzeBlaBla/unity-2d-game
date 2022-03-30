using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BobController : MonoBehaviour
{
    public float shootInterval = 0.5f;
    public List<Transform> shootFromPoints;

    float lastShootTime = 0f;

    void Update()
    {
        if (shootFromPoints.Count > 0)
        {
            if (Time.time - lastShootTime > shootInterval)
            {
                lastShootTime = Time.time;
                foreach (var point in shootFromPoints)
                {
                    // TODO: spawn bullet
                }
            }
        }
        
    }
}
