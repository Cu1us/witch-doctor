using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WitchRandomSounds : MonoBehaviour
{

    [SerializeField] float timer = 20;
    [SerializeField] float timer2 = 30;

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        timer2 -= Time.deltaTime;
        if (timer < 0)
        {
            AudioManager.Play("Witch Random");
            timer = Random.Range(20,30);
        }
        
        /*if (timer2 < 0)
        {
            AudioManager.Play("Fire");
            timer = Random.Range(30,40);
        }*/
        

    }

}
