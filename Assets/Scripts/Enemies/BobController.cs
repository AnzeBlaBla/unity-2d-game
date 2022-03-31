using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BobController : MonoBehaviour
{
    public float shootInterval = 0.5f;
    public List<Transform> shootFromPoints;
    public GameObject bulletPrefab;
    public float bulletSpeed = 10f;
    public float bulletDamage = 2f;

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
                    SpawnBullet(point);
                }
            }
        }

    }


    void SpawnBullet(Transform point)
    {
        // spawn bullet at point facing away from middle of gameobject
        BulletController.SpawnBullet(bulletPrefab, point.position, point.position - transform.position, bulletSpeed, bulletDamage);
    }
}
