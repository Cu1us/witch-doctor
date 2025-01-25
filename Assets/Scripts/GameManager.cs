using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Transform cauldronCenter;
    public float cauldronInsertJumpHeight;
    public float cauldronInsertJumpDuration;

    public GameObject poofParticle;

    public Cauldron cauldron;
    public Clipboard clipboard;

    public Client[] clients;
    public int currentClientIndex;
    public static Client currentClient => Instance.clients[Instance.currentClientIndex];

    void Awake()
    {
        if (Instance == null) Instance = this;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            NextClient();
        }
    }

    [ContextMenu("Next client")]
    void NextClient()
    {
        currentClientIndex++;
        if (currentClientIndex >= clients.Length) currentClientIndex = 0;
        clipboard.SetClient(currentClient);
    }

    public static void PutThingInCauldron(Transform thing)
    {
        if (thing.TryGetComponent(out BaseInteractable interactable))
        {
            interactable.interactable = false;
        }
        thing.transform.DOJump(Instance.cauldronCenter.position, Instance.cauldronInsertJumpHeight, 1, Instance.cauldronInsertJumpDuration)
        .SetEase(Ease.Linear)
        .OnComplete(() =>
        {
            Destroy(thing.gameObject);
        });
    }

    public static void SpawnParticle(GameObject particle, Vector3 position)
    {
        Destroy(Instantiate(particle, position, Quaternion.identity), 3);
    }
}
