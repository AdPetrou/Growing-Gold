using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.Forms.UI.Shop
{
    public partial class ShopBehaviourOld
    {
        protected class ResearchableShopItem : ShopItem
        {
            private int _investment = 0;

            public ResearchableShopItem(VisualTreeAsset _prefab) : base(_prefab)
            {
            }

            protected override void AddFunction
                (
                Color _buttonBackgroundColour, 
                Color _buttonTextColour
                )
            {
                base.AddFunction
                    (
                    _buttonBackgroundColour, 
                    _buttonTextColour
                    );

                var _parent = this.Q("FunctionContainer");
                _parent.Q<Button>().text = "$";

                var _progressBar = new ProgressBar();
                _parent.Add(_progressBar);

                _progressBar.style.flexGrow = 1;
                _progressBar.style.flexShrink = 1;
                _progressBar.style.flexBasis = Length.Percent(100);

                _progressBar.value = 0;
                _progressBar.title = "0%";
            }

            protected override void OnButtonPress()
            {
                var _player = GameManager.Instance.Player;
                var _itemInterface = (_item as IShopItem);

                if (_investment + (_player.Wallet.GetAmount() / 2) >=
                    _itemInterface.Cost)
                {
                    _player.Wallet.RemoveAmount(_itemInterface.Cost - _investment);
                    _investment = _itemInterface.Cost;
                    _itemInterface.OnShopComplete();
                    this.Q<Button>().SetEnabled(false);
                }
                else
                {
                    var _amount = _player.Wallet.GetAmount() / 2;
                     _player.Wallet.RemoveAmount(_amount);
                    _investment += _amount;
                }

                UpdateProgres();
            }

            private void UpdateProgres()
            {
                int _cost = (_item as IShopItem).Cost;
                var _progressBar = this.Q<ProgressBar>();
                _progressBar.title = Mathf.RoundToInt((_investment / (float)_cost) 
                    * 100).ToString() + "%";
            }
        }
    } 
}
