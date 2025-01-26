using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraInteractor : MonoBehaviour
{
    [HideInInspector][SerializeField] Camera cam;
    Ray interactRay;

    [SerializeField] float maxInteractDistance;
    [SerializeField] LayerMask layerMask;
    [SerializeField] Animator handAnimator;

    [SerializeField] float unzoomedFOV;
    [SerializeField] float zoomedFOV;
    [SerializeField] float zoomDuration;
    [SerializeField] AnimationCurve zoomCurve;
    float zoomProgress;
    public Camera handCam;

    IInteractable lastHover;

    void Update()
    {
        zoomProgress = Mathf.Clamp01(zoomProgress + Time.deltaTime / zoomDuration * (Input.GetKey(KeyCode.Mouse1) ? 1 : -1));
        float fov = Mathf.Lerp(unzoomedFOV, zoomedFOV, zoomCurve.Evaluate(zoomProgress));
        cam.fieldOfView = fov;
        handCam.fieldOfView = (fov - unzoomedFOV) * 0.5f + unzoomedFOV;

        interactRay = cam.ViewportPointToRay(new Vector2(0.5f, 0.5f));
        Debug.DrawRay(interactRay.origin, interactRay.direction * 100, Color.yellow);
        Physics.Raycast(interactRay, out RaycastHit hitInfo, maxInteractDistance, layerMask);
        if (hitInfo.collider && hitInfo.collider.TryGetComponent(out IInteractable interactable))
        {
            if (lastHover == null)
            {
                lastHover = interactable;
                interactable.OnHoverStart();
            }
            interactable.OnHover();
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                handAnimator.SetTrigger("Interact");
                interactable.Interact();
            }
        }
        else if (lastHover != null)
        {
            lastHover.OnHoverEnd();
            lastHover = null;
        }
        handAnimator.SetBool("Hovering", lastHover != null);
    }

    [ContextMenu("Reassign private component references")]
    void Reset()
    {
        cam = GetComponent<Camera>();
    }
}
