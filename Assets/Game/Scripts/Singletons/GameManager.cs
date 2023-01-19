using Animancer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Utilities;

namespace Game
{
    public class GameManager : Singleton<GameManager>
    {
        public PlayerBehaviour Player 
        { 
            get 
            { 
                if (_player == null)
                    _player = FindObjectOfType<PlayerBehaviour>();

                return _player;
            }
        }
        private PlayerBehaviour _player;

        // Start is called before the first frame update
        void Start()
        {
            _player = FindObjectOfType<PlayerBehaviour>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public AnimancerState GetState(GameObject _object)
        {
            AnimancerComponent _animator = _object.GetComponent<AnimancerComponent>();
            if (!_animator)
                return null;

            return _animator.States.Current;
        }

        public Blocker AddBlocker(GameObject _target)
        {
            Blocker blocker = _target.GetComponent<Blocker>();
            if(blocker == null)
                blocker = _target.AddComponent<Blocker>();

            return blocker;
        }
        public bool IsBlocked(GameObject _target)
        {
            Blocker blocker = _target.GetComponent<Blocker>();
            if (blocker == null)
                return false;

            return true;
        }
    }
}
