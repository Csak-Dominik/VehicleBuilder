using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private Camera cam;

    [SerializeField]
    private float moveSpeed = 10f;

    [SerializeField]
    private float moveForce = 10f;

    [SerializeField]
    private float lookSpeed = 10f;

    [SerializeField]
    private float normalDrag = 0f;

    [SerializeField]
    private float stoppingDrag = 10f;

    [SerializeField]
    private bool isGrounded = false;

    [SerializeField]
    [Range(-90f, 0)]
    private float minAngle = -90f;

    [SerializeField]
    [Range(0, 90f)]
    private float maxAngle = 90f;

    public bool IsGrounded
    { get { return isGrounded; } set { isGrounded = value; } }

    // Controls
    [SerializeField]
    private Vector2 moveVector;

    [SerializeField]
    private Vector2 lookVector;

    [SerializeField]
    private bool alt;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        cam = Camera.main;
    }

    private void FixedUpdate()
    {
        if (alt) Cursor.lockState = CursorLockMode.None;
        else Cursor.lockState = CursorLockMode.Locked;

        Move();
        Look();

        Debug.Log(UnityAngleConverter(cam.transform.localEulerAngles.x));
    }

    private void Move()
    {
        if (rb.velocity.magnitude < moveSpeed)
        {
            rb.AddRelativeForce(new Vector3(moveVector.x, 0, moveVector.y) * moveForce, ForceMode.VelocityChange);
        }

        if (moveVector == Vector2.zero && isGrounded)
        {
            rb.drag = stoppingDrag;
        }
        else
        {
            // Calculate dot product of moveVector and velocity
            float dot = Vector2.Dot(moveVector, new Vector2(rb.velocity.x, rb.velocity.z).normalized);

            // if the dot is greater than 0.8 then apply normal drag
            // else lerp between normal drag and stopping drag based on dot
            if (dot > 0.8f) rb.drag = normalDrag;
            else rb.drag = Mathf.Lerp(stoppingDrag, normalDrag, Helper.MapRange(dot, -1, 0.8f, stoppingDrag, normalDrag));
        }
    }

    private void Look()
    {
        transform.Rotate(new Vector3(0, lookVector.x, 0) * lookSpeed * Time.fixedDeltaTime);

        var rotation = -lookVector.y * lookSpeed * Time.fixedDeltaTime;

        var currentAngle = -UnityAngleConverter(cam.transform.localEulerAngles.x);
        var allowedAngleChange = Mathf.Clamp(currentAngle + rotation, minAngle, maxAngle) - currentAngle;

        cam.transform.Rotate(Vector3.right * allowedAngleChange);
    }

    private float UnityAngleConverter(float angle)
    {
        if (angle > 180) return -(angle - 360);
        else return -angle;
    }

    #region EventHandlers

    private void SubscribeEvents()
    {
        InputManager.Instance.OnMove += ctx => moveVector = ctx;
        InputManager.Instance.OnLook += ctx => lookVector = ctx;
        InputManager.Instance.OnAlt += ctx => alt = ctx;
    }

    private void UnsubscribeEvents()
    {
        InputManager.Instance.OnMove -= ctx => moveVector = ctx;
        InputManager.Instance.OnLook -= ctx => lookVector = ctx;
        InputManager.Instance.OnAlt -= ctx => alt = ctx;
    }

    private void OnEnable()
    {
        SubscribeEvents();
    }

    private void OnDisable()
    {
        UnsubscribeEvents();
    }

    private void OnDestroy()
    {
        UnsubscribeEvents();
    }

    #endregion EventHandlers
}