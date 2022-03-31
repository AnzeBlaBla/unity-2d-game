using System.Collections;
using System.Collections.Generic;
using UnityEngine;





public class PlayerShooting : MonoBehaviour
{
    InputActions inputActions;
    public GameObject bulletPrefab;



    // Shooting
    [Header("Shooting")]
    public Transform bulletSpawnPosition;
    public float shootingInterval = 0.25f;
    public float shootStartDelay = 0f;
    public float bulletDirectionRandomness = 0.1f;
    bool shootingInput = false;
    [HideInInspector]
    public bool shooting = false;
    float shootingStartTime;
    float lastShotTime;

    [Header("Constraints")]
    public bool stopShootingOnChargeUp = true;



    // Charge up
    [System.Serializable]
    public struct ChargeUpPoint
    {
        public int numberOfBullets;
        public float time;
        public float bulletDamage;
    }
    [Header("Charge Up")]

    public List<ChargeUpPoint> chargeUpPoints = new List<ChargeUpPoint>()
    {
        new ChargeUpPoint() { numberOfBullets = 0, time = 0f, bulletDamage = 0f },
        new ChargeUpPoint() { numberOfBullets = 3, time = 0.5f, bulletDamage = 1f },
        new ChargeUpPoint() { numberOfBullets = 5, time = 1.5f, bulletDamage = 1f },
        new ChargeUpPoint() { numberOfBullets = 7, time = 3f, bulletDamage = 1f },
        new ChargeUpPoint() { numberOfBullets = 9, time = 5f, bulletDamage = 1f }
    };
    public float chargeUpSpread = 45f;
    public float chargeUpBulletSpeed = 25f;
    [HideInInspector]
    public bool chargingUp = false;
    float chargeUpStartTime;
    [HideInInspector]
    public float currentChargeUp = 0f;

    void Start()
    {
        inputActions = InputController.Instance.inputActions;

        inputActions.Player.Shoot.started += ctx => ShootStart();
        inputActions.Player.Shoot.canceled += ctx => ShootEnd();

        inputActions.Player.ChargeUp.started += ctx => ChargeUpStart();
        inputActions.Player.ChargeUp.canceled += ctx => ChargeUpEnd();


    }
    #region Shooting
    void ShootStart()
    {
        //Debug.Log("ShootStart");
        shootingStartTime = Time.time;
        shootingInput = true;
    }
    void ShootEnd()
    {
        //Debug.Log("ShootEnd");
        shootingInput = false;
        shooting = false;
    }

    void doShoot()
    {
        if (Time.time - shootingStartTime > shootStartDelay && shootingInput)
        {
            shooting = true;
        }
        else
        {
            return;
        }

        if (shooting)
        {
            if (Time.time - lastShotTime > shootingInterval)
            {
                lastShotTime = Time.time;
                AudioManager.Instance.Play("Shoot");

                // spawn a bullet facing where the player is looking
                SpawnBullet(transform.right);
            }
        }
    }
    #endregion

    #region ChargeUp
    void ChargeUpStart()
    {
        //Debug.Log("ChargeUpStart");
        chargeUpStartTime = Time.time;
        chargingUp = true;
    }
    void ChargeUpEnd()
    {
        //Debug.Log("ChargeUpEnd");
        if (chargingUp)
        {
            chargingUp = false;
            ShootChargeUp();
        }
        currentChargeUp = 0f;
    }

    void doChargeUp()
    {
        if (chargingUp)
        {
            currentChargeUp = Time.time - chargeUpStartTime;
            //Debug.Log("ChargeUp: " + currentChargeUp);
        }
        if (currentChargeUp >= chargeUpPoints[1].time && chargingUp && stopShootingOnChargeUp)
        {
            shootingInput = false;
        }
        if (currentChargeUp > chargeUpPoints[chargeUpPoints.Count - 1].time)
        {
            ShootChargeUp();
            chargingUp = false;
            currentChargeUp = 0f;
        }
    }
    void ShootChargeUp()
    {

        // find charge up point that was last reached by the time
        ChargeUpPoint chargeUpPoint = chargeUpPoints[0];
        for (int i = 0; i < chargeUpPoints.Count; i++)
        {
            if (chargeUpPoints[i].time < currentChargeUp)
            {
                chargeUpPoint = chargeUpPoints[i];
            }
        }

        if (chargeUpPoint.numberOfBullets == 0)
        {
            return;
        }
        Vector2 middleDirection = (MousePositionToWorldPoint() - transform.position).normalized;

        // calculate angle between spawned bullets
        float angle = chargeUpSpread / (chargeUpPoint.numberOfBullets - 1);

        // spawn bullets
        for (float curAngle = -chargeUpSpread / 2; curAngle <= chargeUpSpread / 2; curAngle += angle)
        {
            SpawnBullet(Quaternion.Euler(0, 0, curAngle) * middleDirection, chargeUpBulletSpeed, chargeUpPoint.bulletDamage);
        }

        AudioManager.Instance.Play("ShootChargeUp");
    }
    #endregion

    void SpawnBullet(Vector2 direction, float speed = 10f, float damage = 1f)
    {
        BulletController.SpawnBullet(bulletPrefab, bulletSpawnPosition.position, direction + Random.insideUnitCircle * bulletDirectionRandomness, speed, damage);
    }

    void Update()
    {
        doShoot();
        doChargeUp();
    }

    Vector3 MousePositionToWorldPoint()
    {
        Vector2 mousePos = inputActions.Player.MousePosition.ReadValue<Vector2>();
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 0));
        return new Vector3(worldPos.x, worldPos.y, 0);
    }
}
