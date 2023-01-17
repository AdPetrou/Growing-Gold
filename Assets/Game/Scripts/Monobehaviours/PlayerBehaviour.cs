using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Plants;

namespace Game.Player
{
    public class PlayerBehaviour : MonoBehaviour
    {
        public PlantScriptable TempPlantDefault;
        public int Gold { get; private set; } = 0;

        [Range(0, 5)][SerializeField] private float PlayerGrowSpeed = 0.1f;

        // Start is called before the first frame update
        void Start()
        {
            if(TempPlantDefault == null)
            { Debug.LogWarning("Default Plant is not assigned to Player"); return; }

            PlanterBehaviour[] _planters = FindObjectsOfType<PlanterBehaviour>();
            foreach (var _planter in _planters)
                _planter.AddPlant(TempPlantDefault);
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
                OnPointerClick();
            else if (Input.GetMouseButtonDown(1))
                OnPointerUse();
        }

        public void OnPointerClick()
        {
            GameObject _object = SpherecastFromMouse();

            PlanterBehaviour _planter = _object.GetComponent<PlanterBehaviour>();
            if(_planter == null)
                _planter = _object.GetComponentInParent<PlanterBehaviour>();
                if (_planter == null)
                    return;

            _planter.PlantType.GrowPlant(_planter.Plant, PlayerGrowSpeed);
        }
        
        public void OnPointerUse()
        {
            GameObject _target = SpherecastFromMouse();
            bool correct = PlantManager.Instance.HarvestPlant(_target, this);
            if(!correct)
            {
                var _planter = _target.GetComponent<PlanterBehaviour>();
                if(_planter != null)
                    PlantManager.Instance.HarvestPlant(_planter.Plant, this);
            }
        }

        private GameObject SpherecastFromMouse()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit rayOut; Physics.SphereCast(ray, 0.1f, out rayOut, 50);

            if (rayOut.point == null)
                return null;

            return rayOut.transform.gameObject;
        }

        public int AddGold(int _amount)
        {
            Gold += _amount;
            return Gold;
        }

        public int RemoveGold(int _amount)
        {
            if (_amount > Gold)
            {
                Debug.Log("Not Enough Gold");
                return -1;
            }

            Gold -= _amount;
            return Gold;
        }
    }
}