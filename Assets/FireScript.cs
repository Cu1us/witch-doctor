using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireScript : MonoBehaviour
{
    [SerializeField] float timer = 45;
 

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            AudioManager.Play("Fire");
            timer = Random.Range(20, 45);
        }

    }
}
