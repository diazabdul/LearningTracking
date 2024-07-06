using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using UnityEngine.UI;
using TMPro;
using NotToday.Inventory.Data;
using NotToday.Inventory.Controller;

namespace NotToday.Inventory.Intaller
{
    public class TransactionInstaller : MonoInstaller
    {
        [Header("Transaction UI")]
        [SerializeField] TextMeshProUGUI titleItemText;
        [SerializeField] Button actionItemButton;
        [SerializeField] TextMeshProUGUI textActionItemButton;
        [SerializeField] Button swipeLeftButton;
        [SerializeField] Button swipeRightButton;
        [Header("Player Wallet UI")]
        [SerializeField] TextMeshProUGUI walletText;
        [Header("Initalize Setting")]
        [SerializeField] ItemList soItemList;
        [SerializeField] int playerInitalWallet;
        [SerializeField] MeshFilter targetMesh;


        public override void InstallBindings()
        {

            var transaction = new TransactionController(soItemList,playerInitalWallet);
            Container.BindInstance(transaction);
            //Container.BindInterfacesTo(transaction.GetType()).FromInstance(transaction);


            var transactionUI = new TransactionUIController(titleItemText, actionItemButton, textActionItemButton, swipeLeftButton, swipeRightButton, walletText, targetMesh, playerInitalWallet, transaction);
            Container.BindInstance(transactionUI);
            Container.BindInterfacesTo(transactionUI.GetType()).FromInstance(transactionUI);


            //Container.BindInterfacesAndSelfTo<BridgeTransactionController>().AsSingle().NonLazy();


        }
    }
}

