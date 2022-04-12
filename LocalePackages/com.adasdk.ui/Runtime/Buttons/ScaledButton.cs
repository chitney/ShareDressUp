using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(AnimationTransformScale), typeof(PlayAudio))]
public class ScaledButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private AnimationTransformScale _animationRectTransformScale;
    private PlayAudio _audio;

    protected void OnEnable()
    {
        _animationRectTransformScale = gameObject.GetComponent<AnimationTransformScale>();
        _audio = gameObject.GetComponent<PlayAudio>(); ;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _animationRectTransformScale.ScaleToEnd();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _animationRectTransformScale.ScaleToStart();
        _audio.Play();
    }
}
