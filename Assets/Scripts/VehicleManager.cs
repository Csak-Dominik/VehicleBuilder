using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class VehicleManager : MonoBehaviour
{
    public class BodyNotEmptyException : Exception
    { }

    [field: SerializeField]
    public bool BuildMode { get; private set; } = true;

    // Relations
    [field: SerializeField]
    public List<BodyManager> Bodies { get; private set; } = new List<BodyManager>();

    public void AddBody(int id)
    {
        // check if id is already taken
        foreach (var b in Bodies)
        {
            if (b.Id == id) throw new ArgumentException("Id already taken");
        }

        var body = new GameObject("body_" + id);
        body.transform.parent = transform;
        body.transform.localPosition = Vector3.zero;
        body.transform.localRotation = Quaternion.identity;

        var bodyManager = body.AddComponent<BodyManager>();
        bodyManager.SetIdAndVehicle(id, this);

        Bodies.Add(bodyManager);
    }

    public void RemoveBody(BodyManager body)
    {
        // check if body is empty
        if (body.Parts.Count != 0) throw new BodyNotEmptyException();

        Bodies.Remove(body);
    }

    public void MergeBodies(BodyManager bodyA, BodyManager bodyB)
    {
        for (int i = bodyB.Parts.Count - 1; i >= 0; i--)
        {
            bodyA.AddPart(bodyB.Parts[i]);
            bodyB.RemovePart(bodyB.Parts[i]);
        }

        RemoveBody(bodyB);
    }
}