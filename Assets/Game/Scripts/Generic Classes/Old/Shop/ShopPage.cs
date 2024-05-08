using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.Forms.UI.Shop
{
    public partial class ShopBehaviourOld
    {
        protected class ShopPage : DynamicElement
        {
            private Label _header;
            private ShopBehaviourOld _shopBehaviour;
            private int _pageIndex;
            private int _pageCount;

            public int PageId
            {
                get { return _pageIndex; }
            }

            public ShopPage(VisualTreeAsset _prefab, int _pageIndex,
                int _pageCount, ShopBehaviourOld _parent) : base(false)
            {
                _prefab.CloneTree(this);
                this._pageIndex = _pageIndex;
                this._pageCount = _pageCount;

                _shopBehaviour = _parent;
                _header = this.Q<Label>("PageHeader");

                style.position = Position.Absolute;
                style.width = Length.Percent(100);
                style.height = Length.Percent(100);

                OnPostVisualCreation();
            }

            protected override void PostLayout(TimerState obj)
            {
                float _scale = resolvedStyle.width / _pageCount;

                base.PostLayout(obj); this.Q<ListView>().visible = true;
                _shopBehaviour.HideAllPages(true);
            }

            public void SetRoot(VisualTreeAsset _xmlShopItem,
                string _pageName, List<GenericScriptable> _items,
                Color _textColour,
                Color _buttonBackgroundColour,
                Color _buttonTextColour
                )
            {
                _items.Reverse();
                _header.text = _pageName;

                var _listView = this.Q<ListView>();
                PopulateListView(_xmlShopItem, _listView, _items,
                    _textColour, _buttonBackgroundColour, _buttonTextColour);
                _listView.style.visibility = StyleKeyword.Null;

                _header.RegisterCallback<ClickEvent>(HeaderClickEvent);
            }

            private void PopulateListView(VisualTreeAsset _xmlShopItem,
                ListView _listView, List<GenericScriptable> _items,
                Color _textColour,
                Color _buttonBackgroundColour,
            Color _buttonTextColour
                )
            {
                switch ((_items[0] as IShopItem).ShopItemType)
                {
                    case ShopItemType.Researchable:
                        _listView.makeItem = () => 
                            new ResearchableShopItem(_xmlShopItem);
                        break;
                    case ShopItemType.Persistent:
                        _listView.makeItem = () => 
                            new PersistentShopItem(_xmlShopItem);
                        break;
                    case ShopItemType.NonPersistent:
                        _listView.makeItem = () => 
                            new ShopItem(_xmlShopItem);
                        break;
                }

                _listView.bindItem = (item, index) =>
                    (item as ShopItem).SetRoot(_items[index], _textColour,
                        _buttonBackgroundColour, _buttonTextColour);
                _listView.itemsSource = _items;
            }

            public void ShowList()
            {
                SetListVisibility(true);
                BringToFront();
            }

            public void HideList()
            {
                SetListVisibility(false);
            }

            private void SetListVisibility(bool _set)
            {
                var _listView = this.Q<ListView>();
                if (_listView.visible != _set)
                    _listView.visible = _set;
            }

            private void HeaderClickEvent(ClickEvent evt)
            {
                _shopBehaviour.HideAllPages();
                ShowList();
            } 
        }
    }
}
