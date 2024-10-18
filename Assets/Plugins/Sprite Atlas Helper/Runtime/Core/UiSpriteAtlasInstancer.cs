using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace STVR.SAH
{
    [RequireComponent(typeof(Image))]
    public class UiSpriteAtlasInstancer : BaseSpriteAtlasInstancer
    {
        [SerializeField, HideInInspector] protected Image _renderer;
        [SerializeField] protected bool initializedAtStart = true;
        protected Sprite _createdSprites;

        protected Dictionary<string, Sprite> localCached = new Dictionary<string, Sprite>();
        protected string currentSpriteId;

        public string CurrentSpriteId => currentSpriteId;

        public Image Renderer
        {
            get
            {
                return _renderer;
            }
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            _renderer = GetComponent<Image>();
        }

        private void Start()
        {
            if (_renderer == null)
                _renderer = GetComponent<Image>();

            _renderer.sprite = null;
            if (initializedAtStart)
            {
                localCached ??= new Dictionary<string, Sprite>();
                if (!localCached.ContainsKey(AtlasSprites))
                    localCached.Add(AtlasSprites, AtlasHelper.CloneSpriteFromAtlas(AtlasGroup, AtlasSprites));
                _renderer.sprite = localCached[AtlasSprites];
                currentSpriteId = AtlasSprites;
            }
        }

        public virtual void SetImageRuntime(string name)
        {
            if (_renderer == null && !TryGetComponent(out _renderer))
            {
                Debug.LogError("Object does not have an Image component.");
                return;
            }

            //no need to update, since current sprite is exact same from requested.
            if (!string.IsNullOrEmpty(currentSpriteId))
                if (currentSpriteId.Equals(name)) return;

            _renderer.sprite = null;

            Debug.Log($"Is sprites [{name}] available? [{AtlasHelper.IsSpriteAvailable(AtlasGroup, name)}]");

            localCached ??= new Dictionary<string, Sprite>();

            if (!localCached.ContainsKey(name))
                localCached.Add(name, AtlasHelper.CloneSpriteFromAtlas(AtlasGroup, name));

            _renderer.sprite = localCached[name];
            currentSpriteId = name;
        }

        public override void Preview(Texture2D SelectedSpritePreview, Vector4 border, string spriteName)
        {
#if UNITY_EDITOR

            if (_renderer == null)
                _renderer = GetComponent<Image>();

            _createdSprites = Sprite.Create(SelectedSpritePreview, new Rect(0, 0, SelectedSpritePreview.width, SelectedSpritePreview.height), new Vector2(0.5f, 0.5f), PixelPerUnit, 1, SpriteMeshType.FullRect, border);
            _createdSprites.name = spriteName;
            _renderer.sprite = _createdSprites;
#endif
        }
    }

}
