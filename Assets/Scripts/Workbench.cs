using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Outline))]
public class Workbench : MonoBehaviour, Interactable
{
    private Outline outline;

    [SerializeField]
    private string interactText = "Press E to open the workbench";

    public string InteractText => interactText;

    private void Awake()
    {
        outline = GetComponent<Outline>();
    }

    public void InteractE() => LoadEditorScene();

    public void InteractQ() => LoadEditorScene();

    public void InteractF()
    {
    }

    private void LoadEditorScene()
    {
        SceneManager.LoadScene("EditorScene");
    }

    public void MouseHoverEnter()
    {
        outline.enabled = true;
    }

    public void MouseHoverExit()
    {
        outline.enabled = false;
    }
}