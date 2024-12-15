using UnityEngine;
using LitMotion;
using LitMotion.Extensions;

public class InOutBounceControlable : MonoBehaviour
{
    [SerializeField] private RectTransform motionRectTransform;
    [SerializeField] private float startPositionY = 0f;
    [SerializeField] private float endPositionY;

    public void StartAnimationBounce()
    {
        motionRectTransform.anchoredPosition = new Vector2(0.32f, startPositionY);
        
        LMotion.Create(
                motionRectTransform.anchoredPosition, 
                new Vector2(0.32f, endPositionY), 
                2f)
            .WithEase(Ease.OutElastic)
            .BindToAnchoredPosition(motionRectTransform);
    }
}