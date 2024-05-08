using System;
using System.Collections.Generic;
using UnityEngine.UIElements;

namespace Game.Forms.UI.Shop
{
    public partial class ShopBehaviourOld
    {
        protected class ShopItem : ResizableUI
        {
            protected GenericScriptable _item;

            public ShopItem(VisualTreeAsset _prefab) : base()
            {
                _prefab.CloneTree(this);
            }

            public virtual void SetRoot
                (
                GenericScriptable _item,
                UnityEngine.Color _textColour,
                UnityEngine.Color _buttonBackgroundColour,
                UnityEngine.Color _buttonTextColour
                )
            {
                this._item = _item;

                var _spriteElement = this.Q("Sprite");
                _spriteElement.style.backgroundImage = new StyleBackground(_item.Sprite);

                var _nameElement = this.Q<Label>("Name");
                _nameElement.text = _item.Name;
                _nameElement.style.color = new StyleColor(_textColour);

                var _descriptionElement = this.Q<Label>("Description");
                if (_descriptionElement != null)
                {
                    _descriptionElement.text = (_item as IShopItem).Description;
                    _descriptionElement.style.color = new StyleColor(_textColour);
                }

                var _costElement = this.Q<Label>("Cost");
                if ((_item as IShopItem).Cost == 0)
                    _costElement.text = "$FREE";
                else
                    _costElement.text = "$" + (_item as IShopItem).Cost.ToString();
                _costElement.style.color = new StyleColor(_textColour);

                AddFunction(_buttonBackgroundColour,
                    _buttonTextColour);
                
                OnPostVisualCreation();
            }

            protected virtual void AddFunction
                (
                UnityEngine.Color _buttonBackgroundColour,
                UnityEngine.Color _buttonTextColour
                )
            {
                var _parent = this.Q("FunctionContainer");
                var _purchaseButton = new Button();
                _parent.Add(_purchaseButton);

                _purchaseButton.style.flexGrow = 1;
                _purchaseButton.style.flexShrink = 1;
                _purchaseButton.style.flexBasis = Length.Percent(25);

                _purchaseButton.text = "Purchase";
                _purchaseButton.clicked += OnButtonPress;
                _purchaseButton.style.backgroundColor = new StyleColor(_buttonBackgroundColour);
                _purchaseButton.style.color = new StyleColor(_buttonTextColour);
            }

            protected virtual void OnButtonPress()
            {
                IShopItem _shopItem = (_item as IShopItem);
                WalletCheck(_shopItem.OnShopComplete, _shopItem.Cost);
                return;
            }

            protected bool WalletCheck(System.Action _validAction, int _value)
            {
                var _player = GameManager.Instance.Player;

                if (_value > 0 && (_player.Wallet == null
                    || _player.Wallet.RemoveAmount(_value) == -1))
                {
                    // Add error box in game later
                    return false;
                }

                _validAction.Invoke();
                return true;
            }         

            protected override void PostLayout(TimerState obj)
            {
                float scale = resolvedStyle.width * 0.2f;
                style.height = scale;
                var _spriteElement = this.Q("Background");
                _spriteElement.style.width = scale - 20;
                _spriteElement.style.height = scale - 20;

                base.PostLayout(obj);
            }
        }
    }
}
