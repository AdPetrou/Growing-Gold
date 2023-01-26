using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.Forms.UI
{
    public class ResizableUIController
    {
        VisualElement _root;
        System.Action _action;

        public ResizableUIController(VisualElement _root, System.Action _action)
        {
            this._root = _root;
            this._action = _action;
            OnPostVisualCreation();
        }

        private void OnPostVisualCreation()
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

        private void AutoSize(TimerState obj)
        {
            // Do any measurements, size adjustments you need (NaNs not an issue now)
            _action.Invoke();

            _root.MarkDirtyRepaint();
            _root.visible = true;
            _root.style.visibility = StyleKeyword.Null;
        }
    }
}
