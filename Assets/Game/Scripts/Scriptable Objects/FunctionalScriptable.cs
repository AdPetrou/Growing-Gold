using Animancer;
using Game.Forms;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Forms
{
    public abstract class FunctionalScriptable : GenericScriptable
    {
        [SerializeField] protected GameObject _prefab;

        [SerializeField] protected AnimationClip _anim;
        [SerializeField] protected float _time;

        public override GameObject CreateObject(Transform _parent, Vector3 _position)
        {
            GameObject _object = Instantiate(_prefab, _parent);
            _object.transform.position = _position;
            _object.name = _name;
            return _object;
        }

        public abstract bool UseObject(GameObject _object, float _offset);
        public abstract IEnumerator SyncToAnimation(GameObject _target,
            AnimancerState _state, System.Action<bool> _callback = null);
        public AnimationClip GetAnim() { return _anim; }

        protected virtual AnimancerState AddAnimator(GameObject _target, float _crossFade)
        {
            AnimancerComponent _animator = _target.GetComponent<AnimancerComponent>();
            if (_animator == null)
            {
                _animator = _target.AddComponent<AnimancerComponent>();
                _animator.Animator = _target.AddComponent<Animator>();
            }
            AnimancerState _targetState = _animator.Play(_anim, _crossFade);
            return _targetState;
        }

        public float GetTime() { return _time; }
    }
}
