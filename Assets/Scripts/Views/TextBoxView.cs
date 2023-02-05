using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class TextBoxView : MonoBehaviour
{
    [SerializeField]
    private RectTransform character;
    private Sequence inSequence;
    private Sequence outSequence;

    [SerializeField]
    private RectTransform panel;

    public void Show()
    {
        AnimateImageIn();
        AnimatePanelIn();
    }

    public void Hide()
    {
        AnimateImageOut();
        AnimatePanelOut();
    }

    private void AnimateImageIn()
    {
        inSequence?.Kill();
        outSequence?.Kill();
        character.localPosition = new Vector3(-1500f, -300f, 0f);

        inSequence = DOTween.Sequence()
            .Append(character.DOAnchorPos(new Vector2(-650f, -300f), 1f, false).SetEase(Ease.InSine))
            .Append(character.DOShakeAnchorPos(2, 10, 2, 0).SetDelay(0.5f).SetLoops(int.MaxValue, LoopType.Yoyo));
    }

    private void AnimateImageOut()
    {
        inSequence?.Kill();
        outSequence?.Kill();

        outSequence = DOTween.Sequence()
            .Append(character.DOAnchorPos(new Vector3(-1500f, -300f, 0f), 1f, false).SetEase(Ease.OutSine));
    }

    private void AnimatePanelIn()
    {
        panel.anchoredPosition = new Vector3(2000f, 0f, 0f);
        DOTween.Sequence()
            .Append(panel.DOAnchorPos(new Vector2(0f, 0f), 0.5f, false).SetEase(Ease.InFlash));
    }

    private void AnimatePanelOut()
    {
        DOTween.Sequence()
            .Append(panel.DOAnchorPos(new Vector2(2000f, -2000f), 0.5f, false).SetEase(Ease.InFlash));
    }
}
