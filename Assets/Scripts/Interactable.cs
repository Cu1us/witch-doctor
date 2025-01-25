using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BaseInteractable : MonoBehaviour, IInteractable
{
    public UnityEvent OnInteract;
    [HideInInspector][SerializeField] new public Renderer renderer;

    public Material hoverMaterial;
    Material baseMaterial;

    public virtual void Interact()
    {
        OnInteract?.Invoke();
    }

    public virtual void OnHover()
    {

    }

    public virtual void OnHoverStart()
    {
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
        baseMaterial = renderer.sharedMaterial;
    }
}

public interface IInteractable
{
    public void Interact();
    public void OnHover();
    public void OnHoverStart();
    public void OnHoverEnd();
}