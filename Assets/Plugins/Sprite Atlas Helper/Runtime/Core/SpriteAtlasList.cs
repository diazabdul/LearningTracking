using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
//using Unity.VisualScripting;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Sprites;
#endif

using UnityEngine;
using UnityEngine.U2D;

#if UNITY_EDITOR
using static UnityEditor.MaterialProperty;
#endif

namespace STVR.SAH
{
    [CreateAssetMenu(fileName = "New Sprite Atlas List", menuName = "STVR/Sprite Atlas Helper/Create new Sprite Atlas List")]
    public class SpriteAtlasList : ScriptableObject
    {
        [SerializeField]
        private SpriteAtlas[] _spriteAtlas;
        protected Dictionary<Sprite, Texture2D> _generatedSpriteTexture = new Dictionary<Sprite, Texture2D>();

        public List<GeneratedTextureStruct> GeneratedTexture;
        public SpriteAtlas[] SpriteAtlas => _spriteAtlas;
        public Dictionary<Sprite, Texture2D> GeneratedSpriteTexture => _generatedSpriteTexture;




        public void InitializeSpriteAtlas()
        {
#if UNITY_EDITOR
            GeneratedTexture = new List<GeneratedTextureStruct>();
            if (_spriteAtlas.Length == 0 || _spriteAtlas == null)
            {
                string[] guid = AssetDatabase.FindAssets("t:SpriteAtlas");
                _spriteAtlas = new SpriteAtlas[guid.Length];
                for (int i = 0; i < guid.Length; i++)
                {
                    string path = AssetDatabase.GUIDToAssetPath(guid[i]);
                    _spriteAtlas[i] = AssetDatabase.LoadAssetAtPath<SpriteAtlas>(path);
                }

                Debug.Log($"Sprite Atlas Initialized. found {guid.Length} atlas(es).");
                Debug.Log($"Generated {GetTotalSpritesCount()} Sprite Texture(s).");
            }

            int GetTotalSpritesCount()
            {
                int count = 0;
                if (_spriteAtlas.Length == 0) return count;

                for (int i = 0; i < _spriteAtlas.Length; i++)
                {
                    count += _spriteAtlas[i].spriteCount;
                }

                return count;
            }
#endif
        }

        public void RefreshGeneratedSpriteTexture()
        {
            _generatedSpriteTexture = new Dictionary<Sprite, Texture2D>();
        }

    }

    [System.Serializable]
    public struct GeneratedTextureStruct
    {
        public string TexturePath;
        public string SpriteName;
        public bool IsSplittedSprites;
        public Color[] SavedColor;

        public GeneratedTextureStruct(string textures, string spriteName, bool isSplittedSprites)
        {
            TexturePath = textures;
            SpriteName = spriteName;
            IsSplittedSprites = isSplittedSprites;
            SavedColor = null;
        }

        public GeneratedTextureStruct(string textures, string spriteName, bool isSplittedSprites, Color[] savedColor)
        {
            TexturePath = textures;
            SpriteName = spriteName;
            IsSplittedSprites = isSplittedSprites;
            SavedColor = savedColor;
        }
    }

    public class SpriteAtlasListHelper
    {
        private SpriteAtlasList _atlasList;
        Regex splitExtensionRegex = new Regex("\\/[A-Za-z_\\s\\d0-9]+(\\.png|jpg)$");
        //Regex getFromParanthesesRegex = new Regex("\\([\\s\\S]*?\\)");
        Regex getFromParanthesesRegex = new Regex("\\([Clone]*?\\)", RegexOptions.IgnoreCase);

        public SpriteAtlasList AtlasList => _atlasList;


