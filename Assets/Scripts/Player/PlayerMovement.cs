using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;





public class PlayerMovement : MonoBehaviour
{
    public GameObject positionPointerPrefab;

    // movement
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float rotateSpeed = 10f;
    public Sound moveSound;
    AudioSource moveAudioSource;
    Vector3 desiredPosition;

    // Shooting
    [Header("Constraints")]
    public bool disableMoveWhenShooting = false;
    public bool disableMoveWhenChargingUp = false;
    public GameObject boundsObject;
    Bounds moveBounds;

    InputActions inputActions;
    GameObject positionPointer;
    bool reachedDesiredPosition = true;
    Rigidbody2D rb;
    PlayerShooting playerShooting;
    PlayerLook playerLook;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerShooting = GetComponent<PlayerShooting>();
        playerLook = GetComponent<PlayerLook>();

        positionPointer = Instantiate(positionPointerPrefab);

        GetComponent<KillableEntity>().OnDeath += OnDeath;
        GetComponent<KillableEntity>().OnRevive += OnRevive;
    }

    void OnDeath(KillableEntity ke)
    {
        // stop this script
        enabled = false;
    }

    void OnRevive(KillableEntity ke)
    {
        // Start this script
        enabled = true;
    }

    void Start()
    {
        moveBounds = boundsObject.GetComponent<SpriteRenderer>().bounds;

        inputActions = InputController.Instance.inputActions;


#if UNITY_STANDALONE
        inputActions.Player.Move.performed += ctx => Move();
#endif

#if UNITY_ANDROID || UNITY_IOS
        inputActions.Touch.PrimaryTouch.performed += ctx => Move();
#endif

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
        if (!enabled)
            return;

#if UNITY_ANDROID || UNITY_IOS
        // if not clicking ui
        if (EventSystem.current.IsPointerOverGameObject())
            return;
#endif

        Vector3 clickedPosition = playerLook.GetPointerPosition();

        if (!moveBounds.Contains(clickedPosition))
            return;

        desiredPosition = clickedPosition;

        positionPointer.transform.position = new Vector3(desiredPosition.x, desiredPosition.y, ZPositions.positionPointer);
        positionPointer.SetActive(true);
        reachedDesiredPosition = false;

        // play move sound
        if (moveSound != null && moveAudioSource == null)
        {
            moveAudioSource = AudioManager.Instance.Play(moveSound);
        }
        AudioManager.Instance.Play("UIClick");
    }
    void doMove()
    {
        rb.angularVelocity = 0; // stop rotating

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

            // stop move sound
            if (moveAudioSource != null)
            {
                AudioManager.Instance.Stop(moveAudioSource);
                moveAudioSource = null;
            }
        }
        else
        {
            // old movement: move towards desired position
            //player.transform.position = Vector2.MoveTowards(player.transform.position, desiredPosition, Time.deltaTime * moveSpeed);

            // new, physics-based movement
            rb.velocity = (desiredPosition - transform.position).normalized * moveSpeed;


        }
    }

    void FixedUpdate()
    {
        doMove();
    }

    Vector3 MousePositionToWorldPoint()
    {
        Vector2 mousePos = inputActions.Player.MousePosition.ReadValue<Vector2>();
        Debug.Log("Mouse position: " + mousePos);
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 0));
        return new Vector3(worldPos.x, worldPos.y, ZPositions.player);
    }
}
