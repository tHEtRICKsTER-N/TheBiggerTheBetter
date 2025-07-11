using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;

[RequireComponent(typeof(CanvasGroup))]
public class AnimatedPanel : MonoBehaviour
{
    [Header("Animation Settings")]
    public bool animateScale = true;
    public bool animateFade = true;

    public float duration = 0.25f;
    public Ease easeType = Ease.Linear;

    [Header("Scale Settings")]
    public Vector3 openScale = Vector3.one;
    public Vector3 closedScale = Vector3.zero;

    [Header("Fade Settings")]
    [Range(0f, 1f)] public float openAlpha = 1f;
    [Range(0f, 1f)] public float closedAlpha = 0f;

    [Header("Optional UI References")]
    public Button closeButton;
    public ScrollRect scrollRect; // <--- assign your Scroll View here
    public RectTransform contentToRebuild; // <--- optional, assign Content object for force rebuild

    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;
    private Tween currentTween;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();
        SetStateInstant(false);

        if (closeButton != null)
        {
            closeButton.onClick.AddListener(HandleCloseButton);
        }
    }

    private void OnEnable()
    {
        ResetScrollPosition(); // Important to prevent auto-scroll issues
        PlayOpenAnimation();
    }

    private void PlayOpenAnimation()
    {
        currentTween?.Kill();

        if (animateScale)
            rectTransform.localScale = closedScale;
        if (animateFade)
            canvasGroup.alpha = closedAlpha;

        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

        Sequence sequence = DOTween.Sequence();

        if (animateScale)
            sequence.Join(rectTransform.DOScale(openScale, duration).SetEase(easeType));

        if (animateFade)
            sequence.Join(canvasGroup.DOFade(openAlpha, duration).SetEase(easeType));

        currentTween = sequence;
    }

    private void HandleCloseButton()
    {
        ClosePanelWithAnimation();
    }

    public void ClosePanelWithAnimation()
    {
        currentTween?.Kill();

        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        Sequence sequence = DOTween.Sequence();

        if (animateScale)
            sequence.Join(rectTransform.DOScale(closedScale, duration).SetEase(easeType));

        if (animateFade)
            sequence.Join(canvasGroup.DOFade(closedAlpha, duration).SetEase(easeType));

        sequence.OnComplete(() =>
        {
            gameObject.SetActive(false);
        });

        currentTween = sequence;
    }

    private void ResetScrollPosition()
    {
        if (scrollRect == null) return;

        if (contentToRebuild != null)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(contentToRebuild);
        }

        StartCoroutine(ResetScrollNextFrame());
    }

    private IEnumerator ResetScrollNextFrame()
    {
        yield return null; // wait one frame for layout to stabilize
        scrollRect.verticalNormalizedPosition = 1f; // scroll to top
    }

    private void SetStateInstant(bool open)
    {
        rectTransform.localScale = open ? openScale : closedScale;
        canvasGroup.alpha = open ? openAlpha : closedAlpha;
        canvasGroup.interactable = open;
        canvasGroup.blocksRaycasts = open;
    }
}
