using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class WinPotion : MonoBehaviour
{
    public Transform liquidPart;
    public float rotationSpeed;
    [SerializeField] private float Timer = 2;
    private bool start = false;

    void Update()
    {
        liquidPart.transform.rotation = Quaternion.FromToRotation(Vector3.forward, Vector3.up);

        transform.rotation *= Quaternion.Euler(0, Time.deltaTime * rotationSpeed, 0);
    }

    public void SetColor(Color color)
    {

        liquidPart.GetComponent<Renderer>().material.SetColor("_Color", color);
        Invoke(nameof(PlaySound),3f);
        
    }

    public void PlaySound()
    {
        AudioManager.Play("Sparkle");
    }
}
