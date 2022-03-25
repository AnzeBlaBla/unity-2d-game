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
    public float moveSpeed = 5f;
    Vector3 desiredPosition;

    // Shooting
    bool shootingInput = false;
    bool shooting = false;
    float shootingStartTime;
    float lastShotTime;
    public float shootingInterval = 0.5f;
    public float shootStartDelay = 0.2f;

    // Charge up
    bool chargingUp = false;
    float chargeUpStartTime;

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
        chargingUp = false;
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
                Vector2 bulletDireciton = (MousePositionToWorldPoint() - player.transform.position).normalized;
                SpawnBullet(bulletDireciton);
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
        //Debug.Log("ChargeUp: " + chargingUp);
    }
    void doMove()
    {
        if(shooting || chargingUp)
        {
            return;
        }
        // move towards desired position
        player.transform.position = Vector2.MoveTowards(player.transform.position, desiredPosition, Time.deltaTime * moveSpeed);

        // if reached desired position, stop moving
        if (Vector3.Distance(player.transform.position, desiredPosition) < 0.1f)
        {
            positionPointer.SetActive(false);
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
