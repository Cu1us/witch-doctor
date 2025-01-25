using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BaseInteractable : MonoBehaviour, IInteractable
{
    public UnityEvent OnInteract;
    [HideInInspector][SerializeField] new public Renderer renderer;

    public bool interactable = true;

    public Material hoverMaterial;
    Material baseMaterial;

    public virtual void Interact()
    {
        if (!interactable) return;
        OnInteract?.Invoke();
        GameManager.PutThingInCauldron(transform);
    }

    public virtual void OnHover()
    {
        if (!interactable) return;

    }

    public virtual void OnHoverStart()
    {
        if (!interactable) return;
        renderer.sharedMaterials = new Material[] { baseMaterial, hoverMaterial };
    }

    public virtual void OnHoverEnd()
    {
        renderer.sharedMaterials = new Material[] { baseMaterial };
    }

    [ContextMenu("Reassign private component references")]
    protected virtual void Reset()
    {
        renderer = GetComponent<Renderer>();
    }

    protected virtual void Awake()
    {
        baseMaterial = renderer.sharedMaterials[0];
    }
}

public interface IInteractable
{
    public void Interact();
    public void OnHover();
    public void OnHoverStart();
    public void OnHoverEnd();
}