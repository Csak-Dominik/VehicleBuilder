using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField]
    private bool interactE = false;

    [SerializeField]
    private bool interactQ = false;

    [SerializeField]
    private bool interactF = false;

    [SerializeField]
    private Interactable interactable = null;

    [SerializeField]
    private Interactable lastInteractable = null;

    private void Update()
    {
        // do a raycast to see if we hit an interactable
        // if we hit a new interactable, call the new interactable's MouseHoverEnter method and call the last interactable's MouseHoverExit method
        // if we didn't hit a new interactable, call the last interactable's MouseHoverExit method
        // if we hit the same interactable, do nothing

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 10f))
        {
            interactable = hit.collider.GetComponent<Interactable>();
            if (interactable != null)
            {
                if (interactable != lastInteractable)
                {
                    interactable.MouseHoverEnter();
                    lastInteractable?.MouseHoverExit();
                }
            }
            else
            {
                lastInteractable?.MouseHoverExit();
            }
        }
        else
        {
            lastInteractable?.MouseHoverExit();
        }

        // draw a debug ray
        Debug.DrawRay(transform.position, transform.forward * 10f, Color.red);

        if (interactE)
        {
            interactE = false;
            InteractE();
        }
        else if (interactQ)
        {
            interactQ = false;
            InteractQ();
        }
        else if (interactF)
        {
            interactF = false;
            InteractF();
        }

        lastInteractable = interactable;
    }

    private void InteractE()
    {
        // do a raycast to see if we hit an interactable
        // if we did, call the interactable's InteractE method
        // else, do nothing

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 10f))
        {
            Debug.Log(hit.collider.name);
            var interactable = hit.collider.GetComponent<Interactable>();
            interactable?.InteractE();
        }
    }

    private void InteractQ()
    {
    }

    private void InteractF()
    {
    }

    #region EventHandlers

    private void SubscribeEvents()
    {
        InputManager.Instance.OnInteractE += ctx => interactE = ctx;
        InputManager.Instance.OnInteractQ += ctx => interactQ = ctx;
        InputManager.Instance.OnInteractF += ctx => interactF = ctx;
    }

    private void UnsubscribeEvents()
    {
        InputManager.Instance.OnInteractE -= ctx => interactE = ctx;
        InputManager.Instance.OnInteractQ -= ctx => interactQ = ctx;
        InputManager.Instance.OnInteractF -= ctx => interactF = ctx;
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