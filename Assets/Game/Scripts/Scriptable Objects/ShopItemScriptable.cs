using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.Forms.UI.Scriptable
{
    [CreateAssetMenu(fileName = "Shop Item", menuName = "Scriptables/Shop/Item")]
    public class ShopItemScriptable : UIScriptable
    {
        [MyBox.Separator]
        [Header("Item Data")]
        [SerializeField] protected string _description;
        [SerializeField] protected float _cost;
        [SerializeField] protected float _costPremium;

        public override VisualElement CreateUI()
        {
            var _element = base.CreateUI();
            _element.Q<Label>("Item-Name").text = _name;
            _element.Q<Label>("Item-Description").text = _description;
            _element.Q<Button>("Purchase").text = _cost.ToString();
            _element.Q<Button>("Purchase-Premium").text = _costPremium.ToString();
            return _element;
        }
    }
}