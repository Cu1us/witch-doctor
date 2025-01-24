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
        float dx = Input.GetAxisRaw("Mouse X");
        float dy = Input.GetAxisRaw("Mouse Y");

        cam.transform.rotation *= Quaternion.AngleAxis(-dy * 2.5f, Vector3.right);

        cam.transform.eulerAngles += Vector3.up * dx * 5;
    }


    [ContextMenu("Reassign private component references")]
    void Reset()
    {
        cam = GetComponent<Camera>();
    }
}
