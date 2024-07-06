using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NotToday.Inventory.Data
{
    [CreateAssetMenu(fileName = "Item List", menuName = "Scriptable Objects/Item List")]
    public class ItemList : ScriptableObject
    {
        public Item[] Items;
    }
    [System.Serializable]
    public class Item
    {
        public string Name;
        public int Price;
        public Mesh Mesh;
        public ItemState ItemState;

        public Item(Item item)
        {
            Name = item.Name;
            Price = item.Price;
            Mesh = item.Mesh;
            ItemState = item.ItemState;
        }
    }
    public enum ItemState
    {
        Available,
        Purchased,
        Equip
    }
}

