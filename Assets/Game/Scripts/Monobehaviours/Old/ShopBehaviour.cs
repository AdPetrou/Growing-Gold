using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using MyBox;

namespace Game.Forms.UI.Shop 
{
    public partial class ShopBehaviourOld : MonoBehaviour
    {
        #region Serializable Fields

        #region XML Fields

        [Separator][Header("XML Fields")]

        [Header("")]
        [SerializeField] private bool _editUI = false;

        [ReadOnly(nameof(_editUI), true)][SerializeField]
        private VisualTreeAsset _xmlShopMenu;
        [ReadOnly(nameof(_editUI), true)][SerializeField]
        private VisualTreeAsset _xmlShopPage;
        [ReadOnly(nameof(_editUI), true)][SerializeField] 
        private VisualTreeAsset _xmlShopItem;
        [ReadOnly(nameof(_editUI), true)][SerializeField] 
        private PanelSettings _panel;

        #endregion

        #region Shop Design

        [Separator][Header("Shop Design")]

        [Header("")]
        [SerializeField] private Color _backgroundColour;
        [SerializeField] private Color _highlightColour;
        [SerializeField] private Color _textColour;

        [Header("")]
        [SerializeField] private Color _buttonBackgroundColour;
        [SerializeField] private Color _buttonTextColour;

        [Header("")]
        [SerializeField] private string _shopName;
        [SerializeField] private StretchType _stretchType;

        [MyBox.ConditionalField(nameof(_stretchType), true, 
            StretchType.STRETCH_BOTH, StretchType.STRETCH_WIDTH)]
        [Range(1, 100)] [SerializeField] private int _shopWidth;

        [MyBox.ConditionalField(nameof(_stretchType), true,
            StretchType.STRETCH_BOTH, StretchType.STRETCH_HEIGHT)]
        [Range(1, 100)][SerializeField] private int _shopHeight;

        [SerializeField] private AnchorType _anchor;

        #endregion

        [Separator][Header("Shop Settings")]
        [SerializeField] private List<PageListItem> _applicableTypes;
        [Serializable]
        private class PageListItem
        {
            [Dropdown(nameof(_relevantTypes))]
            public string Type;
            public string PageHeader;
        }

        #endregion

        #region Private Fields
        private List<ShopPage> _shopPages;
        private List<List<GenericScriptable>> _items;
        private DynamicElement _root;
        #endregion
        #region Private Properties
        private List<string> _relevantTypes
        {
            get
            {
                var _items = Resources.LoadAll("");
                var _shopArray = new List<string>();
                foreach (object _item in _items)
                    if (_item is IShopItem && _item is GenericScriptable)
                        _shopArray.Add((_item as IShopItem).TypeName);

                return _shopArray.ToList();
            }
        }
        private List<GenericScriptable> _shopItems
        {
            get
            {
                var _items = Resources.LoadAll("");
                var _shopArray = new List<GenericScriptable>();

                foreach (object _item in _items)
                    if (_item is IShopItem && 
                        CheckPageTypeList((_item as IShopItem).TypeName))
                            _shopArray.Add(_item as GenericScriptable);

                Array.Sort(_shopArray.ToArray(), delegate (GenericScriptable x, GenericScriptable y)
                { return (x as IShopItem).Cost.CompareTo((y as IShopItem).Cost); });

                return _shopArray;
            }
        }
        #endregion


        public void Start()
        {
            _items = new List<List<GenericScriptable>>();
            foreach (var _type in _applicableTypes)
                _items.Add(new List<GenericScriptable>());
            _shopPages = new List<ShopPage>();

            PopulateItems();
            CreateUI();
            //SetColours();
            ToggleShop();
        }

        private bool CheckPageTypeList(string _typeName)
        {
            foreach (var _listItem in _applicableTypes)
                if (_listItem.Type == _typeName)
                    return true;

            return false;
        }
        private int IndexOfPageTypeList(string _typeName)
        {
            for (int i = 0; i < _applicableTypes.Count; i++)
                if (_applicableTypes[i].Type == _typeName)
                    return i;

            return -1;
        }

        private void CreateUI()
        {
            var _uiDocument = gameObject.AddComponent<UIDocument>();
            _uiDocument.panelSettings = _panel;
            _uiDocument.sortingOrder = 10;
            //_uiDocument.visualTreeAsset = _xmlShopMenu;

            _root = new DynamicElement(_shopWidth, _shopHeight, _stretchType);
            _uiDocument.rootVisualElement.Add(_root); _xmlShopMenu.CloneTree(_root);
            _root.Anchor = _anchor;

            for (int i = 0; i < _applicableTypes.Count; i++)
            {
                _shopPages.Add(CreatePage(i, _applicableTypes.Count));    
                _root.Q("PageHeaders").Add(_shopPages[i].Q<Label>("PageHeader"));
                _root.Q("Pages").Add(_shopPages[i]);
            }
            if (_shopPages.Count == 1)
                _root.Remove(_root.Q("PageHeaders"));

            _root.Q<Label>("Header").text = _shopName;
            _root.Q<Button>("Exit").clicked += ToggleShop;
            _root.transform.position = new Vector2(0, 0);
        }

        private void PopulateItems()
        {           
            foreach (var _item in _shopItems)
            {              
                int _index = IndexOfPageTypeList((_item as IShopItem).TypeName);
                if( _index > -1 ) _items[_index].Add(_item);
            }
        }

        private ShopPage CreatePage(int _pageIndex, int _pageCount)
        {
            var _page = new ShopPage(_xmlShopPage, _pageIndex, _pageCount, this);
            _page.SetRoot(
                _xmlShopItem, _applicableTypes[_pageIndex].PageHeader, _items[_pageIndex],
                _textColour, _buttonBackgroundColour, _buttonTextColour);

            return _page;
        }

        public void HideAllPages(bool _enableFirst = false)
        {
            foreach (var _page in _shopPages)
            {
                if (_root.visible && _enableFirst 
                    && _page == _shopPages[0])
                    _page.ShowList();
                else
                    _page.HideList(); 
            }

        }

        private void SetColours()
        {
            _root.Query<Label>().ForEach((_obj) =>
                _obj.style.color = new StyleColor(_textColour));
            _root.Q<Button>("Exit").style.color = new StyleColor(_textColour);

            var _pageHeaders = _root.Q("PageHeaders");
            if(_pageHeaders != null)
                _root.Q("PageHeaders").Query().ForEach((_obj) =>
                    _obj.style.backgroundColor = new StyleColor(_backgroundColour));

            var _background = _root.Q("Background");
            _background.style.backgroundColor = new StyleColor(_backgroundColour);
            _root.BorderColor = _highlightColour; _root.SetChildBorders(_highlightColour);

            _root.MarkDirtyRepaint();
        }

        public void ToggleShop(InputAction.CallbackContext _context)
        {
            if (!_context.started)
                return;

            _root.visible = !_root.visible;
            HideAllPages(true);
        }
        public void ToggleShop()
        {
            _root.visible = !_root.visible;
            HideAllPages(true);
        }
    } 
}
