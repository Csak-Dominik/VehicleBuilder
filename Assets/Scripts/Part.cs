using System.Collections.Generic;
using System.Linq;
using System.Xml;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class Part : MonoBehaviour
{
    public string uuid;

    [field: SerializeField]
    public BodyManager Body { get; private set; }

    [field: SerializeField]
    public string Name { get; private set; }

    [field: SerializeField]
    public string Category { get; private set; }

    [field: SerializeField]
    public List<string> Tags { get; private set; } = new List<string>();

    [SerializeField]
    private MeshFilter meshFilter;

    [SerializeField]
    private bool gizmos = false;

    private void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();

        LoadDefinition();
    }

    private void Update()
    {
    }

    private void OnDrawGizmos()
    {
        if (!gizmos) return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, 0.1f);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position + transform.right, 0.1f);
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position + transform.up, 0.1f);
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position + transform.forward, 0.1f);
    }

    private void LoadDefinition()
    {
        string path = "Assets/Parts/Definitions/" + uuid + ".xml";

        // load the xml document
        XmlDocument doc = new XmlDocument();
        doc.Load(path);

        var part_data = doc.SelectSingleNode("part_data");

        // load the name
        Name = part_data.Attributes["name"].Value;

        // load the category
        Category = part_data.Attributes["category"].Value;

        // load the tags
        Tags = part_data.Attributes["tags"].Value.Split(';').ToList();
    }

    public void SetBody(BodyManager body)
    {
        Body = body;
        transform.parent = body.transform;
    }
}