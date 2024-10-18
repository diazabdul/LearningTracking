using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace STVR.SAH
{
    [System.Serializable]
    public struct ButtonAnimation
    {
        public string SpriteName;
        public Color Color;
        public bool ColorTransition;
    }

    [RequireComponent(typeof(EventTrigger))]
    [RequireComponent(typeof(UiSpriteAtlasInstancer))]
    public class UiSpriteAtlasButtonAnimator : MonoBehaviour
    {
        public ButtonAnimation OnDefault;
        public ButtonAnimation OnHover;
        public ButtonAnimation OnClick;

        EventTrigger evTrigger;
        UiSpriteAtlasInstancer uiSpriteAtlas;

        private void Awake()
        {
            evTrigger = GetComponent<EventTrigger>();
            uiSpriteAtlas = GetComponent<UiSpriteAtlasInstancer>();

            EventTrigger.Entry onPointerExitEntry = new EventTrigger.Entry();
            onPointerExitEntry.eventID = EventTriggerType.PointerExit;
            onPointerExitEntry.callback.AddListener((data) =>
            {
                PlayAnimation(OnDefault);
            });

            EventTrigger.Entry onPointerUpEntry = new EventTrigger.Entry();
            onPointerUpEntry.eventID = EventTriggerType.PointerUp;
            onPointerUpEntry.callback.AddListener((data) =>
            {
                PlayAnimation(OnDefault);
            });

            EventTrigger.Entry onPointerEnterEntry = new EventTrigger.Entry();
            onPointerEnterEntry.eventID = EventTriggerType.PointerEnter;
            onPointerEnterEntry.callback.AddListener((data) =>
            {
                PlayAnimation(OnHover);
            });

            EventTrigger.Entry onPointerDownEntry = new EventTrigger.Entry();
            onPointerDownEntry.eventID = EventTriggerType.PointerDown;
            onPointerDownEntry.callback.AddListener((data) =>
            {
                PlayAnimation(OnClick);
            });

            evTrigger.triggers.Add(onPointerDownEntry);
            evTrigger.triggers.Add(onPointerUpEntry);
            evTrigger.triggers.Add(onPointerEnterEntry);
            evTrigger.triggers.Add(onPointerExitEntry);
        }

        private void PlayAnimation(ButtonAnimation anim)
        {
            uiSpriteAtlas.SetImageRuntime(anim.SpriteName);
            if (anim.ColorTransition)
                uiSpriteAtlas.Renderer.DOColor(anim.Color, 0.3f);
            else
                uiSpriteAtlas.Renderer.color = anim.Color;
        }
    }

}
