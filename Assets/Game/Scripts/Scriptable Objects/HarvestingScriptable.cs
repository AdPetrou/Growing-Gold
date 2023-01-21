using Animancer;
using Game.Forms.Plants;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Forms.Tools 
{
    [CreateAssetMenu(fileName = "Plant Type", menuName = "Scriptables/Tools/Harvesting")]
    public class HarvestingScriptable : ToolScriptable, IShopItem
    {
        public ShopItemType ShopItemType { get { return ShopItemType.Persistant; } }

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

        public void OnButtonPress()
        {
            GameManager.Instance.Player.EquipTool(this);
        }
    }
}
