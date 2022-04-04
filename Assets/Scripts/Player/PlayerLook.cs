using System.Collections;
using System.Collections.Generic;
using UnityEngine;





public class PlayerLook : MonoBehaviour
{
    // movement
    [Header("Rotation")]
    public float rotateSpeed = 10f;

    InputActions inputActions;
    public Vector3 lastPointerPosition;

    void Awake()
    {
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
        inputActions = InputController.Instance.inputActions;
    }
    void doRotate()
    {
        // Rotate towards mouse
        Vector2 direction = GetPointerPosition() - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion desiredRotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, Time.deltaTime * rotateSpeed);
    }
    void Update()
    {
        doRotate();
    }

    public Vector3 GetPointerPosition()
    {
        // mobile input
#if UNITY_ANDROID || UNITY_IOS
        Vector2 lookAtPos = inputActions.Touch.PrimaryTouchPosition.ReadValue<Vector2>();
        Debug.Log("Touch position: " + lookAtPos);
        if(lookAtPos == Vector2.zero)
        {
            lookAtPos = lastPointerPosition;
        } else {
            lastPointerPosition = lookAtPos;
        }
#endif

#if UNITY_STANDALONE
        Vector2 lookAtPos = inputActions.Player.MousePosition.ReadValue<Vector2>();
        Debug.Log("Mouse position: " + mousePos);
#endif
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(lookAtPos.x, lookAtPos.y, 0));
        return new Vector3(worldPos.x, worldPos.y, ZPositions.player);

    }
}
