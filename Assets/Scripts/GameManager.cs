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

    void Awake()
    {
        if (Instance == null) Instance = this;
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
}
