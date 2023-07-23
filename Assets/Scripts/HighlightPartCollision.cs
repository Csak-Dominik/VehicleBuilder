using UnityEngine;

public class HighlightPartCollision : MonoBehaviour
{
    [field: SerializeField]
    public bool CanPlacePart { get; private set; } = true;

    private MeshFilter meshFilter;

    private void Awake()
    {
        gameObject.layer = 2; // ignore raycast

        var collider = GetComponent<Collider>();
        collider.isTrigger = true;

        var rb = gameObject.AddComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeAll;

        meshFilter = GetComponent<MeshFilter>();
    }

    private void Start()
    {
        var mesh = meshFilter.mesh;

        Vector3 center = Vector3.zero;
        int count = 0;

        foreach (var vertex in mesh.vertices)
        {
            center += vertex;
            count++;
        }

        center /= count;

        // scale around center
        float scale = 0.9f;

        var vertices = mesh.vertices;
        for (int i = 0; i < mesh.vertices.Length; i++)
        {
            vertices[i] = Vector3.Lerp(center, mesh.vertices[i], scale);
        }

        mesh.vertices = vertices;

        mesh.RecalculateBounds();
        mesh.RecalculateNormals();

        meshFilter.mesh = mesh;

        // remove mesh collider
        Destroy(GetComponent<MeshCollider>());

        // add mesh collider
        var meshCollider = gameObject.AddComponent<MeshCollider>();
        meshCollider.convex = true;
        meshCollider.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("HighlightPartCollision: OnCollisionEnter");
        if (other.GetComponent<Part>() != null)
        {
            CanPlacePart = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //Debug.Log("HighlightPartCollision: OnCollisionExit");
        if (other.GetComponent<Part>() != null)
        {
            CanPlacePart = true;
        }
    }
}