using UnityEngine;
using LitMotion;
using LitMotion.Extensions;

public class InOutElasticY : MonoBehaviour
{
    [SerializeField] private RectTransform motionRectTransform;
    [SerializeField] private float startPositionY = 0f;
    [SerializeField] private float endPositionY;

    private bool alreadyUse = false;

    public void StartAnimationBounce()
    {
        motionRectTransform.anchoredPosition = new Vector2(alreadyUse ? endPositionY : startPositionY, 0f);
        
        LMotion.Create(
                motionRectTransform.anchoredPosition, 
                new Vector2(alreadyUse ? startPositionY : endPositionY, 0), 
                2f)
            .WithEase(Ease.OutElastic)
            .BindToAnchoredPosition(motionRectTransform);
        alreadyUse = !alreadyUse;
    }
}