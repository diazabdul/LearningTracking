using UnityEditor;

namespace STVR.SAH.Editors
{
    [CustomEditor(typeof(UiSpriteAtlasInstancer)), CanEditMultipleObjects]
    public class UiSpriteAtlasInstancerEditor : SpriteAtlasInstancerEditor
    {
        protected override void OnEnable()
        {
            base.OnEnable();
            AtlasInstancer = target as UiSpriteAtlasInstancer;
        }
    }
}