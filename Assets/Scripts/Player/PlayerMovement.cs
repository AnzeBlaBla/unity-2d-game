using System.Collections;
using System.Collections.Generic;
using UnityEngine;





public class PlayerMovement : MonoBehaviour
{
    public GameObject positionPointerPrefab;

    // movement
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float rotateSpeed = 10f;
    Vector3 desiredPosition;

    // Shooting
    [Header("Constraints")]
    public bool disableMoveWhenShooting = false;
    public bool disableMoveWhenChargingUp = false;

    InputActions inputActions;
    GameObject positionPointer;
    bool reachedDesiredPosition = true;
    Rigidbody2D rb;
    PlayerShooting playerShooting;


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerShooting = GetComponent<PlayerShooting>();

        positionPointer = Instantiate(positionPointerPrefab);
    }

    void Start()
    {
        inputActions = InputController.Instance.inputActions;
        inputActions.Player.Move.performed += ctx => Move();

        Reset();
    }

    public void Reset()
    {
        reachedDesiredPosition = true;
        if (positionPointer.activeSelf)
            positionPointer.SetActive(false);
    }
    void Move()
    {
        desiredPosition = MousePositionToWorldPoint();
        positionPointer.transform.position = new Vector3(desiredPosition.x, desiredPosition.y, ZPositions.positionPointer);
        positionPointer.SetActive(true);
        reachedDesiredPosition = false;
    }
    void doMove()
    {
        if (playerShooting.shooting && disableMoveWhenShooting)
        {
            return;
        }
        if (playerShooting.chargingUp && disableMoveWhenChargingUp)
        {
            return;
        }
        if (reachedDesiredPosition)
        {
            rb.velocity = Vector2.zero;
            return;
        }


        // if reached desired position, stop moving
        if (Vector3.Distance(transform.position, desiredPosition) < 0.1f)
        {
            rb.velocity = Vector2.zero;
            reachedDesiredPosition = true;
            positionPointer.SetActive(false);
        }
        else
        {
            // old movement: move towards desired position
            //player.transform.position = Vector2.MoveTowards(player.transform.position, desiredPosition, Time.deltaTime * moveSpeed);

            // new, physics-based movement
            rb.velocity = (desiredPosition - transform.position).normalized * moveSpeed;


        }
    }
    void doRotate()
    {
        // Rotate towards mouse
        Vector2 direction = MousePositionToWorldPoint() - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion desiredRotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, Time.deltaTime * rotateSpeed);
    }
    void Update()
    {
        doRotate();
    }

    void FixedUpdate()
    {
        doMove();
    }

    Vector3 MousePositionToWorldPoint()
    {
        Vector2 mousePos = inputActions.Player.MousePosition.ReadValue<Vector2>();
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 0));
        return new Vector3(worldPos.x, worldPos.y, 0);
    }
}
