using UnityEngine.UIElements;

namespace Game.Forms.UI.Shop
{
    internal class ShopItem : ResizableUI
    {
        public ShopItem(VisualElement _root)
        {
            this._root = _root;

            //SetRoot();
            OnPostVisualCreation();
        }

        public void SetRoot(GenericScriptable _item)
        {
            var _spriteElement = _root.Q("Sprite");
            _spriteElement.style.backgroundImage = new StyleBackground(_item.Sprite);

            var _nameElement = _root.Q<Label>("Name");
            _nameElement.text = _item.Name;

            var _costElement = _root.Q<Label>("Cost");
            _costElement.text = "$" + _item.Cost.ToString();

            var _purchaseButton = Root.Q<Button>();
            _purchaseButton.clicked += (_item as IShopItem).OnButtonPress;
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
                _label.style.fontSize = scale * 0.35f;

            base.AutoSize(obj);
        }
    }
}
