using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BodyManager : MonoBehaviour
{
    [field: SerializeField]
    public int Id { get; private set; }

    private Rigidbody rb;

    // Relations
    [field: SerializeField]
    public List<Part> Parts { get; private set; } = new List<Part>();

    [field: SerializeField]
    public VehicleManager Vehicle { get; private set; }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (Vehicle.BuildMode)
        {
            rb.isKinematic = true;
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }
        else
        {
            rb.isKinematic = false;
            rb.constraints = RigidbodyConstraints.None;
        }
    }

    public void SetIdAndVehicle(int id, VehicleManager vehicle)
    {
        Id = id;
        Vehicle = vehicle;
    }

    public void AddPart(Part part)
    {
        Parts.Add(part);
        part.SetBody(this);
    }

    public void RemovePart(Part part)
    {
        Parts.Remove(part);
    }

    public void RemovePart(int index)
    {
        Parts.RemoveAt(index);
    }
}