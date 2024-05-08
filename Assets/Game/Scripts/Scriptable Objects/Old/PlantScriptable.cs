using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Animancer;
using static UnityEngine.GraphicsBuffer;

namespace Game.Forms.Plants 
{
    //[CreateAssetMenu(fileName = "Plant", menuName = "Scriptables/Plant")]
    public class PlantScriptable : FunctionalScriptable, IShopItem
    {
        [SerializeField] private int _value;
        [SerializeField] protected int _cost;
        [SerializeField][TextArea] protected string _description;

        public ShopItemType ShopItemType => ShopItemType.Researchable;
        public string Description => _description;
        public int Cost => _cost;

        public override bool UseObject(GameObject _target, float _speed)
        {
            if (_speed <= 0)
                _speed = 0.1f;

            AnimancerState _state = AddAnimator(_target, 0);

            // This takes the length of the clip and adds the normalized difference for the arbitrary _timeToGrow variable
            // _timeToGrow may vary and it's easier to set the value in unity than edit the length of the animation
            // The speed will be dynamically determined by the players upgrades, this will be a normalized % value
            _state.Speed = _anim.length / _time * _speed;
            StaticCoroutine.Start(SyncToAnimation(_target, _state));
            return true;
        }

        protected override IEnumerator SyncToAnimation(GameObject _target, 
            AnimancerState _state, System.Action<bool> _callback = null)
        {
            float timeLeft = (_state.NormalizedEndTime - _state.NormalizedTime) 
                * _state.Length / _state.Speed;

            yield return new WaitForSeconds(timeLeft);
            PlantManager.Instance.HarvestablePlants.Add(_target);
        }

        public void HarvestPlant(GameObject _target, PlayerBehaviour _player)
        {
            AnimancerComponent _animator = _target.GetComponent<AnimancerComponent>();
            _animator.Stop(_anim);

            if(_player.Wallet != null)
                _player.Wallet.AddAmount(_value);

            Destroy(_target.GetComponent<Utilities.Blocker>());
        }

        public void OnShopComplete()
        {
            throw new System.NotImplementedException();
        }
    } 
}
