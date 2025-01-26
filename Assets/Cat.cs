using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cat : MonoBehaviour
{
    public Renderer headRenderer;
    public Material unblink;
    public Material blink;
    public float timer;

    void Start()
    {
        InvokeRepeating(nameof(Blink), 4, 2f);
        timer = Time.deltaTime;
    }

    void Blink()
    {
        if (Random.value < 0.4f) return;
        Invoke(nameof(Unblink), 0.25f);
        headRenderer.sharedMaterial = blink;
        // Random chance to play cat sound here?
    }
    void Unblink()
    {
        headRenderer.sharedMaterial = unblink;
    }

    void Sound()
    {
        if (timer < 0)
        {
            AudioManager.Play("Cat");
            timer = Random.Range(10, 20);
        }
    }


}
