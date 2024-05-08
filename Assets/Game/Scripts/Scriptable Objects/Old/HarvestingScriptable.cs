using Animancer;
using Game.Forms.Plants;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Forms.Tools 
{
    //[CreateAssetMenu(fileName = "Harvesting Tool", menuName = "Scriptables/Tools/Harvesting")]
    public class HarvestingScriptable : ToolScriptable, IShopItem
    {
        [SerializeField] protected int _cost;
        [SerializeField][TextArea] protected string _description;

        public string Description => _description;
        public ShopItemType ShopItemType { get { return ShopItemType.Persistent; } }
        public int Cost => _cost;

        public override bool UseObject(GameObject _target, float _yOffset)
        {
            if (!base.UseObject(_target, _yOffset))
                return false;

            PlantManager _manager = PlantManager.Instance;
            var _planter = _manager.FindPlanterBehaviour(_target);
            var _object = CreateObject(_planter.transform,
                _planter.transform.position + new Vector3(-0.3f, _yOffset, -0.1f));

            AddAnimator(_object, 2); var _targetState = UseAnim(_object, 0.5f);
            _targetState.Speed = _useAnim.length / _time;

            StaticCoroutine.Start(SyncToAnimation(_object, _targetState,
                (_returnValue) => 
                {
                    if (_returnValue)
                    {
                        _manager.HarvestPlant(_planter.Plant, 
                        GameManager.Instance.Player); Destroy(_object);
                    }
                })
            );
            return true;
        }

        public void OnShopComplete()
        {
            GameManager.Instance.Player.EquipTool(this);
        }
    }
}
