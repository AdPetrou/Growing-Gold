using Game.Forms.Tools;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.Forms.UI
{
    public class Hotbar : DynamicElement
    {
        private class Slot : DynamicElement
        {
            public ToolScriptable Tool { get { return _tool; } }
            private ToolScriptable _tool;

            private Hotbar _hotbar;
            private StyleColor _neutralColour;
            private StyleColor _selectedColour;

            internal Slot(ToolScriptable _tool, VisualTreeAsset _slotTree,
                VisualElement _parent, Hotbar _hotbar) : base(false)
            {
                this._tool = _tool;
                this._hotbar = _hotbar;

                _slotTree.CloneTree(this);
                style.position = Position.Absolute;
                _parent.Add(this);

                _neutralColour = new StyleColor();
                _selectedColour = new StyleColor();

                RegisterCallback<ClickEvent>(OnSelect);
                OnPostVisualCreation();
            }

            public void ReplaceTool(ToolScriptable _tool)
            {
                if (_hotbar.ActiveSlot != null
                    && _hotbar.ActiveSlot.Equals(this))
                    OnDeselect();

                this._tool = _tool;
                OnPostVisualCreation();
            }

            protected override void PostLayout(TimerState obj)
            {
                var _base = this.Q("Base");

                _neutralColour = _base.style.backgroundColor;
                _selectedColour = _base.style.backgroundColor;
                _selectedColour.value += Color.blue;

                var _slotSprite = _base.Q("Sprite");
                _slotSprite.style.backgroundImage = new StyleBackground(Tool.Sprite);
                base.PostLayout(obj);
            }

            public void OnSelect(ClickEvent evt)
            {
                if (!_hotbar.SetActiveSlot(this))
                    return;

                this.Q("Base").style.backgroundColor = _selectedColour;
                GameManager.Instance.Player.SetActiveTool(Tool);
            }

            public void OnDeselect()
            {
                this.Q("Base").style.backgroundColor = _neutralColour;
                GameManager.Instance.Player.RemoveActiveTool();
            }
        }

        private List<Slot> _slots; 
        private VisualTreeAsset _slotTree;
        private Slot _activeSlot;

        private Slot ActiveSlot { get { return _activeSlot; } }

        public List<System.Type> ToolTypeList
        {
            get
            {
                var _return = new List<System.Type>();
                foreach (var _slot in _slots)
                    _return.Add(_slot.Tool.GetType());

                return _return;
            }
        }

        public Hotbar(GameObject _hotbar, VisualTreeAsset _slotTree,
            PanelSettings _panel, int _slotAmount) : base(false)
        {
            this._slotTree = _slotTree;

            var _uiDocument = _hotbar.GetComponent<UIDocument>();
            if (!_uiDocument)
                _uiDocument = _hotbar.AddComponent<UIDocument>();

            _slots = new List<Slot>();
            _uiDocument.rootVisualElement.Add(this);
            _uiDocument.panelSettings = _panel;

            style.height = 96 * _panel.scale;
            style.justifyContent = Justify.FlexStart;
            style.alignContent = Align.FlexStart;
            style.flexDirection = FlexDirection.Row;

            Anchor = AnchorType.ANCHOR_BOTTOMCENTER;
        }

        private bool SetActiveSlot(Slot _slot)
        {
            if (_slot.Equals(_activeSlot))
            {
                _activeSlot.OnDeselect();
                _activeSlot = null;
                return false;
            }

            if(_activeSlot != null && _activeSlot.Tool)
                _activeSlot.OnDeselect();

            _activeSlot = _slot;
            return true;
        }

        protected override void PostLayout(TimerState obj)
        {
            // Do any measurements, size adjustments you need (NaNs not an issue now)
            var _width = resolvedStyle.height * _slots.Count;
            style.width = _width;

            for (int i = 0; i < _slots.Count; i++)
            {
                _slots[i].transform.position =
                    new Vector2(resolvedStyle.height * i, 0);
            }           

            base.PostLayout(obj);
        }

        public void AddToSlot(ToolScriptable _tool)
        {
            var _slot = new Slot(_tool, _slotTree, this, this);
            _slot.style.width = resolvedStyle.height;
            _slot.style.height = resolvedStyle.height;

            _slots.Add(_slot);
            OnPostVisualCreation();
        }

        public void ReplaceTool(ToolScriptable _tool, int _index)
        {
            if (_slots[_index].Tool == _tool)
                return;

            _slots[_index].ReplaceTool(_tool);
        }
    }
}
