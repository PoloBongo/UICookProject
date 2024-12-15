using System;
using UnityEngine;
using LitMotion;
using LitMotion.Extensions;

public class InOutBounceControlableUI : MonoBehaviour
{
    [SerializeField] private RectTransform motionRectTransform;
    [SerializeField] private float startPositionX = 0f;
    [SerializeField] private float endPositionX;

    private bool alreadyUsed = false;

    public void StartAnimationBounce()
    {
        motionRectTransform.anchoredPosition = new Vector2(alreadyUsed ? endPositionX : startPositionX, 0);
        
        LMotion.Create(
                motionRectTransform.anchoredPosition, 
                new Vector2(alreadyUsed ? startPositionX : endPositionX, 0), 
                2f)
            .WithEase(Ease.OutElastic)
            .BindToAnchoredPosition(motionRectTransform);
        alreadyUsed = !alreadyUsed;
    }
}