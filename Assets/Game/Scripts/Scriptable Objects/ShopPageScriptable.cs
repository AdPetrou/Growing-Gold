using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.Forms.UI.Scriptable
{
    [CreateAssetMenu(fileName = "Shop Page", menuName = "Scriptables/Shop/Page")]
    public class ShopPageScriptable : UIScriptable
    {
        [MyBox.Separator]
        [Header("Page Data")]
        [SerializeField] protected List<ShopItemScriptable> _items;

        public List<ShopItemScriptable> Items => _items;

        public override VisualElement CreateUI()
        {
            var _shopPage = new ShopPageElement(_name, _items);
            return _shopPage;
        }
    }
}
