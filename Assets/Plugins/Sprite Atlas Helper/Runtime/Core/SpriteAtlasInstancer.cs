using System.Collections.Generic;
using UnityEngine;

namespace STVR.SAH
{

    [RequireComponent(typeof(SpriteRenderer))]
    public class SpriteAtlasInstancer : BaseSpriteAtlasInstancer
    {
        [SerializeField, HideInInspector] protected SpriteRenderer _renderer;
        [SerializeField] protected bool InitializedAtStart = true;
        protected Sprite createdSprite;
        protected float ppu;

        protected Dictionary<string, Sprite> localCached = new Dictionary<string, Sprite>();
        protected string currentSpriteId;

        public string CurrentSpriteId => currentSpriteId;

        public SpriteRenderer Renderers
        {
            get
            {
                return _renderer;
            }
        }


        protected override void OnEnable()
        {
            base.OnEnable();
            _renderer = GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            if (_renderer == null)
                _renderer = GetComponent<SpriteRenderer>();

            _renderer.sprite = null;
            if (InitializedAtStart)
            {
                localCached ??= new Dictionary<string, Sprite>();
                if (!localCached.ContainsKey(AtlasSprites))
                    localCached.Add(AtlasSprites, AtlasHelper.CloneSpriteFromAtlas(AtlasGroup, AtlasSprites, PixelPerUnit));
                _renderer.sprite = localCached[AtlasSprites];
                currentSpriteId = AtlasSprites;
            }
        }

        public override void Preview(Texture2D SelectedSpritePreview, Vector4 border, string spriteName)
        {
#if UNITY_EDITOR
            if (_renderer == null)
                _renderer = GetComponent<SpriteRenderer>();


            createdSprite = Sprite.Create(SelectedSpritePreview, new Rect(0, 0, SelectedSpritePreview.width, SelectedSpritePreview.height), new Vector2(0.5f, 0.5f), PixelPerUnit);
            createdSprite.name = spriteName;
            _renderer.sprite = createdSprite;
#endif
        }

        public virtual void ChangeSpriteRuntime(string spriteName)
        {
            //_renderer.sprite = null;
            //_renderer.sprite = AtlasHelper.CloneSpriteFromAtlas(AtlasGroup, spriteName, PixelPerUnit);
            if (_renderer == null && !TryGetComponent(out _renderer))
            {
                Debug.LogError("Object does not have an SpriteRenderer component.");
                return;
            }

            //no need to update, since current sprite is exact same from requested.
            if (!string.IsNullOrEmpty(currentSpriteId))
                if (currentSpriteId.Equals(spriteName)) return;

            _renderer.sprite = null;


            Debug.Log($"Is sprites [{spriteName}] available? [{AtlasHelper.IsSpriteAvailable(AtlasGroup, spriteName)}]");

            if (spriteName == null) return;
            
            localCached ??= new Dictionary<string, Sprite>();

            if (!localCached.ContainsKey(spriteName))
                localCached.Add(spriteName, AtlasHelper.CloneSpriteFromAtlas(AtlasGroup, spriteName, PixelPerUnit));
                //Error karena parameter 'name' itu berasal dari class object
                //localCached.Add(name, AtlasHelper.CloneSpriteFromAtlas(AtlasGroup, spriteName, PixelPerUnit));

            _renderer.sprite = localCached[spriteName];
            currentSpriteId = spriteName;
        }
    }
}
