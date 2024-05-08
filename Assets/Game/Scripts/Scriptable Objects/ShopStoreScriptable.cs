using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.Forms.UI.Scriptable 
{
    [CreateAssetMenu(fileName = "Shop Store", menuName = "Scriptables/Shop/Store")]
    public class ShopStoreScriptable : UIScriptable
    {
        [MyBox.Separator]
        [Header("Shop Data")]
        [SerializeField] protected List<ShopPageScriptable> _pages;

        public override VisualElement CreateUI()
        {
            var _reference = base.CreateUI();
            var _element = new ShopElement(_reference.styleSheets, _reference.ElementAt(0));
            _element.Q<Label>("Shop-Title").text = _name;

            foreach (var _page in _pages) 
            { _element.AddPage(_page.CreateUI() as ShopPageElement); }
            return _element;
        }
    }
}
