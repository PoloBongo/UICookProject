using UnityEngine;
using LitMotion;
using LitMotion.Extensions;

public class InOutElasticYRecipeBook : MonoBehaviour
{
    [SerializeField] private RectTransform motionRectTransform;
    [SerializeField] private float startPositionY = 0f;
    [SerializeField] private float endPositionY;

    private bool alreadyUse = false;

    public void StartAnimationBounce()
    {
        motionRectTransform.anchoredPosition = new Vector2(0f, alreadyUse ? endPositionY : startPositionY);
        
        LMotion.Create(
                motionRectTransform.anchoredPosition, 
                new Vector2(0f, alreadyUse ? startPositionY : endPositionY), 
                2f)
            .WithEase(Ease.OutElastic)
            .BindToAnchoredPosition(motionRectTransform);
        alreadyUse = !alreadyUse;
    }
}