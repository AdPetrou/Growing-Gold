using UnityEngine.UIElements;

namespace Game.Forms.UI.Shop
{
    internal class ShopItem : ResizableUI
    {
        GenericScriptable _item;
        public ShopItem(VisualElement _root)
        {
            this._root = _root;

            //SetRoot();
            OnPostVisualCreation();
        }

        public void SetRoot(GenericScriptable _item)
        {
            this._item = _item;

            var _spriteElement = _root.Q("Sprite");
            _spriteElement.style.backgroundImage = new StyleBackground(_item.Sprite);

            var _nameElement = _root.Q<Label>("Name");
            _nameElement.text = _item.Name;

            var _descriptionElement = _nameElement.Q<Label>("Description");
            _descriptionElement.text = (_item as IShopItem).Description;

            var _costElement = _root.Q<Label>("Cost");
            if (_item.Cost == 0)
                _costElement.text = "$FREE";
            else
                _costElement.text = "$" + _item.Cost.ToString();

            var _purchaseButton = Root.Q<Button>();
            _purchaseButton.clicked += OnButtonPress;
        }

        private void OnButtonPress()
        {
            var _player = GameManager.Instance.Player;

            if (_item.Cost > 0 && ( _player.Wallet == null 
                || _player.Wallet.RemoveAmount(_item.Cost) == -1))
            {
                // Add error box in game later
                return;
            }

            (_item as IShopItem).OnButtonPress();
        }

        protected override void AutoSize(TimerState obj)
        {
            float scale = _root.resolvedStyle.width * 0.2f;
            _root.style.height = scale;
            var _spriteElement = _root.Q("Background");
            _spriteElement.style.width = scale - 20;
            _spriteElement.style.height = scale - 20;

            var _labelList = _root.Query<Label>().ToList();
            foreach (var _label in _labelList)
            {
                if(_label.name != "Description")
                    _label.style.fontSize = scale * 0.35f;
                else
                    _label.style.fontSize = scale * 0.15f;
            }

            base.AutoSize(obj);
        }
    }
}
