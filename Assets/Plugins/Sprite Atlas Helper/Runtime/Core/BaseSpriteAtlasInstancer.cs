using UnityEngine;
using UnityEngine.Serialization;

namespace STVR.SAH
{
    public abstract class BaseSpriteAtlasInstancer : MonoBehaviour
    {
        [SerializeField, HideInInspector] protected string AtlasSprites;
        [SerializeField, HideInInspector] protected string AtlasGroup;
        [SerializeField, HideInInspector] protected int AtlasGroupIndex;
        [SerializeField, HideInInspector] protected int SpriteListIndex;
        /// <summary>
        /// Default always 100.
        /// </summary>
        [SerializeField, FormerlySerializedAs("PixelPerUnit")] protected float PixelPerUnit = 100;

        [SerializeField, SpriteAtlasSearch(typeof(string))] protected string SpriteName;

        //protected bool isUseStringAsQueryUsed() => UseStringAsQuery;

        protected SpriteAtlasListHelper AtlasHelper;

        protected virtual void Awake()
        {
            AtlasHelper ??= new SpriteAtlasListHelper();
        }

        protected virtual void OnEnable()
        {
            AtlasHelper ??= new SpriteAtlasListHelper();
        }

        public abstract void Preview(Texture2D SelectedSpritePreview, Vector4 border, string stringValue);
    }
}
