using System;
using UnityEngine;

namespace STVR.SAH
{
    public class SpriteAtlasSearchAttribute : PropertyAttribute
    {
        public Type searchedObjectType;
        public SpriteAtlasSearchAttribute(Type searchedObjectType)
        {
            this.searchedObjectType = searchedObjectType;
        }
    }
}