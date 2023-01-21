using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

namespace Game.Forms.UI.Shop
{
    internal class ShopPage : ResizableUI
    {
        private Label _header;
        private ListView _listView;
        private int _pageIndex;
        private int _pageCount;

        public int PageId
        {
            get { return _pageIndex; }
        }

        public ShopPage(VisualElement _root, int _pageIndex,
            int _pageCount, ShopBehaviour _parent)
        {
            this._root = _root.Q("Page");
            this._pageIndex = _pageIndex;
            this._pageCount = _pageCount;

            _header = _root.Q<Label>("Label");
            _header.userData = _parent;

            //SetRoot();
            OnPostVisualCreation();
        }

        protected override void AutoSize(TimerState obj)
        {
            _root.style.width = Length.Percent(100);
            _root.style.height = Length.Percent(100);

            float _scale = _root.resolvedStyle.width / _pageCount;
            _header.transform.position = new Vector2(_scale * _pageIndex, 0);
            _header.style.width = Length.Percent((100 / _pageCount) - 1);

            base.AutoSize(obj);
            _listView.visible = true;
            HideList();
        }

        public void SetRoot(VisualTreeAsset _xmlShopItem,
            List<string> _types, List<GenericScriptable> _items)
        {
            _header.text = (_items[0] as IShopItem).TypeName;

            var _pageList = _root.Q<ListView>();
            _pageList.style.width = Length.Percent(100);
            _pageList.style.height = Length.Percent(100);
            PopulateListView(_xmlShopItem, _pageList, _items);
            _listView = _pageList; _listView.style.visibility = StyleKeyword.Null;

            _header.RegisterCallback<ClickEvent>(HeaderClickEvent);
        }

        private void PopulateListView(VisualTreeAsset _xmlShopItem,
            ListView _listView, List<GenericScriptable> _items)
        {
            _listView.makeItem = () =>
            {
                var _shopElement = _xmlShopItem.Instantiate();
                var _item = new ShopItem(_shopElement);
                _shopElement.userData = _item;
                return _shopElement;
            };
            _listView.bindItem = (item, index) =>
            { (item.userData as ShopItem).SetRoot(_items[index]); };
            _listView.itemsSource = _items;
        }

        public void HideList()
        {
            SetListVisibility(false);
        }

        private void SetListVisibility(bool _set)
        {
            if (_listView.visible != _set)
                _listView.visible = _set;               
        }

        private void HeaderClickEvent(ClickEvent evt)
        {
            var _shop = _header.userData as ShopBehaviour;
            _shop.HideAllPages();
            SetListVisibility(true);
            _root.BringToFront();
        }
    }
}
