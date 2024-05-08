using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.Forms.UI
{
    public class ShopElement : VisualElement
    {
        private List<ShopPageElement> _shopHeaders;

        public ShopElement(VisualElementStyleSheetSet _styleSheets, 
            VisualElement _transfer = null)
        {
            _shopHeaders = new List<ShopPageElement>();
            if (_transfer != null)
            {
                name = _transfer.name;

                for(int i = 0; i < _styleSheets.count; i++)
                    styleSheets.Add(_styleSheets[i]);

               var _classes = _transfer.GetClasses();
                foreach (var _class in _classes)
                    AddToClassList(_class);

                while (_transfer.childCount > 0)
                    Add(_transfer.ElementAt(0));
            }
        }

        public void AddPage(ShopPageElement _header)
        {
            _shopHeaders.Add(_header);
            this.Q("Pages").Add(_header);
            Insert(0, _header.ItemContainer);
            _header.RegisterCallback<ClickEvent>(PageHeaderClickEvent);
        }

        private void PageHeaderClickEvent(ClickEvent evt)
        {
            HidePages();
            (evt.target as ShopPageElement).ShowPage();
        }
        public void HidePages()
        {
            foreach (ShopPageElement _header in _shopHeaders)
                _header.HidePage();
        }
    }
}