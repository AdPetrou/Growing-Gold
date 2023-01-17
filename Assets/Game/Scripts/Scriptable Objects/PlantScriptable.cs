using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Animancer;

namespace Game.Plants 
{
    [CreateAssetMenu(fileName = "Plant Type", menuName = "Scriptables/Plant")]
    public class PlantScriptable : ScriptableObject
    {
        [SerializeField] private GameObject _prefab;
        [SerializeField] private AnimationClip _growthAnim;

        [SerializeField] private string _plantName;
        [SerializeField] private float _timeToGrow;
        [SerializeField] private int _value;
        [SerializeField] private int _cost;

        public GameObject CreatePlant(Transform _parent, Vector3 _position)
        {
            GameObject _plant = Instantiate(_prefab, _parent);
            _plant.transform.position = _position;
            _plant.name = _plantName;
            return _plant;
        }

        public void GrowPlant(GameObject _target, float _speed)
        {
            if (_speed <= 0)
                _speed = 0.1f;

            AnimancerComponent _animator = _target.GetComponent<AnimancerComponent>();
            if (_animator == null)
            {
                _animator = _target.AddComponent<AnimancerComponent>();
                _animator.Animator = _target.AddComponent<Animator>();
            }
            
            AnimancerState _state = _animator.Play(_growthAnim);

            // This takes the length of the clip and adds the normalized difference for the arbitrary _timeToGrow variable
            // _timeToGrow may vary and it's easier to set the value in unity than edit the length of the animation
            // The speed will be dynamically determined by the players upgrades, this will be a normalized % value
            _state.Speed = (1 + Mathf.Abs(_growthAnim.length - _timeToGrow) / _growthAnim.length) * _speed;
            StaticCoroutine.Start(FollowAnimation(_target, _state));
        }

        public IEnumerator FollowAnimation(GameObject _target, AnimancerState _state)
        {
            float timeLeft = (_state.NormalizedEndTime - _state.NormalizedTime) 
                * _state.Length / _state.Speed;

            yield return new WaitForSeconds(timeLeft);
            PlantManager.Instance.HarvestablePlants.Add(_target);
        }

        public void HarvestPlant(GameObject _target, Player.PlayerBehaviour _player)
        {
            AnimancerComponent _animator = _target.GetComponent<AnimancerComponent>();
            _animator.Stop(_growthAnim); _player.AddGold(_value);
        }
    } 
}
