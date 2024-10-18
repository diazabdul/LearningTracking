using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace STVR.SAH
{
    public class SpriteAtlasInstancerAllowNull : SpriteAtlasInstancer
    {
        public override void ChangeSpriteRuntime(string spriteName)
        {
            if (_renderer == null && !TryGetComponent(out _renderer))
            {
                Debug.LogError("Object does not have an SpriteRenderer component.");
                return;
            }


            _renderer.sprite = null;


            Debug.Log($"Is sprites [{spriteName}] available? [{AtlasHelper.IsSpriteAvailable(AtlasGroup, spriteName)}]");

            if (spriteName == null) return;

            localCached ??= new Dictionary<string, Sprite>();

            if (!localCached.ContainsKey(spriteName))
                localCached.Add(spriteName, AtlasHelper.CloneSpriteFromAtlas(AtlasGroup, spriteName, PixelPerUnit));

            _renderer.sprite = localCached[spriteName];
            currentSpriteId = spriteName;
        }
    }
}