        public SpriteAtlasListHelper()
        {
#if UNITY_EDITOR
            string[] guid = AssetDatabase.FindAssets("t:SpriteAtlasList");
            for (int i = 0; i < guid.Length; i++)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid[i]);
                _atlasList = AssetDatabase.LoadAssetAtPath<SpriteAtlasList>(path);
            }
#elif !UNITY_EDITOR
            _atlasList = GetFromResourceFolder<SpriteAtlasList>($"Atlas");
#endif
        }

        public void RegisterGeneratedSpriteTextures()
        {
#if UNITY_EDITOR
            _atlasList.RefreshGeneratedSpriteTexture();

            GetAllAtlas(out string[] groupList);
            for (int i = 0; i < groupList.Length; i++)
            {
                GetAvailableSpriteFromAtlas(groupList[i], out string[] spriteListName);
                Sprite[] sprites = new Sprite[spriteListName.Length];

                GetAtlas(groupList[i]).GetSprites(sprites);

                for (int f = 0; f < sprites.Length; f++)
                {
                    var Matches = getFromParanthesesRegex.Match(sprites[f].name);

                    if (getFromParanthesesRegex.IsMatch(sprites[f].name))
                    {
                        var splitted = getFromParanthesesRegex.Split(sprites[f].name);
                        string path = AssetDatabase.GetAssetPath(SpriteUtility.GetSpriteTexture(GetAtlas(groupList[i]).GetSprite(splitted[0]), false).GetInstanceID());

                        _atlasList.GeneratedTexture.Add(new GeneratedTextureStruct(path, splitted[0], IsTexturePathIsExist(path)));
                    }
                }

                SetDuplicateTexturePathToSplitSprites();

                #region [OBSOLETE]
                //for (int j = 0; j < spriteListName.Length; j++)
                //{

                //    string path = AssetDatabase.GetAssetPath(SpriteUtility.GetSpriteTexture(GetAtlas(groupList[i]).GetSprite(spriteListName[j]), false).GetInstanceID());
                //    string resPath = GetResourcePathFromAssetDatabase(path);
                //    Texture2D tex = GetFromResourceFolder<Texture2D>(resPath);
                //    if (!string.IsNullOrEmpty(resPath))
                //        _atlasList.GeneratedTexture.Add(new GeneratedTextureStruct(resPath, spriteListName[j]));

                //    if (_atlasList.GeneratedSpriteTexture.Count == 0)
                //    {
                //        _atlasList.GeneratedSpriteTexture.Add(GetAtlas(groupList[i]).GetSprite(spriteListName[j]), tex);
                //    }
                //    else
                //    {
                //        if (_atlasList.GeneratedSpriteTexture.ContainsKey(GetAtlas(groupList[i]).GetSprite(spriteListName[j])))
                //        {
                //            _atlasList.GeneratedSpriteTexture[GetAtlas(groupList[i]).GetSprite(spriteListName[j])] = tex;
                //        }
                //        else
                //        {
                //            _atlasList.GeneratedSpriteTexture.Add(GetAtlas(groupList[i]).GetSprite(spriteListName[j]), tex);
                //        }
                //    }
                //}
                #endregion
            }
#endif
        }

        private string RemoveMatchesPattern(Regex pattern, string textToCheck)
        {
            if (pattern.IsMatch(textToCheck))
            {
                return pattern.Replace(textToCheck, "");
            }

            return textToCheck;
        }

        private bool IsTexturePathIsExist(string path)
        {
            if (string.IsNullOrEmpty(path)) return false;

            for (int i = 0; i < _atlasList.GeneratedTexture.Count; i++)
            {
                if (_atlasList.GeneratedTexture[i].TexturePath == path)
                    return true;
            }

            return false;
        }

        private void SetDuplicateTexturePathToSplitSprites()
        {
            List<string> duplicatePath = new List<string>();

            for (int i = 0; i < _atlasList.GeneratedTexture.Count; i++)
            {
                if (_atlasList.GeneratedTexture[i].IsSplittedSprites)
                {
                    if (!duplicatePath.Contains(_atlasList.GeneratedTexture[i].TexturePath))
                        duplicatePath.Add(_atlasList.GeneratedTexture[i].TexturePath);
                }
            }

            if (duplicatePath.Count > 0)
            {
                for (int i = 0; i < duplicatePath.Count; i++)
                {
                    for (int j = 0; j < _atlasList.GeneratedTexture.Count; j++)
                    {
                        if (_atlasList.GeneratedTexture[j].TexturePath == duplicatePath[i])
                        {
                            _atlasList.GeneratedTexture[j] = new GeneratedTextureStruct(_atlasList.GeneratedTexture[j].TexturePath,
                                                                                        _atlasList.GeneratedTexture[j].SpriteName,
                                                                                        true);
                        }
                    }
                }
            }
        }

        public void GetAllAtlas(out string[] atlasGroupList)
        {
            atlasGroupList = new string[_atlasList.SpriteAtlas.Length];

            for (int i = 0; i < atlasGroupList.Length; i++)
            {
                atlasGroupList[i] = _atlasList.SpriteAtlas[i].name;
            }
        }

        public string GetResourcePathFromAssetDatabase(string adbPath)
        {
            if (adbPath.Contains("Resources"))
            {
                int end = adbPath.LastIndexOf("Resources") + ("Resources").Length + 1;
                int extension = 0;
                int size = 0;
                if (adbPath.Contains(".png"))
                    extension = (".png").Length;
                else if (adbPath.Contains(".jpg"))
                    extension = (".jpg").Length;

                size = (adbPath.Length - end) - extension;

                string resPath = adbPath.Substring(end, size);

                return resPath;
            }

            return "";
        }

        public T GetFromResourceFolder<T>(string path) where T : Object
        {
            return Resources.LoadAll<T>(path)[0];

            //return Resources.Load<T>(path);
        }


