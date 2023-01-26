using Animancer;
using Game.Forms.Plants;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.Forms.Tools
{
    public abstract class ToolScriptable : FunctionalScriptable
    {
        [MyBox.Separator][Header("Tool Data")]
        [SerializeField] protected bool _requiresPlantGrown;
        [SerializeField] protected PanelSettings _panel;
        [SerializeField] protected RenderTexture _renderTexture;
        [SerializeField] protected VisualTreeAsset _progressBar;
        [SerializeField] protected AnimationClip _useAnim;

        public bool RequiresPlantGrown { get { return _requiresPlantGrown; } }

        public override bool UseObject(GameObject _target, float _offset)
        {
            if (!_target)
                return false;

            PlantManager _manager = PlantManager.Instance;
            var _planter = _manager.FindPlanterBehaviour(_target);
            if (!_planter || 
                (RequiresPlantGrown && !_manager.IsPlantGrown(_planter.Plant)) ||
                (!RequiresPlantGrown && GameManager.Instance.IsBlocked(_planter.Plant)))
                return false;

            return true;
        }

        protected VisualElement CreateWidget(GameObject _parent)
        {
            var _object = new GameObject("Meter"); 
            _object.transform.parent = _parent.transform;
            _object.transform.position = _parent.transform.position + Vector3.up;
            _object.transform.LookAt(Camera.main.transform);

            var _uiDocument = _object.AddComponent<WorldSpaceUIDocument>();
            int _scale = 10;
            _uiDocument.InitPanel(350 * _scale, 40 * _scale, _scale, 350, 
                _progressBar, _panel, _renderTexture);
            _uiDocument.RebuildPanel();

            var _camRotation = Camera.main.transform.eulerAngles;
            _object.transform.eulerAngles = new Vector3(_camRotation.x, 360 + _camRotation.y, 0);

            return _uiDocument.UIWidget;
        }

        protected void SetWidgetFill(VisualElement _widget, 
            float _normalizedPercentage)
        {
            if (_widget.name != "Fill")
            {
                _widget = _widget.Q("Fill");
                if (_widget.name != "Fill")
                    return;
            }

            _widget.style.width = Length.Percent(_normalizedPercentage * 100);
        }

        protected AnimancerState UseAnim(GameObject _target, float _crossFade)
        {
            var _controller = _target.GetComponent<AnimancerComponent>();
            return _controller.Play(_useAnim, _crossFade);
        }

        public override IEnumerator SyncToAnimation(GameObject _target,
    AnimancerState _state, System.Action<bool> _callback = null)
        {
            float increment = 0.05f;
            float timeLeft = (_state.NormalizedEndTime - _state.NormalizedTime)
               * _state.Length / _state.Speed;
            float currentTime = 0;

            var _progressBar = CreateWidget(_target);
            SetWidgetFill(_progressBar, currentTime / timeLeft);

            while (timeLeft > currentTime)
            {
                currentTime += increment;
                SetWidgetFill(_progressBar, currentTime / timeLeft);

                if (timeLeft - currentTime < increment)
                    yield return new WaitForSeconds(timeLeft - currentTime);
                else
                    yield return new WaitForSeconds(increment);
            }
          
            _callback(true);
        }
    }
}
