using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace STVR.SAH.Editors
{
    public class AtlasListSearchProvider : ScriptableObject, ISearchWindowProvider
    {
        SpriteAtlasListHelper _atlasHelper;
        private Action<int, int, string, string> onSelectSpriteCallback;

        public AtlasListSearchProvider(Action<int, int, string, string> onSelectCallback)
        {
            onSelectSpriteCallback = onSelectCallback;
        }

        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            if (_atlasHelper == null)
                _atlasHelper = new SpriteAtlasListHelper();

            List<SearchTreeEntry> sList = new List<SearchTreeEntry>();

            _atlasHelper.GetAllAtlas(out string[] atlasGroupList);

            sList.Add(new SearchTreeGroupEntry(new GUIContent("Atlas List"), 0));

            Array.Sort<string>(atlasGroupList, (a, b) => string.Compare(a, b));

            foreach (var atlasGroup in atlasGroupList)
            {
                sList.Add(new SearchTreeGroupEntry(new GUIContent(atlasGroup), 1));
                _atlasHelper.GetAvailableSpriteFromAtlas(atlasGroup, out string[] spritelist);

                Array.Sort<string>(spritelist, (a, b) => string.Compare(a, b));

                foreach (var sprite in spritelist)
                {
                    SearchTreeEntry entry = new SearchTreeEntry(new GUIContent(sprite, _atlasHelper.GetPreviewImage(sprite, atlasGroup)));
                    entry.level = 2;
                    entry.userData = $"{Array.FindIndex(atlasGroupList, x => x == atlasGroup)}~{Array.FindIndex(spritelist, x => x == sprite)}~{atlasGroup}~{sprite}";
                    sList.Add(entry);
                }
            }

            return sList;
        }

        public bool OnSelectEntry(SearchTreeEntry SearchTreeEntry, SearchWindowContext context)
        {
            var split = SearchTreeEntry.userData.ToString().Split('~');
            onSelectSpriteCallback?.Invoke(int.Parse(split[0]), int.Parse(split[1]), split[2], split[3]);
            return true;
        }
    }

    [CustomPropertyDrawer(typeof(SpriteAtlasSearchAttribute))]
    public class SpriteAtlasSearchAttributesPropertyDrawer : PropertyDrawer
    {
        SerializedProperty _atlasGroupIndex;
        SerializedProperty _spriteListIndex;
        SerializedProperty _atlasGroup;
        SerializedProperty _atlasSprite;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            position.x = 180;
            position.width = 465;
            position.x += position.width - 525;
            position.width = 100;

            _spriteListIndex = property.serializedObject.FindProperty("SpriteListIndex");
            _atlasGroupIndex = property.serializedObject.FindProperty("AtlasGroupIndex");
            _atlasGroup = property.serializedObject.FindProperty("AtlasGroup");
            _atlasSprite = property.serializedObject.FindProperty("AtlasSprites");

            if (GUI.Button(position, new GUIContent("Find Sprites"), EditorStyles.popup))
            {
                SearchWindow.Open(new SearchWindowContext(GUIUtility.GUIToScreenPoint(Event.current.mousePosition)),
                    new AtlasListSearchProvider((int atlasGroupIndex, int spriteIndex, string atlasGroup, string spriteName) =>
                {
                    _atlasGroupIndex.intValue = atlasGroupIndex;
                    _spriteListIndex.intValue = spriteIndex;
                    property.serializedObject.ApplyModifiedProperties();
                    _atlasGroup.stringValue = atlasGroup;
                    _atlasSprite.stringValue = spriteName;
                    property.serializedObject.ApplyModifiedProperties();
                }));
            }
        }
    }
}