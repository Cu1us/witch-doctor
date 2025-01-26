using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraMovement : MonoBehaviour
{
    [HideInInspector][SerializeField] Camera cam;

    Quaternion startRotation;

    [SerializeField] Vector3 forwardDir;
    [SerializeField] Vector3 maxEuler;
    [SerializeField] Vector3 minEuler;

    void Awake()
    {
        startRotation = cam.transform.rotation;
        forwardDir = forwardDir.normalized;

        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float dx = Input.GetAxisRaw("Mouse X");
        float dy = Input.GetAxisRaw("Mouse Y");

        Quaternion prevRotation = cam.transform.rotation;

        cam.transform.rotation *= Quaternion.AngleAxis(-dy * 2.5f, Vector3.right);

        cam.transform.eulerAngles += Vector3.up * dx * 5;

        Vector3 rotation = cam.transform.eulerAngles;
        rotation.y = (rotation.y + 180) % 360;
        rotation.x = ClampAngle(rotation.x, minEuler.x, maxEuler.x);
        rotation.y = ClampAngle(rotation.y, minEuler.y, maxEuler.y);
        rotation.z = 0;
        rotation.y = (rotation.y - 180) % 360;

        cam.transform.eulerAngles = rotation;
    }


    [ContextMenu("Reassign private component references")]
    void Reset()
    {
        cam = GetComponent<Camera>();
    }

    float ClampAngle(float angle, float from, float to)
    {
        // accepts e.g. -80, 80
        if (angle < 0f) angle = 360 + angle;
        if (angle > 180f) return Mathf.Max(angle, 360 + from);
        return Mathf.Min(angle, to);
    }
}
