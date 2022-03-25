using System.Collections;
using System.Collections.Generic;
using UnityEngine;





public class PlayerController : MonoBehaviour
{
    // references
    public GameObject player;
    InputActions inputActions;
    public GameObject bulletPrefab;
    public GameObject positionPointerPrefab;
    GameObject positionPointer;


    // movement
    [Header("Movement")]
    public float moveSpeed = 5f;
    Vector3 desiredPosition;

    // Shooting
    [Header("Shooting")]
    public float shootingInterval = 0.5f;
    public float shootStartDelay = 0.2f;
    bool shootingInput = false;
    bool shooting = false;
    float shootingStartTime;
    float lastShotTime;


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
        new ChargeUpPoint() { numberOfBullets = 0, time = 0f, bulletDamage = 1f },
        new ChargeUpPoint() { numberOfBullets = 3, time = 0.5f, bulletDamage = 1f },
        new ChargeUpPoint() { numberOfBullets = 5, time = 1f, bulletDamage = 1f },
        new ChargeUpPoint() { numberOfBullets = 7, time = 1.5f, bulletDamage = 1f },
        new ChargeUpPoint() { numberOfBullets = 9, time = 2f, bulletDamage = 1f }
    };
    public float chargeUpSpread = 45f;
    public float chargeUpBulletSpeed = 25f;
    bool chargingUp = false;
    float chargeUpStartTime;
    float currentChargeUp = 0f;


    void Start()
    {
        inputActions = InputController.Instance.inputActions;
        inputActions.Player.Move.performed += ctx => Move();

        inputActions.Player.Shoot.started += ctx => ShootStart();
        inputActions.Player.Shoot.canceled += ctx => ShootEnd();

        inputActions.Player.ChargeUp.started += ctx => ChargeUpStart();
        inputActions.Player.ChargeUp.canceled += ctx => ChargeUpEnd();

        positionPointer = Instantiate(positionPointerPrefab);
        positionPointer.SetActive(false);

    }
    #region Shooting
    void ShootStart()
    {
        Debug.Log("ShootStart");
        shootingStartTime = Time.time;
        shootingInput = true;
    }
    void ShootEnd()
    {
        Debug.Log("ShootEnd");
        shootingInput = false;
        shooting = false;
    }
    #endregion

    #region ChargeUp
    void ChargeUpStart()
    {
        Debug.Log("ChargeUpStart");
        chargeUpStartTime = Time.time;
        chargingUp = true;
    }
    void ChargeUpEnd()
    {
        Debug.Log("ChargeUpEnd");
        if (chargingUp)
        {
            chargingUp = false;

            ShootChargeUp();
        }
    }
    #endregion

    void Move()
    {
        desiredPosition = MousePositionToWorldPoint();
        positionPointer.transform.position = new Vector3(desiredPosition.x, desiredPosition.y, 90);
        positionPointer.SetActive(true);
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
                Vector2 bulletDirection = (MousePositionToWorldPoint() - player.transform.position).normalized;
                SpawnBullet(bulletDirection);
            }
        }
    }
    void SpawnBullet(Vector2 direction, float speed = 10f, float damage = 1f)
    {
        GameObject bullet = Instantiate(bulletPrefab, new Vector3(transform.position.x, transform.position.y, 10), Quaternion.identity);
        BulletController bc = bullet.GetComponent<BulletController>();
        bc.speed = speed;
        bc.damage = damage;
        bc.direction = direction;
    }
    void doChargeUp()
    {
        if (chargingUp)
        {
            currentChargeUp = Time.time - chargeUpStartTime;
            Debug.Log("ChargeUp: " + currentChargeUp);
        }
        if (currentChargeUp >= chargeUpPoints[1].time && chargingUp)
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
        Debug.Log("ShootChargeUp");
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
        Vector2 middleDirection = (MousePositionToWorldPoint() - player.transform.position).normalized;

        // calculate angle between spawned bullets
        float angle = chargeUpSpread / (chargeUpPoint.numberOfBullets - 1);
        Debug.Log("Angle: " + angle);
        // spawn bullets
        for (float curAngle = -chargeUpSpread / 2; curAngle <= chargeUpSpread / 2; curAngle += angle)
        {
            Debug.Log("SpawnBullet");
            SpawnBullet(Quaternion.Euler(0, 0, curAngle) * middleDirection, chargeUpBulletSpeed, chargeUpPoint.bulletDamage);
        }
        // reset charge up
        currentChargeUp = 0f;

    }
    void doMove()
    {
        if (shooting || chargingUp)
        {
            return;
        }


        // if reached desired position, stop moving
        if (Vector3.Distance(player.transform.position, desiredPosition) < 0.1f)
        {
            positionPointer.SetActive(false);
        }
        else
        {
            // move towards desired position
            player.transform.position = Vector2.MoveTowards(player.transform.position, desiredPosition, Time.deltaTime * moveSpeed);
        }
    }
    void doRotate()
    {
        // Rotate towards mouse
        Vector2 direction = MousePositionToWorldPoint() - player.transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        player.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
    void Update()
    {
        doShoot();
        doChargeUp();
        doMove();
        doRotate();
    }

    Vector3 MousePositionToWorldPoint()
    {
        Vector2 mousePos = inputActions.Player.MousePosition.ReadValue<Vector2>();
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 0));
        return new Vector3(worldPos.x, worldPos.y, 0);
    }
}
