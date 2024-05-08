using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.Forms.UI
{
    public class DynamicElement : ResizableUI
    {
        private Length _width;
        private Length _height;

        public DynamicElement(float _width = 0f, 
            float _height = 0f) : base()
        {
            Width = _width; Height = _height;
        }

        public DynamicElement(bool useDefault = true) : base()
        {
            if (!useDefault)
            { Width = 0; Height = 0; return; }

            Width = this.style.width.value.value;
            Height = this.style.height.value.value;
        }

        public DynamicElement(float _width, float _height,
            StretchType _stretch) : base()
        {
            Width = _width; Height = _height;
            Stretch = _stretch;
        }

        public float Width { get => _width.value; set => _width = Length.Percent(value); }
        public float Height { get => _height.value; set => _height = Length.Percent(value); }

        public Color BorderColor
        { set => SetBorderColour(this, value); }

        public AnchorType Anchor
        { set => SetAnchor(this, value); }

        public StretchType Stretch
        { set => SetStretch(this, value); }

        private void SetBorderColour(VisualElement _element, Color _color)
        {
            _element.style.borderTopColor = _color;
            _element.style.borderRightColor = _color;
            _element.style.borderBottomColor = _color;
            _element.style.borderLeftColor = _color;
        }

        private void SetAnchor(VisualElement _element, AnchorType _anchor)
        {
            Align _align = Align.Center;
            Justify _justify = Justify.FlexStart;
            switch (_anchor)
            {
                case AnchorType.ANCHOR_TOPLEFT:
                    _align = Align.FlexStart;
                    _justify = Justify.FlexStart;
                    break;
                case AnchorType.ANCHOR_TOPRIGHT:
                    _align = Align.FlexEnd;
                    _justify = Justify.FlexStart;
                    break;
                case AnchorType.ANCHOR_TOPCENTER:
                    _align = Align.Center;
                    _justify = Justify.FlexStart;
                    break;
                case AnchorType.ANCHOR_LEFT:
                    _align = Align.FlexStart;
                    _justify = Justify.Center;
                    break;
                case AnchorType.ANCHOR_RIGHT:
                    _align = Align.FlexEnd;
                    _justify = Justify.Center;
                    break;
                case AnchorType.ANCHOR_CENTER:
                    _align = Align.Center;
                    _justify = Justify.Center;
                    break;
                case AnchorType.ANCHOR_BOTTOMLEFT:
                    _align = Align.FlexStart;
                    _justify = Justify.FlexEnd;
                    break;
                case AnchorType.ANCHOR_BOTTOMRIGHT:
                    _align = Align.FlexEnd;
                    _justify = Justify.FlexEnd;
                    break;
                case AnchorType.ANCHOR_BOTTOMCENTER:
                    _align = Align.Center;
                    _justify = Justify.FlexEnd;
                    break;
            }

            if (_element.parent != null)
            {
                _element.parent.style.flexDirection = FlexDirection.Column;
                _element.parent.style.justifyContent = _justify;
            }
            _element.style.alignSelf = _align;
        }

        private void SetStretch(VisualElement _element, StretchType _stretch)
        {
            switch (_stretch)
            {
                case StretchType.STRETCH_WIDTH:
                    _element.style.width = Length.Percent(100);
                    _element.style.height = _height;
                    break;
                case StretchType.STRETCH_HEIGHT:
                    _element.style.width = _width;
                    _element.style.height = Length.Percent(100);
                    break;
                case StretchType.STRETCH_NONE:
                    _element.style.width = _width;
                    _element.style.height = _height;
                    break;
                case StretchType.STRETCH_BOTH:
                    _element.style.width = Length.Percent(100);
                    _element.style.height = Length.Percent(100);
                    break;
            }
        }

        public void SetChildBorder<T>(Color _colour, string _childName = null) 
            where T : VisualElement => SetBorderColour(this.Q<T>(_childName), _colour);
        public void SetChildBorders<T>(Color _colour) where T : VisualElement =>
         this.Query().ForEach((_obj) => { SetBorderColour(_obj, _colour); });

        public void SetChildBorder(Color _colour, string _childName = null) => 
            SetBorderColour(this.Q(_childName), _colour);
        public void SetChildBorders(Color _colour) =>
            this.Query().ForEach((_obj) => { SetBorderColour(_obj, _colour); });

    }
}
