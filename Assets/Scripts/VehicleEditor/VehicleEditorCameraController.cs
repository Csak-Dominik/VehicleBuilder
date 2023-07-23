using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class VehicleEditorCameraController : MonoBehaviour
{
    private Camera cam;
    private Transform camTransform; // cache transform for performance

    [SerializeField]
    private float rotationSpeed = 20f;

    [SerializeField]
    private float panSpeed = 0.1f;

    [SerializeField]
    private float scrollSpeed = 0.1f;

    [SerializeField]
    private float freeMoveSpeed = 0.1f;

    [SerializeField]
    private bool leftMouseDown = false;

    [SerializeField]
    private bool rightMouseDown = false;

    [SerializeField]
    private bool middleMouseDown = false;

    [SerializeField]
    private float mouseScroll = 0f;

    [SerializeField]
    private bool focus = false;

    private bool prevFocus = false;

    [SerializeField]
    private Vector2 mouseDelta;

    [SerializeField]
    private Vector2 moveVector;

    [SerializeField]
    private bool shift;

    [SerializeField]
    private Vector3 orbitPos = Vector3.zero;

    [SerializeField]
    private float orbitResetDistance = 3f;

    [SerializeField]
    private float minDistance = 0.5f;

    [SerializeField]
    private float maxDistance = 10f;

    private bool prevOrbitMode = true;
    public bool orbitMode = true;

    private Coroutine cameraFocusMove;

    private void Awake()
    {
        cam = GetComponent<Camera>();
        camTransform = cam.transform;

        Cursor.lockState = CursorLockMode.None;
    }

    private void Update()
    {
        if (orbitMode) OrbitMode();
        else FreeMode();

        prevOrbitMode = orbitMode;
        prevFocus = focus;
    }

    private void OrbitMode()
    {
        if (!prevOrbitMode)
        {
            // set orbitPos to center of screen
            Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            FocusObjectSmooth(ray);
        }

        if (rightMouseDown && cameraFocusMove == null)
        {
            // rotate around orbitPos
            camTransform.RotateAround(orbitPos, Vector3.up, mouseDelta.x * rotationSpeed * Time.deltaTime);
            // roate around local x axis
            camTransform.RotateAround(orbitPos, camTransform.right, -mouseDelta.y * rotationSpeed * Time.deltaTime);
        }

        if (middleMouseDown && cameraFocusMove == null)
        {
            var camDist = Vector3.Distance(camTransform.position, orbitPos);

            float sensitivity = camDist / 10f;

            // pan camera
            var multiplier = panSpeed * sensitivity * Time.deltaTime;
            camTransform.Translate(-mouseDelta.x * multiplier, -mouseDelta.y * multiplier, 0, Space.Self);

            // keep orbitPos in front of camera
            orbitPos = camTransform.position + camTransform.forward * camDist;
        }

        if (mouseScroll != 0)
        {
            float scrollDist = mouseScroll * scrollSpeed * Time.deltaTime;
            var camDist = Vector3.Distance(camTransform.position, orbitPos);

            var allowedDist = Mathf.Clamp(camDist * (1 - scrollDist), minDistance, maxDistance);

            // move camera closer to orbitPos
            camTransform.position = orbitPos - camTransform.forward * allowedDist;
        }

        if (focus && !prevFocus)
        {
            // raycast from mouse screenpos
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            FocusObjectSmooth(ray, false);
        }
    }

    private void FreeMode()
    {
        if (rightMouseDown)
        {
            var multiplier = rotationSpeed * Time.deltaTime;
            // the top of the camera should always be up
            camTransform.Rotate(Vector3.up, mouseDelta.x * multiplier, Space.World);
            // roate around local x axis
            camTransform.Rotate(camTransform.right, -mouseDelta.y * multiplier, Space.World);
        }

        if (middleMouseDown && cameraFocusMove == null)
        {
            var camDist = Vector3.Distance(camTransform.position, orbitPos);

            float sensitivity = camDist / 10f;

            // pan camera
            var multiplier = panSpeed * sensitivity * Time.deltaTime;
            camTransform.Translate(-mouseDelta.x * multiplier, -mouseDelta.y * multiplier, 0, Space.Self);

            // keep orbitPos in front of camera
            orbitPos = camTransform.position + camTransform.forward * camDist;
        }

        if (moveVector != Vector2.zero)
        {
            // move camera
            var multiplier = freeMoveSpeed * Time.deltaTime;
            camTransform.Translate(moveVector.x * multiplier, 0, moveVector.y * multiplier, Space.Self);
        }

        if (mouseScroll != 0)
        {
            var multiplier = freeMoveSpeed * 0.25f * Time.deltaTime;
            camTransform.Translate(0, 0, mouseScroll / 120f * multiplier, Space.Self);
        }

        if (focus && !prevFocus)
        {
            // raycast from mouse screenpos
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            FocusObjectSmooth(ray, false);
        }
    }

    private void FocusObject(Ray ray, bool resetOnMiss = true)
    {
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            var collider = hit.collider;
            if (collider != null && collider.GetComponent<Part>() != null)
            {
                orbitPos = collider.transform.position;
                // move camera to orbitPos
                camTransform.position = orbitPos - camTransform.forward * orbitResetDistance;
            }
            else if (resetOnMiss)
            {
                orbitPos = camTransform.position + camTransform.forward * orbitResetDistance;
                // move camera to orbitPos
                //camTransform.position = orbitPos - camTransform.forward * orbitResetDistance;
            }
        }
        else if (resetOnMiss)
        {
            orbitPos = camTransform.position + camTransform.forward * orbitResetDistance;
            // move camera to orbitPos
            //camTransform.position = orbitPos - camTransform.forward * orbitResetDistance;
        }
    }

    // same as FocusObject but with smooth movement
    private void FocusObjectSmooth(Ray ray, bool resetOnMiss = true)
    {
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            var collider = hit.collider;
            if (collider != null && collider.GetComponent<Part>() != null)
            {
                orbitPos = collider.transform.position;
                // move camera to orbitPos
                // start coroutine to move camera to orbitPos
                if (cameraFocusMove != null) StopCoroutine(cameraFocusMove);
                cameraFocusMove = StartCoroutine("MoveCameraToOrbitPos");
            }
            else if (resetOnMiss)
            {
                orbitPos = camTransform.position + camTransform.forward * orbitResetDistance;
                // move camera to orbitPos
                // start coroutine to move camera to orbitPos
                //if (cameraFocusMove != null) StopCoroutine(cameraFocusMove);
                //cameraFocusMove = StartCoroutine("MoveCameraToOrbitPos");
            }
        }
        else if (resetOnMiss)
        {
            orbitPos = camTransform.position + camTransform.forward * orbitResetDistance;
            // move camera to orbitPos
            // start coroutine to move camera to orbitPos
            //if (cameraFocusMove != null) StopCoroutine(cameraFocusMove);
            //cameraFocusMove = StartCoroutine("MoveCameraToOrbitPos");
        }
    }

    private IEnumerator MoveCameraToOrbitPos()
    {
        //var camDist = Vector3.Distance(camTransform.position, orbitPos);
        var targetPos = orbitPos - camTransform.forward * orbitResetDistance;

        while (Vector3.Distance(camTransform.position, targetPos) > 0.01f)
        {
            camTransform.position = Vector3.Lerp(camTransform.position, targetPos, 0.2f);
            yield return null;
        }

        cameraFocusMove = null;
    }

    #region Event Handlers

    private void SubscribeEvents()
    {
        InputManager.Instance.OnLeftClick += ctx => leftMouseDown = ctx;
        InputManager.Instance.OnRightClick += ctx => rightMouseDown = ctx;
        InputManager.Instance.OnMiddleClick += ctx => middleMouseDown = ctx;
        InputManager.Instance.OnLook += ctx => mouseDelta = ctx;
        InputManager.Instance.OnScroll += ctx => mouseScroll = ctx.y;
        InputManager.Instance.OnEditorFocus += ctx => focus = ctx;
        InputManager.Instance.OnMove += ctx => moveVector = ctx;
        InputManager.Instance.OnShift += ctx => shift = ctx;
    }

    private void UnsubscribeEvents()
    {
        InputManager.Instance.OnLeftClick -= ctx => leftMouseDown = ctx;
        InputManager.Instance.OnRightClick -= ctx => rightMouseDown = ctx;
        InputManager.Instance.OnMiddleClick -= ctx => middleMouseDown = ctx;
        InputManager.Instance.OnLook -= ctx => mouseDelta = ctx;
        InputManager.Instance.OnScroll -= ctx => mouseScroll = ctx.y;
        InputManager.Instance.OnEditorFocus -= ctx => focus = ctx;
        InputManager.Instance.OnMove -= ctx => moveVector = ctx;
        InputManager.Instance.OnShift -= ctx => shift = ctx;
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

    #endregion Event Handlers
}