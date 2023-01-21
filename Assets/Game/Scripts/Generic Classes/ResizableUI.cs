using UnityEngine.UIElements;

namespace Game.Forms.UI
{
    public abstract class ResizableUI
    {
        public VisualElement Root { get { return _root; } }
        protected VisualElement _root;

        public void OnPostVisualCreation()
        {
            // Make invisble so you don't see the size re-adjustment
            // (Non-visible objects still go through transforms in the layout engine)
            _root.visible = false;
            _root.schedule.Execute(WaitOneFrame);
        }

        private void WaitOneFrame(TimerState obj)
        {
            // Because waiting once wasn't working
            _root.schedule.Execute(AutoSize);
        }

        protected virtual void AutoSize(TimerState obj)
        {
            // Do any measurements, size adjustments you need (NaNs not an issue now)
            _root.MarkDirtyRepaint();
            _root.visible = true;
            _root.style.visibility = StyleKeyword.Null;
        }
    }
}
