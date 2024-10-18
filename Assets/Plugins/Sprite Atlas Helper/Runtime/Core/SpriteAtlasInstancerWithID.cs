using UnityEngine;

namespace STVR.SAH
{
    public interface ISAHInitializer
    {
        public void RunInitialize();
    }

    [RequireComponent(typeof(SpriteRenderer))]
    public class SpriteAtlasInstancerWithID : SpriteAtlasInstancer, ISAHInitializer
    {
        [SerializeField] int instancerId;

        public int InstancerId => instancerId;

        public void RunInitialize()
        {
            if (_renderer == null)
                _renderer = GetComponent<SpriteRenderer>();

            _renderer.sprite = null;
            _renderer.sprite = AtlasHelper.CloneSpriteFromAtlas(AtlasGroup, AtlasSprites, PixelPerUnit);
        }
    }
}
