using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Transform cauldronCenter;

    void Awake()
    {
        if (Instance == null) Instance = this;
    }

    static void PutThingInCauldron(Transform thing)
    {
        Vector3 start = thing.transform.position;
    }
}