#if UNITY_EDITOR
        public void GetAvailableSpriteFromAtlas(string atlasGroup, out string[] atlasSpriteList)
        {
            SpriteAtlas atlas = GetAtlas(atlasGroup);
            Sprite[] sprites = new Sprite[atlas.spriteCount];
            atlas.GetSprites(sprites);
            atlasSpriteList = new string[sprites.Length];

            for (int i = 0; i < sprites.Length; i++)
            {
                var data = SpriteUtility.GetSpriteTexture(sprites[i], false);
                var removedPattern = RemoveMatchesPattern(getFromParanthesesRegex, sprites[i].name);
                atlasSpriteList[i] = removedPattern;
            }
        }
#endif

        public SpriteAtlas GetAtlas(string atlasGroup)
        {
            for (int i = 0; i < _atlasList.SpriteAtlas.Length; i++)
            {
                if (_atlasList.SpriteAtlas[i].name == atlasGroup)
                    return _atlasList.SpriteAtlas[i];
            }
            return null;
        }

#if UNITY_EDITOR
        public Texture2D GetPreviewImage(string spriteName, string atlasGroup)
        {
            SpriteAtlas atlas = GetAtlas(atlasGroup);

            if (atlas == null)
                return new Texture2D(10, 10);

            if (SpriteUtility.GetSpriteTexture(atlas.GetSprite(spriteName), false) == null)
                return new Texture2D(10, 10);

            var info = _atlasList.GeneratedTexture.Find(x => x.SpriteName == spriteName);

            if (info.IsSplittedSprites)
            {
                return GetValidTextures(spriteName, atlas);
            }

            return atlas.GetSprite(spriteName).texture;
            //return SpriteUtility.GetSpriteTexture(atlas.GetSprite(spriteName), false);
        }

        private Texture2D GetValidTextures(string spriteName, SpriteAtlas atlas)
        {
            var info = _atlasList.GeneratedTexture.Find(x => x.SpriteName == spriteName);

            if (info.IsSplittedSprites)
            {
                var spriteLoaded = atlas.GetSprite(spriteName);
                var textures = new Texture2D((int)spriteLoaded.rect.width, (int)spriteLoaded.rect.height);
                textures.name = spriteName;

                Color[] defaultPixels = Enumerable.Repeat<Color>(new Color(0, 0, 0, 0), (int)spriteLoaded.rect.width * (int)spriteLoaded.rect.height).ToArray();
                var pixels = spriteLoaded.texture.GetPixels((int)spriteLoaded.textureRect.x,
                                                            (int)spriteLoaded.textureRect.y,
                                                            (int)spriteLoaded.textureRect.width,
                                                            (int)spriteLoaded.textureRect.height);
                textures.SetPixels(defaultPixels);
                textures.SetPixels((int)spriteLoaded.textureRectOffset.x,
                                   (int)spriteLoaded.textureRectOffset.y,
                                   (int)spriteLoaded.textureRect.width,
                                   (int)spriteLoaded.textureRect.height,
                                   pixels);
                textures.Apply();


                //Debug.Log($"{textures.width}| img height {textures.height} with random color {textures.GetPixel(210, 201)}");

                SaveSpriteColor(pixels, spriteName);

                return textures;
            }

            return atlas.GetSprite(spriteName).texture;
            //return SpriteUtility.GetSpriteTexture(atlas.GetSprite(spriteName), false);
        }
