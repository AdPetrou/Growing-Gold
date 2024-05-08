using Animancer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Forms.Plants;
using MyBox;

namespace Game.Forms.Tools
{
    //[CreateAssetMenu(fileName = "Irrigation Tool", menuName = "Scriptables/Tools/Irrigation")]
    public class IrrigationScriptable : ToolScriptable, IShopItem
    {
        [SerializeField] protected int _cost;
        [SerializeField] protected float _efficieny;
        [SerializeField][TextArea] protected string _description;

        public string Description => _description;
        public ShopItemType ShopItemType { get { return ShopItemType.Persistent; } }
        public int Cost => _cost;

        public override bool UseObject(GameObject _target, float _yOffset)
        {
            if (!base.UseObject(_target, _yOffset))
                return false;

            var _planter = PlantManager.Instance.FindPlanterBehaviour(_target);
            GameManager.Instance.AddBlocker(_planter.Plant);
            var _object = CreateObject(_planter.transform,
                _planter.transform.position + new Vector3(-0.6f, _yOffset + 0.5f, 0));

            AddAnimator(_object, 2); var _targetState = UseAnim(_object, 0.5f);
            _targetState.Speed = _useAnim.length / _time;

            StaticCoroutine.Start(SyncToAnimation(_object, _targetState, 
                (_returnValue) => 
                { 
                    if (_returnValue)
                    { 
                        _planter.PlantType.UseObject(_planter.Plant, _efficieny);
                        Destroy(_object);
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
