using Game.Forms.Tools;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.Forms.UI
{
    internal struct Slot
    {
        public ToolScriptable Tool { get { return _tool; } }
        private ToolScriptable _tool;
        public VisualElement SlotElement { get { return _slot; } }
        private VisualElement _slot; 

        private Hotbar _hotbar;
        private StyleColor _neutralColour; 
        private StyleColor _selectedColour;

        internal Slot(ToolScriptable _tool, VisualTreeAsset _slotTree, 
            VisualElement _parent, Hotbar _hotbar)
        {
            this._tool = _tool; _slot = new VisualElement();
            this._hotbar = _hotbar;

            _slotTree.CloneTree(_slot);
            _slot = _slot.Q("Root");
            _slot.style.position = Position.Absolute;
            _parent.Add(_slot);

            _neutralColour = _slot.style.backgroundColor;
            _selectedColour = _slot.style.backgroundColor;
            _selectedColour.value += Color.blue;
            var _slotSprite = _slot.Q("Sprite");
            _slotSprite.style.backgroundImage = new StyleBackground(Tool.Sprite);

            SlotElement.RegisterCallback<ClickEvent>(OnSelect);
        }

        public void ReplaceTool(ToolScriptable _tool)
        {
            if(_hotbar.ActiveSlot.Equals(this))
                OnDeselect();

            this._tool = _tool;
            SetBackground();
        }

        private void SetBackground()
        {
            _neutralColour = _slot.style.backgroundColor;
            _selectedColour = _slot.style.backgroundColor;
            _selectedColour.value += Color.blue;
            var _slotSprite = _slot.Q("Sprite");
            _slotSprite.style.backgroundImage = new StyleBackground(Tool.Sprite);
        }

        public void OnSelect(ClickEvent evt)
        {
            if (!_hotbar.SetActiveSlot(this))
                return;

            _slot.style.backgroundColor = _selectedColour;
            GameManager.Instance.Player.SetActiveTool(Tool);
        }

        public void OnDeselect()
        {
            _slot.style.backgroundColor = _neutralColour;
            GameManager.Instance.Player.RemoveActiveTool();
        }
    }

    public class Hotbar : ResizableUI
    {
        private List<Slot> _slots; 
        private VisualTreeAsset _slotTree;
        private Slot _activeSlot;

        internal Slot ActiveSlot { get { return _activeSlot; } }

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
            PanelSettings _panel, int _slotAmount)
        {
            this._slotTree = _slotTree;

            var _uiDocument = _hotbar.GetComponent<UIDocument>();
            if (!_uiDocument)
                _uiDocument = _hotbar.AddComponent<UIDocument>();

            _root = new VisualElement(); _slots = new List<Slot>();
            _uiDocument.rootVisualElement.Add(_root);
            _uiDocument.panelSettings = _panel;

            _root.style.justifyContent = Justify.FlexStart;
            _root.style.alignContent = Align.FlexStart;
            _root.style.flexDirection = FlexDirection.Row;
        }

        internal bool SetActiveSlot(Slot _slot)
        {
            if (_slot.Equals(_activeSlot))
                return false;

            if(_activeSlot.Tool)
                _activeSlot.OnDeselect();

            _activeSlot = _slot;
            return true;
        }

        protected override void AutoSize(TimerState obj)
        {
            // Do any measurements, size adjustments you need (NaNs not an issue now)
            float _scale = Screen.width * 0.05f;

            for (int i = 0; i < _slots.Count; i++)
            {
                _slots[i].SlotElement.style.width = _scale;
                _slots[i].SlotElement.style.height = _scale;
                _slots[i].SlotElement.transform.position =
                    new Vector2(_scale * i, 0);
            }

            _root.style.height = _scale;
            _root.style.width = _scale * _slots.Count;
            _root.transform.position = new Vector2(Screen.width / 2
                - (_scale * _slots.Count / 2), Screen.height - _scale);

            base.AutoSize(obj);
        }

        public void AddToSlot(ToolScriptable _tool)
        {
            _slots.Add(new Slot(_tool, _slotTree, _root, this));
            OnPostVisualCreation();
        }

        public void ReplaceTool(ToolScriptable _tool, int _index)
        {
            _slots[_index].ReplaceTool(_tool);
        }
    }
}
