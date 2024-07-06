using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using UnityEngine.UI;
using TMPro;
using System;

namespace NotToday.Inventory.Controller
{
    public class TransactionUIController : IInitializable
    {
        TextMeshProUGUI titleItemText;
        Button actionItemButton;
        TextMeshProUGUI textActionItemButton;
        Button swipeLeftButton;
        Button swipeRightButton;
        TextMeshProUGUI walletText;
        MeshFilter targetMesh;


        TransactionController transactionController;
        int playerWallet;
        int itemIndex;
        NotToday.Inventory.Data.Item currentItem;
        NotToday.Inventory.Data.Item lastEquipItem;
        public TransactionUIController(TextMeshProUGUI titleItemText,
                                        Button actionItemButton,
                                        TextMeshProUGUI textActionItemButton,
                                        Button swipeLeftButton,
                                        Button swipeRightButton,
                                        TextMeshProUGUI walletText,
                                        MeshFilter mesh,
                                        int initalWallet,
                                        TransactionController controller
                                        )
        {
            this.titleItemText = titleItemText;
            this.actionItemButton = actionItemButton;
            this.textActionItemButton = textActionItemButton;
            this.swipeLeftButton = swipeLeftButton;
            this.swipeRightButton = swipeRightButton;
            this.walletText = walletText;

            this.playerWallet = initalWallet;
            this.targetMesh = mesh;
            this.transactionController = controller;
        }
        public void Initialize()
        {
            swipeLeftButton.onClick.AddListener(SwipeLeftListener);
            swipeRightButton.onClick.AddListener(SwipeRightListener);

            actionItemButton.onClick.AddListener(ActionButtonListener);

            FillItemContent();
            walletText.text = playerWallet.ToString();
        }

        private void ActionButtonListener()
        {
            switch (currentItem.ItemState)
            {
                case Data.ItemState.Available:
                    OnClickAvailable();
                    break;
                case Data.ItemState.Purchased:
                    OnClickPurchased();
                    break;
                case Data.ItemState.Equip:
                    OnClickEquip();
                    break;
                default:
                    break;
            }
        }
        void OnClickAvailable()
        {
            if(playerWallet - currentItem.Price >= 0)
            {
                playerWallet -= currentItem.Price;
                transactionController.ChangeItemState(itemIndex, Data.ItemState.Equip);

                Debug.Log("Transaction Success !!!");
            }
            else
            {
                Debug.Log("You Dont Have Money !!!");
                return;
            }
            walletText.text = playerWallet.ToString();
            OnEquip();
            FillItemContent();
        }
        void OnClickPurchased()
        {
            transactionController.ChangeItemState(itemIndex, Data.ItemState.Equip);
            OnEquip();
            FillItemContent();
        }
        void OnClickEquip()
        {
            transactionController.ChangeItemState(itemIndex, Data.ItemState.Purchased);
            SetActionButtonText();
            targetMesh.mesh = null;
        }

        private void SwipeRightListener()
        {
            if((itemIndex+1) < transactionController.GetCountItemOnDictionary())
            {
                itemIndex++;
            }
            else
            {
                itemIndex = 0;
            }

            FillItemContent();
        }

        private void SwipeLeftListener()
        {
            if((itemIndex-1) >= 0)
            {
                itemIndex--;
            }
            else
            {
                itemIndex = transactionController.GetCountItemOnDictionary() - 1;
            }

            FillItemContent();
        }
        void FillItemContent()
        {
            Debug.Log("Item Content Filled");
            
            currentItem = transactionController.GetItemAtIndex(itemIndex);
            SetActionButtonText();
            targetMesh.mesh = currentItem.Mesh;
            titleItemText.text = currentItem.Name;            
        }
        
        void OnEquip()
        {
            if(lastEquipItem != null)
                lastEquipItem.ItemState = Data.ItemState.Purchased;

            lastEquipItem = currentItem;
        }

        void SetActionButtonText()
        {
            switch (currentItem.ItemState)
            {
                case Data.ItemState.Available:
                    textActionItemButton.text = currentItem.Price.ToString();
                    break;
                case Data.ItemState.Purchased:
                    textActionItemButton.text = "Equip";
                    break;
                case Data.ItemState.Equip:
                    textActionItemButton.text = "Unequip";
                    break;
                default:
                    break;
            }
        }
    }

}

