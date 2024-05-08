// src* = https://gist.github.com/andrew-raphael-lukasik/8f65a4d7055e29f80376bcb4f9b500af
using UnityEngine;
using UnityEngine.UIElements;

// IMPORTANT NOTE:
// This elemeent doesn't work with flexGrow as it leads to undefined behaviour (recursion).
// Use Size/Width[%] and Size/Height attributes</b> instead

[UnityEngine.Scripting.Preserve]
public class LabelAutoFit : Label
{

    public Axis axis { get; set; }
    public float ratio { get; set; }

    [UnityEngine.Scripting.Preserve]
    public new class UxmlFactory : UxmlFactory<LabelAutoFit, UxmlTraits> { }

    [UnityEngine.Scripting.Preserve]
    public new class UxmlTraits : Label.UxmlTraits// VisualElement.UxmlTraits
    {
        UxmlFloatAttributeDescription _ratio = new UxmlFloatAttributeDescription
        {
            name = "ratio",
            defaultValue = 0.1f,
            restriction = new UxmlValueBounds { min = "0.0", max = "0.9", excludeMin = false, excludeMax = true }
        };
        UxmlEnumAttributeDescription<Axis> _axis = new UxmlEnumAttributeDescription<Axis>
        {
            name = "ratio-axis",
            defaultValue = Axis.Vertical
        };
        public override void Init(VisualElement ve, IUxmlAttributes bag, 
            CreationContext cc)
        {
            base.Init(ve, bag, cc);

            LabelAutoFit instance = ve as LabelAutoFit;
            instance.RegisterCallback<GeometryChangedEvent>(instance.OnGeometryChanged);

            instance.ratio = _ratio.GetValueFromBag(bag, cc);
            instance.axis = _axis.GetValueFromBag(bag, cc);
            instance.style.fontSize = 1;// triggers GeometryChangedEvent
        }
    }

    void OnGeometryChanged(GeometryChangedEvent evt)
    {
        //float oldRectSize = this.axis == Axis.Vertical ? evt.oldRect.height : evt.oldRect.width;
        float newRectLenght = this.axis == Axis.Vertical ? evt.newRect.height : evt.newRect.width;

        float oldFontSize = this.style.fontSize.value.value;
        float newFontSize = newRectLenght * this.ratio;

        float fontSizeDelta = Mathf.Abs(oldFontSize - newFontSize);
        float fontSizeDeltaNormalized = fontSizeDelta / Mathf.Max(oldFontSize, 1);

        if (fontSizeDeltaNormalized > 0.01f)
            this.style.fontSize = newFontSize;
    }

    public enum Axis { Horizontal, Vertical }

}