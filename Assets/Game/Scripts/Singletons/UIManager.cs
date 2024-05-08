using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Game.Forms.UI.Scriptable;
using UnityEngine.UIElements;
using System;

namespace Game.Forms.UI
{
    public class UIManager : Utilities.Singleton<UIManager>
    {
        private UIScriptable[] _elements;

        // Start is called before the first frame update
        void Start()
        {
            _elements = Resources.LoadAll<ShopStoreScriptable>("");
            foreach (ShopStoreScriptable _element in _elements) 
            { _element.CreateObject(GameManager.Instance.Player.Panel, gameObject.transform); }
        }
    } 
}
