using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.Forms.UI.Shop
{
    public partial class ShopBehaviourOld
    {
        protected class PersistentShopItem : ShopItem
        {
            public PersistentShopItem(VisualTreeAsset _prefab) : base(_prefab) { }
            private class EquipData
            {
                public PersistentShopItem ShopItem { get; private set; }
                public string ItemType { get; private set; }

                public EquipData(PersistentShopItem shopItem, string itemType)
                {
                    ShopItem = shopItem;
                    ItemType = itemType;
                }
            }
            private static List<EquipData> _equippedTools = new List<EquipData>();

            protected override void OnButtonPress()
            {
                IShopItem _shopItem = (_item as IShopItem);
                if (GameManager.Instance.Player.
                           PersistentItems.Contains(_shopItem))
                {
                    _shopItem.OnShopComplete();
                    SetEquipped(new EquipData(this, _shopItem.TypeName));
                }
                else if (WalletCheck(_shopItem.OnShopComplete,
                        _shopItem.Cost))
                {
                    this.Q<Label>("Cost").text = "OWNED";
                    GameManager.Instance.Player.
                        PersistentItems.Add(_shopItem);
                    SetEquipped(new EquipData(this, _shopItem.TypeName));
                }
                return;
            }

            private void SetEquipped(EquipData _newData)
            {
                EquipData _typeMatch = null;
                _equippedTools.ForEach
                    ((_object) =>
                    {
                        if (_object.ItemType == _newData.ItemType)
                        { _typeMatch = _object; return; }
                    });

                if (_typeMatch == null)
                {
                    _newData.ShopItem.EditButton("Equipped", false);
                    _equippedTools.Add(_newData); return;
                }

                _typeMatch.ShopItem.EditButton("Equip", true);
                _newData.ShopItem.EditButton("Equipped", false);
                _equippedTools[_equippedTools.IndexOf(_typeMatch)] = _newData;
            }

            public void EditButton(string _text, bool _active)
            {
                var _button = this.Q<Button>();
                _button.text = _text;
                _button.SetEnabled(_active);
            }
        }
    }
}
