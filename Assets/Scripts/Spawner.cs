using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject prefab;
    public float spawnCooldown;
    GameObject spawnedObject;

    float spawnTimer = 0;

    void Update()
    {
        if (spawnedObject == null)
        {
            spawnTimer += Time.deltaTime;
            if (spawnTimer > spawnCooldown)
            {
                spawnTimer = 0;
                spawnedObject = Instantiate(prefab, transform.position, transform.rotation, transform);
                GameManager.SpawnParticle(GameManager.Instance.poofParticle, transform.position);
            }
        }
        else
        {
            spawnTimer = 0;
        }
    }
}
