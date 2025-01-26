using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Clipboard : MonoBehaviour
{
    public Renderer paperRenderer;
    public Transform paper;

    Vector3 paperLocalPos;
    Camera mainCamera;

    public Transform clipboardUpPos;
    public Transform clipboardDownPos;
    public Transform clipboardContainer;

    public float flipDuration;
    public Ease flipEase;

    public float unflippedRot;
    public float flippedRot;

    bool flipping = false;
    bool flipped = false;

    void Awake()
    {
        paperLocalPos = paper.localPosition;
        mainCamera = Camera.main;
    }

    void Start()
    {
        paperRenderer.material.mainTexture = GameManager.currentClient.clipboardTexture;
    }

    void Update()
    {
        float lerp = mainCamera.transform.eulerAngles.x;
        if (lerp > 180f) lerp = 0;
        lerp = 1 - Mathf.Clamp01(lerp / 60);
        transform.position = Vector3.Lerp(clipboardDownPos.position, clipboardUpPos.position, lerp);
        transform.rotation = Quaternion.Lerp(clipboardDownPos.rotation, clipboardUpPos.rotation, lerp);

        if (Input.GetKeyDown(KeyCode.Space) && !flipping)
        {

            AudioManager.Play("Clipboard Turn");
            Vector3 flipTargetRot = Vector3.right * (flipped ? unflippedRot : flippedRot);
            flipped = !flipped;
            clipboardContainer.DOLocalRotate(flipTargetRot, flipDuration)
            .SetEase(flipEase)
            .OnComplete(OnFinishFlip);
        }
    }

    void OnFinishFlip()
    {
        flipping = false;
    }

    public void SetClient(Client client)
    {
        paper.DOLocalMoveX(paperLocalPos.x + 1.5f, 0.75f).OnComplete(() =>
        {
            AudioManager.Play("Clipboard Clip");
            paperRenderer.material.mainTexture = client.clipboardTexture;
            AudioManager.Play("Clipboard Paper");
            paper.DOLocalMoveX(paperLocalPos.x, 0.75f);
        });
    }
}
