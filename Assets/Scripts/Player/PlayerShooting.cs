using System.Collections;
using System.Collections.Generic;
using UnityEngine;





public class PlayerShooting : MonoBehaviour
{
    InputActions inputActions;
    PlayerLook playerLook;
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

    public ParticleSystem shootParticles;

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

    /* public List<ChargeUpPoint> chargeUpPoints = new List<ChargeUpPoint>()
    {
        new ChargeUpPoint() { numberOfBullets = 0, time = 0f, bulletDamage = 0f },
        new ChargeUpPoint() { numberOfBullets = 3, time = 0.5f, bulletDamage = 1f },
        new ChargeUpPoint() { numberOfBullets = 5, time = 1.5f, bulletDamage = 1f },
        new ChargeUpPoint() { numberOfBullets = 7, time = 3f, bulletDamage = 1f },
        new ChargeUpPoint() { numberOfBullets = 9, time = 5f, bulletDamage = 1f }
    };
    public float chargeUpSpread = 45f;
    public float chargeUpBulletSpeed = 25f; */
    public float dashTime = 0.5f;
    public float dashStrength = 10f;
    public float maxChargeUpTime = 5f;
    public float dashDamageMult = 1f;
    public float dashDamage = 0f;
    public bool isDashing = false;
    [HideInInspector]
    public bool chargingUp = false;
    float chargeUpStartTime;
    [HideInInspector]
    public float currentChargeUp = 0f;

    void Awake()
    {
        playerLook = GetComponent<PlayerLook>();
    }

    void Start()
    {
        inputActions = InputController.Instance.inputActions;

        inputActions.Player.Shoot.started += ctx => ShootStart();
        inputActions.Player.Shoot.canceled += ctx => ShootEnd();

        inputActions.Player.ChargeUp.started += ctx => ChargeUpStart();
        inputActions.Player.ChargeUp.canceled += ctx => ChargeUpEnd();


    }
    #region Shooting
    public void ShootStart()
    {
        //Debug.Log("ShootStart");
        shootingStartTime = Time.time;
        shootingInput = true;
    }
    public void ShootEnd()
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
    public void ChargeUpStart()
    {
        Debug.Log("ChargeUpStart");
        chargeUpStartTime = Time.time;
        chargingUp = true;
    }
    public void ChargeUpEnd()
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
        /* if (currentChargeUp >= chargeUpPoints[1].time && chargingUp && stopShootingOnChargeUp)
        {
            shootingInput = false;
        }
        if (currentChargeUp > chargeUpPoints[chargeUpPoints.Count - 1].time)
        {
            ShootChargeUp();
            chargingUp = false;
            currentChargeUp = 0f;
        } */
        if(currentChargeUp > maxChargeUpTime)
        {
            ShootChargeUp();
            chargingUp = false;
            currentChargeUp = 0f;
        }
    }
    void ShootChargeUp()
    {
        // Old code, for shooting multiple buillets (shotgun)
        /* // find charge up point that was last reached by the time
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
        Vector2 middleDirection = (playerLook.GetPointerPosition() - transform.position).normalized;

        // calculate angle between spawned bullets
        float angle = chargeUpSpread / (chargeUpPoint.numberOfBullets - 1);

        // spawn bullets
        for (float curAngle = -chargeUpSpread / 2; curAngle <= chargeUpSpread / 2; curAngle += angle)
        {
            SpawnBullet(Quaternion.Euler(0, 0, curAngle) * middleDirection, chargeUpBulletSpeed, chargeUpPoint.bulletDamage);
        }

        AudioManager.Instance.Play("ShootChargeUp"); */


        // New code - dash player forward for charge up amound
        Vector2 dashDirection = (playerLook.GetPointerPosition() - transform.position).normalized;
        StartCoroutine(DoDash());


    }

    // Dash forward
    IEnumerator DoDash()
    {
        isDashing = true;
        dashDamage = currentChargeUp * dashDamageMult;
        PlayerMovement playerMovement = GetComponent<PlayerMovement>();
        playerMovement.enabled = false;

        float chargeUpPercent = currentChargeUp / maxChargeUpTime;

        float dashStartTime = Time.time;
        float dashEndTime = Time.time + (dashTime * chargeUpPercent);
        float lerpTime = 0f;
        Vector2 dashStartPosition = transform.position;
        Vector2 dashEndPosition = transform.position + transform.right * dashStrength * chargeUpPercent;

        while (Time.time < dashEndTime)
        {
            lerpTime = (Time.time - dashStartTime) / dashTime;
            transform.position = Vector2.Lerp(dashStartPosition, dashEndPosition, lerpTime);
            yield return null;
        }

        playerMovement.enabled = true;
        isDashing = false;
    }
    #endregion

    void SpawnBullet(Vector2 direction, float speed = 10f, float damage = 1f)
    {
        ScreenShake.Instance.Shake(0.1f, 0.1f, 0.5f);

        BulletController.SpawnBullet(bulletPrefab, bulletSpawnPosition.position, direction + Random.insideUnitCircle * bulletDirectionRandomness, speed, damage);

        if (shootParticles != null)
        {
            shootParticles.Play();
        }
    }

    void Update()
    {
        doShoot();
        doChargeUp();
    }

}
