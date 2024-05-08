using Game.Forms.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.VFX;

namespace Game.Forms.UI
{
    public class ShopPageElement : LabelAutoFit
    {
        private VisualElement _itemContainer;
        private List<VisualElement> _items;

        public VisualElement ItemContainer { get { return _itemContainer; } }

        public ShopPageElement(string _pageName,
            List<Scriptable.ShopItemScriptable> _itemScriptables)
        {
            this.text = _pageName;
            this.AddToClassList("shop-title");
            this.ratio = 1;

            _itemContainer = new VisualElement();
            _itemContainer.AddToClassList("shop-page");
            _items = new List<VisualElement>();
            for (int i = 0; i < _itemScriptables.Count; i++)
            {
                var _itemUI = _itemScriptables[i].CreateUI().ElementAt(0);
                List<TimeValue> delay = new List<TimeValue> { i * 0.25f };
                _itemUI.style.transitionDelay = new StyleList<TimeValue>(delay);
                _itemContainer.Add(_itemUI);
                _items.Add(_itemUI);
            }
        }

        public void HidePage()
        {
            foreach (var _item in _items) { _item.style.display = DisplayStyle.None; }
            _itemContainer.style.display = DisplayStyle.None;
        }

        public void ShowPage()
        {
            _itemContainer.style.display = StyleKeyword.Null;
            foreach (var _item in _items) { _item.style.display = StyleKeyword.Null; }
        }
    }
}
