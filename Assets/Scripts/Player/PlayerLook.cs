using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;





public class PlayerLook : MonoBehaviour
{
    // movement
    [Header("Rotation")]
    public float rotateSpeed = 10f;

    InputActions inputActions;
    PlayerMovement playerMovement;
    public Vector3 lastPointerPosition;
    void Awake()
    {
        GetComponent<KillableEntity>().OnDeath += OnDeath;
        GetComponent<KillableEntity>().OnRevive += OnRevive;

        playerMovement = GetComponent<PlayerMovement>();
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
        inputActions = InputController.Instance.inputActions;
    }
    void doRotate()
    {
        Vector3 lookAtPos = GetPointerPosition();
        if (lookAtPos == Vector3.zero)
        {
            return;
        }
        // Rotate towards mouse
        Vector2 direction = lookAtPos - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion desiredRotation = Quaternion.AngleAxis(angle, Vector3.forward);

        // snappy aim if not ingame
        if (GameManager.Instance.playing)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, Time.deltaTime * rotateSpeed);
        }
        else
        {
            transform.rotation = desiredRotation;
        }
    }
    void Update()
    {
        //doRotate();
        StartCoroutine(LateRotate()); // so that we can see if the pointer is over UI in this frame
    }

    IEnumerator LateRotate()
    {
        yield return null;
        doRotate();
    }

    public Vector3 GetPointerPosition()
    {
        Vector2 lookAtPos;
        // mobile input
#if UNITY_ANDROID || UNITY_IOS

        lookAtPos = inputActions.Touch.PrimaryTouchPosition.ReadValue<Vector2>();
        //Debug.Log("Touch position: " + lookAtPos);
        if (lookAtPos == Vector2.zero || playerMovement.pointerOverUI)
        {
            lookAtPos = lastPointerPosition;
        }
        else
        {
            lastPointerPosition = lookAtPos;
        }
#endif

#if UNITY_STANDALONE
        lookAtPos = inputActions.Player.MousePosition.ReadValue<Vector2>();
        //Debug.Log("Mouse position: " + lookAtPos);
#endif
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(lookAtPos.x, lookAtPos.y, 0));
        return new Vector3(worldPos.x, worldPos.y, ZPositions.player);

    }
}
