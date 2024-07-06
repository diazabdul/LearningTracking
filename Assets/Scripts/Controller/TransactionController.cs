using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NotToday.Inventory.Data;

namespace NotToday.Inventory.Controller
{
    public class TransactionController
    {
        readonly Dictionary<int, Item> itemOnShop = new Dictionary<int, Item>();
        int playerWallet;


        System.Action OnPurchased;
        public TransactionController(ItemList itemList, int initalWallet)
        {
            playerWallet = initalWallet;
            for (int i = 0; i < itemList.Items.Length; i++)
            {
                Item temp = new(itemList.Items[i]);
                itemOnShop.Add(i, temp);
            }
        }
        public void Initialize()
        {
            Debug.Log("Item On Shop Lenght = " + itemOnShop.Count);
        }
        public Item GetItemAtIndex(int i)
        {
            return itemOnShop[i];
        }
        public int GetCountItemOnDictionary()
        {
            return itemOnShop.Count;
        }
        public void ChangeItemState(int index, ItemState state)
        {
            itemOnShop[index].ItemState = state;
        }
    }
}

