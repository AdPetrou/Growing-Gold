using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.Forms.UI.Scriptable
{
    public abstract class UIScriptable : ScriptableObject
    {
        [MyBox.Separator]
        [Header("Generic Data")]
        [SerializeField] protected string _name;
        [SerializeField] protected VisualTreeAsset _uiTree;

        public string Name => _name;

        public virtual VisualElement CreateUI() { return _uiTree.CloneTree(); }
        public virtual GameObject CreateObject(Vector3 _position, 
            PanelSettings _panel = null, Transform _parent = null)
        {
            var _object = new GameObject(name);
            if(_parent != null)
                _object.transform.parent = _parent;
            _object.transform.position = _position;

            var _uiDocument = _object.AddComponent<UIDocument>();
            _uiDocument.panelSettings = _panel;
            _uiDocument.rootVisualElement.Add(CreateUI());

            return _object;
        }

        public GameObject CreateObject(PanelSettings _panel = null, Transform _parent = null)
        { return CreateObject(Vector3.zero, _panel, _parent); }
    }
}