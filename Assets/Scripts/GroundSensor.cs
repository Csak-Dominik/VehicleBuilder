using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundSensor : MonoBehaviour
{
    [SerializeField]
    private PlayerController playerController;

    private void OnTriggerEnter(Collider other)
    {
        playerController.IsGrounded = true;
    }

    private void OnTriggerExit(Collider other)
    {
        playerController.IsGrounded = false;
    }
}