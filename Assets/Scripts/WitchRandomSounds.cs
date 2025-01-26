using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WitchRandomSounds : MonoBehaviour
{

    [SerializeField] float timer = 20;

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            AudioManager.Play("Witch Random");
            timer = Random.Range(20,30);
        }
    }

}
