using System;
using UnityEngine;
using LitMotion;
using LitMotion.Extensions;
using UnityEngine.UI;

public class InOutElasticYFrigo : MonoBehaviour
{
    [SerializeField] private RectTransform motionRectTransform;
    [SerializeField] private float startPositionY = 0f;
    [SerializeField] private float endPositionY;
    [SerializeField] private bool activeOnEnable;
    [SerializeField] private bool activeOnDisable;
    [SerializeField] private float xOffset;
    
    private bool alreadyUse = false;

    private void OnEnable()
    {
        if (activeOnEnable)
        {
            StartAnimationBounce();
        }
    }

    private void OnDisable()
    {
        if (activeOnDisable && alreadyUse)
        {
            StartAnimationBounce();
            alreadyUse = false;
        }
    }

    private void StartAnimationBounce()
    {
        motionRectTransform.anchoredPosition = new Vector2(xOffset, alreadyUse ? endPositionY : startPositionY);
        
        LMotion.Create(
                motionRectTransform.anchoredPosition, 
                new Vector2(xOffset, alreadyUse ? startPositionY : endPositionY), 
                2f)
            .WithEase(Ease.OutElastic)
            .BindToAnchoredPosition(motionRectTransform);
    }

    public void SwitchBoolean()
    {
        StartAnimationBounce();
        alreadyUse = !alreadyUse;
    }
}