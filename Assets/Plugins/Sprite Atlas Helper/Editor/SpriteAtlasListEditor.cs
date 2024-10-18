using UnityEditor;
using UnityEngine;

namespace STVR.SAH.Editors
{
    [CustomEditor(typeof(SpriteAtlasList)), CanEditMultipleObjects]
    public class SpriteAtlasListEditor : Editor
    {
        SpriteAtlasList _atlasList;
        SpriteAtlasListHelper _atlasHelper;

        private void OnEnable()
        {
            if (_atlasHelper == null)
                _atlasHelper = new SpriteAtlasListHelper();

            if (_atlasList == null)
                _atlasList = target as SpriteAtlasList;
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (GUILayout.Button("Initialize Sprite Atlas"))
            {
                _atlasList.InitializeSpriteAtlas();
                _atlasHelper.RegisterGeneratedSpriteTextures();
            }
        }
    }
}