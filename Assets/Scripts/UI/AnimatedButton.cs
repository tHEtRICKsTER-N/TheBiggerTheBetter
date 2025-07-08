using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AnimatedButton : Button
{
    [Header("Animation Settings")]
    public float scaleDownFactor = 0.5f;
    public float animationDuration = 0.07f;
    public Ease easeType = Ease.OutQuad;

    private bool isAnimating = false;
    private Vector3 originalScale;

    protected override void Awake()
    {
        base.Awake();
        originalScale = transform.localScale;
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        if (!interactable || isAnimating)
            return;

        AnimateClick();
    }

    private void AnimateClick()
    {
        isAnimating = true;

        // Scale down to 50%
        transform.DOScale(originalScale * scaleDownFactor, animationDuration)
            .SetEase(easeType)
            .OnComplete(() =>
            {
                // Scale back up to original
                transform.DOScale(originalScale, animationDuration)
                    .SetEase(easeType)
                    .OnComplete(() =>
                    {
                        onClick.Invoke();
                        isAnimating = false;
                    });
            });
    }
}
