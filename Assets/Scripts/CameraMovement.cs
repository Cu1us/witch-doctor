using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraMovement : MonoBehaviour
{
    [SerializeField] Camera cam;

    Quaternion startRotation;

    void Awake()
    {
        startRotation = cam.transform.rotation;

        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {

    }


    [ContextMenu("Reassign private component references")]
    void Reset()
    {
        cam = GetComponent<Camera>();
    }
}
