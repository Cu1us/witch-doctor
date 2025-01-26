using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinPotion : MonoBehaviour
{
    Transform liquidPart;

    void Update()
    {
        liquidPart.transform.rotation = Quaternion.FromToRotation(Vector3.forward, Vector3.down);
    }
}
