using UnityEngine;
using UnityEngine.UIElements;

namespace Game.Forms.UI
{
    public abstract class ResizableUI : VisualElement
    {
        public ResizableUI() : base() { }

        public void OnPostVisualCreation()
        {
            // Make invisble so you don't see the size re-adjustment
            // (Non-visible objects still go through transforms in the layout engine)
            visible = false;
            schedule.Execute(WaitOneFrame);
        }

        private void WaitOneFrame(TimerState obj)
        {
            // Because waiting once wasn't working
            schedule.Execute(PostLayout);
        }

        protected virtual void PostLayout(TimerState obj)
        {
            // Do any measurements, size adjustments you need (NaNs not an issue now)
            MarkDirtyRepaint();
            visible = true;
            style.visibility = StyleKeyword.Null;
        }
    }
}
