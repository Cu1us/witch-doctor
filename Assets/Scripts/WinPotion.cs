using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinPotion : MonoBehaviour
{
    public Transform liquidPart;
    public float rotationSpeed;

    void Update()
    {
        liquidPart.transform.rotation = Quaternion.FromToRotation(Vector3.forward, Vector3.up);

        transform.rotation *= Quaternion.Euler(0, Time.time * rotationSpeed * Mathf.Deg2Rad, 0);
    }

    public void SetColor(Color color)
    {
        liquidPart.GetComponent<Renderer>().material.SetColor("_Color", color);
    }
}