#endif

        private void SaveSpriteColor(Color[] col, string spriteName)
        {
            if (!string.IsNullOrEmpty(GetGeneratedTexture(spriteName).SpriteName))
            {
                //if (GetGeneratedTexture(spriteName).SavedColor == null)
                //{
                for (int i = 0; i < _atlasList.GeneratedTexture.Count; i++)
                {
                    if (_atlasList.GeneratedTexture[i].SpriteName == spriteName)
                    {
                        _atlasList.GeneratedTexture[i] = new GeneratedTextureStruct(_atlasList.GeneratedTexture[i].TexturePath,
                                                                                    _atlasList.GeneratedTexture[i].SpriteName,
                                                                                    _atlasList.GeneratedTexture[i].IsSplittedSprites,
                                                                                    col);
                    }
                }
                //}
            }
        }

        private GeneratedTextureStruct GetGeneratedTexture(string spriteName)
        {
            if (_atlasList.GeneratedTexture.Count <= 0 || _atlasList.GeneratedTexture == null) return default;

            for (int i = 0; i < _atlasList.GeneratedTexture.Count; i++)
            {
                if (_atlasList.GeneratedTexture[i].SpriteName == spriteName)
                    return _atlasList.GeneratedTexture[i];
            }

            return default;
        }

        public Sprite CloneSpriteFromAtlas(string atlasGroup, string spriteName, float pixelPerUnit = 16f)
        {
            if (_atlasList.GeneratedTexture.Count == 0 || _atlasList.GeneratedTexture == null)
            {
                return GetAtlas(atlasGroup).GetSprite(spriteName);
            }

            #region [Obsolete]
            //var _spriteTex = _atlasList.GeneratedTexture.Find(x => x.SpriteName == spriteName);
            ////Texture2D spriteTex = GetFromResourceFolder<Texture2D>(_spriteTex.TexturePath);

            //SpriteAtlas atlas = GetAtlas(atlasGroup);
            //Texture2D spriteTex = null;
            //var sprites = GetAtlas(atlasGroup).GetSprite(spriteName);

            //spriteTex = new Texture2D((int)sprites.rect.width, (int)sprites.rect.height);

            //var info = GetGeneratedTexture(spriteName);

            //if (spriteTex != null)
            //{
            //    if (info.IsSplittedSprites)
            //    {
            //        Color[] defaultPixels = Enumerable.Repeat<Color>(new Color(0, 0, 0, 0), (int)sprites.rect.width * (int)sprites.rect.height).ToArray();

            //        spriteTex.SetPixels(defaultPixels);
            //        spriteTex.SetPixels((int)sprites.textureRectOffset.x,
            //                       (int)sprites.textureRectOffset.y,
            //                       (int)sprites.textureRect.width,
            //                       (int)sprites.textureRect.height,
            //                       GetGeneratedTexture(spriteName).SavedColor);

            //        spriteTex.Apply();

            //        //Debug.Log(GetGeneratedTexture(spriteName).SavedColor[3]);

            //        //Debug.Log($"{sprites.texture.name} | {spriteTex.width}| img height {spriteTex.height} with random color {sprites.texture.GetPixel(210, 201)}");

            //        spriteTex.filterMode = filter;

            //        return Sprite.Create(spriteTex,
            //                             new Rect(0, 0, spriteTex.width, spriteTex.height),
            //                             new Vector2(0.5f, 0.5f),
            //                             pixelPerUnit,
            //                             1,
            //                             SpriteMeshType.FullRect,
            //                             sprites.border);
            //    }
            //    else
            //    {
            //        sprites.texture.filterMode = filter;

            //        return Sprite.Create(sprites.texture,
            //                             sprites.textureRect,
            //                             new Vector2(0.5f, 0.5f),
            //                             pixelPerUnit,
            //                             1,
            //                             SpriteMeshType.FullRect,
            //                             sprites.border);
            //    }
            //}
            #endregion

            Vector2 pivot = GetAtlas(atlasGroup).GetSprite(spriteName).pivot / GetAtlas(atlasGroup).GetSprite(spriteName).rect.size;
            return Sprite.Create(GetAtlas(atlasGroup).GetSprite(spriteName).texture, GetAtlas(atlasGroup).GetSprite(spriteName).textureRect, pivot, pixelPerUnit);
        }

        public Sprite CloneSpriteFromAtlas(string atlasGroup, string spriteName)
        {
            if (_atlasList.GeneratedTexture.Count == 0 || _atlasList.GeneratedTexture == null)
            {
                return GetAtlas(atlasGroup).GetSprite(spriteName);
            }

            return GetAtlas(atlasGroup).GetSprite(spriteName);
        }

        public bool IsSpriteAvailable(string atlasGroup, string spriteName)
        {
            return GetAtlas(atlasGroup).GetSprite(spriteName) != null;
        }


#if UNITY_EDITOR
        public bool TryGetPreviewImage(string spriteName, string atlasGroup, out Texture2D texture)
        {
            try
            {
                texture = GetPreviewImage(spriteName, atlasGroup);

                if (texture == null)
                    return false;

                return true;
            }
            catch (System.Exception e)
            {

                Debug.Log($"is error with message {e.Message}");

                texture = null;
                return false;
            }

        }
#endif
    }
}
