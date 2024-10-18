using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace STVR.SAH
{
    public class UiSpriteAtlasInstancerAllowNull : UiSpriteAtlasInstancer
    {
        public override void SetImageRuntime(string name)
        {
            if (_renderer == null && !TryGetComponent(out _renderer))
            {
                Debug.LogError("Object does not have an Image component.");
                return;
            }

            _renderer.sprite = null;

            Debug.Log($"Is sprites [{name}] available? [{AtlasHelper.IsSpriteAvailable(AtlasGroup, name)}]");
            
            if (name == null) return;
            
            localCached ??= new Dictionary<string, Sprite>();

            if (!localCached.ContainsKey(name))
                localCached.Add(name, AtlasHelper.CloneSpriteFromAtlas(AtlasGroup, name));

            _renderer.sprite = localCached[name];
            currentSpriteId = name;
        }
    }
}

