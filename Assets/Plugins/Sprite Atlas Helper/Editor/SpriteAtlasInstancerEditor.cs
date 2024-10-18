using System;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

namespace STVR.SAH.Editors
{
    public class SpriteAtlasWindow
    {
        [MenuItem("STVR/Sprite Atlas Instancer/Begin Sprite Atlas Instancer Setup (Default)")]
        static void DefaultInitializationSetup()
        {
            SpriteAtlasWindowCore.CreateFolder();
        }

        [MenuItem("STVR/Sprite Atlas Instancer/Begin Sprite Atlas Instancer Setup (Atlas folder inside Plugin)")]
        static void InitializationSetup()
        {
            SpriteAtlasWindowCore.CreateFolderResourcesInPlugin();
        }
    }

    public class SpriteAtlasWindowCore
    {
        public const string DEF_DIR = "Plugins/Sprite Atlas Helper";
        public const string RES_DIR = "Resources/Atlas";
        public const string RES_FOLDER = "Resources";
        public const string ATLAS_FOLDER = "Atlas";
        public const string DEF_SPLASH_SCREEN_NAME = "Splashscreen";

        public static void CreateFolder()
        {
            if (!AssetDatabase.IsValidFolder($"Assets/{RES_DIR}"))
                Directory.CreateDirectory($"Assets/{RES_DIR}");

            AssetDatabase.Refresh();

            CreateInitializationInstances($"Assets/{RES_DIR}");
        }

        public static void CreateFolderResourcesInPlugin()
        {
            if (!AssetDatabase.IsValidFolder($"Assets/{DEF_DIR}/{RES_DIR}"))
                Directory.CreateDirectory($"Assets/{DEF_DIR}/{RES_DIR}");

            AssetDatabase.Refresh();

            CreateInitializationInstances($"Assets/{DEF_DIR}/{RES_DIR}");
        }

        private static void CreateInitializationInstances(string dir)
        {
            SpriteAtlasList instance = ScriptableObject.CreateInstance<SpriteAtlasList>();

            AssetDatabase.CreateAsset(instance, $"{dir}/Atlas List.asset");
            AssetDatabase.Refresh();
            Selection.activeObject = instance;
            EditorGUIUtility.PingObject(instance);
            Debug.LogFormat("Don't forget to fill Sprite Atlas and click [Initialize Sprite Atlas]~");
        }
    }

    [CustomEditor(typeof(SpriteAtlasInstancer)), CanEditMultipleObjects]
    public class SpriteAtlasInstancerEditor : Editor
    {

        SpriteAtlasListHelper _atlasHelper;
        protected BaseSpriteAtlasInstancer AtlasInstancer;
        SerializedProperty _atlasGroupIndex;
        SerializedProperty _spriteListIndex;
        SerializedProperty _atlasGroup;
        SerializedProperty _atlasSprite;
        SerializedProperty _spriteNameString;

        string[] _atlasSpriteList;
        string[] _atlasGroupList;

        Texture2D _previewSpriteTemp;

        protected virtual void OnEnable()
        {
            if (_atlasHelper == null)
                _atlasHelper = new SpriteAtlasListHelper();

            AtlasInstancer = target as SpriteAtlasInstancer;
            _spriteListIndex = serializedObject.FindProperty("SpriteListIndex");
            _atlasGroupIndex = serializedObject.FindProperty("AtlasGroupIndex");
            _atlasGroup = serializedObject.FindProperty("AtlasGroup");
            _atlasSprite = serializedObject.FindProperty("AtlasSprites");
            _spriteNameString = serializedObject.FindProperty("SpriteName");
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            SetAtlasGroup();
            SetAtlasGroupSprites();
            SetPreviewImage();
            AddAssignButton();
        }

        private void AddAssignButton()
        {
            if (GUILayout.Button("Assign to Renderer"))
            {
                if (_atlasHelper != null)
                    if (_atlasSprite != null)
                        if (_atlasGroup != null)
                        {
                            AtlasInstancer.Preview(_previewSpriteTemp,
                                                   _atlasHelper.GetAtlas(_atlasGroup.stringValue).GetSprite(_atlasSprite.stringValue).border,
                                                    _atlasSprite.stringValue);
                        }
            }
        }

        private void SetPreviewImage()
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Preview Image", GUILayout.Width(100f));

            if (_atlasHelper.TryGetPreviewImage(_atlasSprite.stringValue, _atlasGroup.stringValue, out _previewSpriteTemp))
            {
                GUILayout.Box(_previewSpriteTemp, GUILayout.Width(125f), GUILayout.Height(125f));
            }
            else
                GUILayout.Box("Cannot preview sprites. make sure select correct sprite atlas.");
            EditorGUILayout.EndHorizontal();
        }

        private void SetAtlasGroupSprites()
        {
            _atlasHelper.GetAvailableSpriteFromAtlas(_atlasGroupList[_atlasGroupIndex.intValue], out _atlasSpriteList);

            Array.Sort<string>(_atlasSpriteList, (a, b) => string.Compare(a, b));

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Sprites", GUILayout.Width(100f));
            serializedObject.Update();
            //_spriteListIndex.intValue = EditorGUILayout.Popup(_spriteListIndex.intValue, _atlasSpriteList);
            serializedObject.ApplyModifiedProperties();

            if (_spriteListIndex.intValue > _atlasSpriteList.Length)
            {
                serializedObject.Update();
                _spriteListIndex.intValue = 0;
                serializedObject.ApplyModifiedProperties();
            }

            serializedObject.Update();
            //_atlasSprite.stringValue = _atlasSpriteList[_spriteListIndex.intValue];
            EditorGUILayout.LabelField(_atlasSprite.stringValue, GUILayout.Width(300f));
            serializedObject.ApplyModifiedProperties();

            EditorGUILayout.EndHorizontal();
        }

        private void SetAtlasGroup()
        {
            _atlasHelper.GetAllAtlas(out _atlasGroupList);

            Array.Sort<string>(_atlasGroupList, (a, b) => string.Compare(a, b));

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Atlas Group", GUILayout.Width(100f));
            serializedObject.Update();
            //_atlasGroupIndex.intValue = EditorGUILayout.Popup(_atlasGroupIndex.intValue, _atlasGroupList);
            //_atlasGroup.stringValue = _atlasGroupList[_atlasGroupIndex.intValue];
            EditorGUILayout.LabelField(_atlasGroup.stringValue, GUILayout.Width(300f));
            serializedObject.ApplyModifiedProperties();
            EditorGUILayout.EndHorizontal();
        }
    }
}