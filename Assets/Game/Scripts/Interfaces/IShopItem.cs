using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Game.Forms
{
    public interface IShopItem
    {
        public ShopItemType ShopItemType
        {
            get;
        }

        public string TypeName
        {
            get
            {
                string _typeName = GetType().ToString();
                Regex r = new Regex(@"(?!^)(?=[A-Z])");
                if (_typeName.Contains('.'))
                {
                    int _textIndex = _typeName.LastIndexOf('.') + 1;

                    string _text = _typeName.Substring(_textIndex,
                        _typeName.Length - _textIndex);
                    _text = r.Replace(_text, " ");
                    return _text;
                }
                return _typeName;
            }
        }
        public void OnButtonPress();
    }
}
