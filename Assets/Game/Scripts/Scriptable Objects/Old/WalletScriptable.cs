using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.Forms.Wallets
{
    //[CreateAssetMenu(fileName = "Wallet", menuName = "Scriptables/Wallet")]
    public class WalletScriptable : GenericScriptable, IShopItem
    {
        [MyBox.Separator][Header("Wallet Data")]
        [SerializeField] protected VisualTreeAsset _xmlWalletUI;
        [SerializeField] protected PanelSettings _panel;
        [SerializeField] protected int _cost;
        [SerializeField] protected int _maxAmount;
        [SerializeField][TextArea] protected string _description;


        public string Description => "STORES: " + _maxAmount.ToString("N0") 
            + "\n" + _description;
        public int MaxAmount => _maxAmount;
        public ShopItemType ShopItemType => ShopItemType.Persistent;
        public int Cost => _cost;

        public override GameObject CreateObject(Transform _parent, Vector3 _position)
        {
            var _object = new GameObject();
            _object.transform.parent = _parent;
            _object.transform.position = _position;

            var _uiDocument = _object.AddComponent<UIDocument>();
            _uiDocument.panelSettings = _panel;
            _uiDocument.visualTreeAsset = _xmlWalletUI;
           
            _uiDocument.rootVisualElement.Q("Sprite").style.backgroundImage =
                new StyleBackground(_sprite);

            return _object;
        }

        public void OnShopComplete()
        {
            GameManager.Instance.Player.EquipWallet(this);
        }
    }
}
