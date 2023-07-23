using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class PartBuilder : MonoBehaviour
{
    [field: SerializeField]
    public GameObject SelectedPartPrefab { get; private set; }

    private GameObject highlightPart;

    [SerializeField]
    private Material placeableMat;

    [SerializeField]
    private Material notPlaceableMat;

    private bool prevCanPlacePart = false;
    private HighlightPartCollision highlightPartCollision;
    private MeshRenderer highlightPartMeshRenderer;

    private bool leftMouse = false;
    private bool prevLeftMouse = false;

    private Part lastHoverPart;

    private void Awake()
    {
        // instantiate the highlight part
        highlightPart = Instantiate(SelectedPartPrefab);

        // disable the highlight part
        highlightPart.SetActive(false);

        // set the highlight part's material to the highlight material
        highlightPartMeshRenderer = highlightPart.GetComponent<MeshRenderer>();
        highlightPartMeshRenderer.material = notPlaceableMat;

        highlightPartCollision = highlightPart.AddComponent<HighlightPartCollision>();
    }

    private void Update()
    {
        // raycast to find the part we're hovering over
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            var collider = hit.collider;
            if (collider != null && collider.GetComponent<Part>() != null)
            {
                //Debug.Log("Hovering over part: " + collider.name);

                var part = collider.GetComponent<Part>();
                lastHoverPart = part;

                // enable the highlight part
                highlightPart.SetActive(true);

                //var side = Helper.GetSideFromDir(hit.normal);
                //var side = Helper.GetSide(hit.collider.transform.position, hit.point);
                var side = Helper.GetSideMixed(hit.collider.transform.position, hit.point, hit.normal);

                // set the highlight part's position to the part's position
                highlightPart.transform.position = Helper.BlockPosOnSide(part.transform.position, side);
            }
            else
            {
                // disable the highlight part
                highlightPart.SetActive(false);
            }
        }
        else
        {
            // disable the highlight part
            highlightPart.SetActive(false);
        }

        // Change the highlight part's material based on whether or not we can place the part
        var canPlacePart = highlightPartCollision.CanPlacePart;

        if (canPlacePart && !prevCanPlacePart)
        {
            highlightPartMeshRenderer.material = placeableMat;
        }
        else if (!canPlacePart && prevCanPlacePart)
        {
            highlightPartMeshRenderer.material = notPlaceableMat;
        }

        // place the part
        if (leftMouse && !prevLeftMouse && canPlacePart && highlightPart.activeSelf)
        {
            // instantiate the part
            var part = Instantiate(SelectedPartPrefab);

            // set the part's position to the highlight part's position
            part.transform.position = highlightPart.transform.position;
            // set the matrix to the highlight part's matrix
            part.transform.rotation = highlightPart.transform.rotation;

            var partComp = part.GetComponent<Part>();
            lastHoverPart.Body.AddPart(partComp);
        }

        prevCanPlacePart = highlightPartCollision.CanPlacePart;
        prevLeftMouse = leftMouse;
    }

    public void SetSelectedPartPrefab(GameObject prefab)
    {
        SelectedPartPrefab = prefab;

        // instantiate the highlight part
        highlightPart = Instantiate(SelectedPartPrefab);

        // disable the highlight part
        highlightPart.SetActive(false);

        // set the highlight part's material to the highlight material
        highlightPartMeshRenderer = highlightPart.GetComponent<MeshRenderer>();
        highlightPartCollision = highlightPart.AddComponent<HighlightPartCollision>();
        highlightPartMeshRenderer.material = notPlaceableMat;
    }

    #region Event Handlers

    private void SubscribeEvents()
    {
        InputManager.Instance.OnLeftClick += ctx => leftMouse = ctx;
    }

    private void UnsubscribeEvents()
    {
        InputManager.Instance.OnLeftClick -= ctx => leftMouse = ctx;
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