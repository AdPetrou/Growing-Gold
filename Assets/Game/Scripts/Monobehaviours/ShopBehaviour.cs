using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

namespace Game.Forms.UI.Shop 
{
    public class ShopBehaviour : MonoBehaviour
    {
        [MyBox.Separator][Header("UI Settings")]

        [SerializeField] private bool _editUI = false;

        [MyBox.ReadOnly(nameof(_editUI), true)][SerializeField]
        private VisualTreeAsset _xmlShopMenu;

        [MyBox.ReadOnly(nameof(_editUI), true)][SerializeField]
        private VisualTreeAsset _xmlShopPage;

        [MyBox.ReadOnly(nameof(_editUI), true)][SerializeField] 
        private VisualTreeAsset _xmlShopItem;

        [MyBox.ReadOnly(nameof(_editUI), true)][SerializeField] 
        private PanelSettings _panel;

        [MyBox.Separator][Header("Shop Settings")]
        [Range(1, 100)] [SerializeField] private int _shopWidth;
        [Dropdown(nameof(_relevantTypes))]
        [SerializeField] private List<string> _applicableTypes;

        private List<ShopPage> _shopPages;
        private List<List<GenericScriptable>> _items;
        private VisualElement _root;
       
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
                    if (_item is IShopItem && _applicableTypes.Contains((_item as IShopItem).TypeName))
                        _shopArray.Add(_item as GenericScriptable);

                Array.Sort(_shopArray.ToArray(), delegate (GenericScriptable x, GenericScriptable y)
                { return x.Cost.CompareTo(y.Cost); });

                return _shopArray;
            }
        }

        public void Start()
        {
            _items = new List<List<GenericScriptable>>();
            foreach (var _type in _applicableTypes)
                _items.Add(new List<GenericScriptable>());
            _shopPages = new List<ShopPage>();

            PopulateItems();
            CreateUI();
        }

        private void CreateUI()
        {
            var _uiDocument = gameObject.AddComponent<UIDocument>();
            _uiDocument.panelSettings = _panel;
            _uiDocument.visualTreeAsset = _xmlShopMenu;

            _root = _uiDocument.rootVisualElement.Q("Root");
            _root.style.width = Length.Percent(_shopWidth);

            for (int i = 0; i < _applicableTypes.Count; i++)
            {
                _shopPages.Add(CreatePage(i, _applicableTypes.Count));
                _root.Q("Pages").Add(_shopPages[i].Root);
            }

            var _labelList = _root.Query<Label>().ToList();
            foreach (var _label in _labelList)
                _label.style.fontSize = _shopWidth;

            float _scale = _shopWidth * 3;

            var _exitButton = _root.Q<Button>("Exit");
            _exitButton.style.width = _scale;
            _exitButton.style.height = _scale;
            _exitButton.style.fontSize = _scale * 0.75f;
            _exitButton.clicked += ToggleShop;

            _root.Q<Label>("Header").style.fontSize = _scale;
            _root.transform.position = new Vector2(0, 0);           
        }

        private void PopulateItems()
        {           
            foreach (var _item in _shopItems)
            {              
                int _index = _applicableTypes.IndexOf((_item as IShopItem).TypeName);
                if( _index > -1 )
                    _items[_index].Add(_item);
            }
        }

        private ShopPage CreatePage(int _pageIndex, int _pageCount)
        {
            var _pageElement = _xmlShopPage.Instantiate();
            var _page = new ShopPage(_pageElement, 
                _pageIndex, _pageCount, this);
            _page.SetRoot(_xmlShopItem, _applicableTypes, _items[_pageIndex]);  

            return _page;
        }

        public void HideAllPages()
        {
            foreach (var _page in _shopPages)
                _page.HideList();
        }

        public void ToggleShop(InputAction.CallbackContext _context)
        {
            if (!_context.started)
                return;

            _root.visible = !_root.visible;
            HideAllPages();
        }
        public void ToggleShop()
        {
            _root.visible = !_root.visible;
            HideAllPages();
        }
    } 
}
