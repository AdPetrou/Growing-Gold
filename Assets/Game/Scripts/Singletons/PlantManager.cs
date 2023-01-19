using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace Game.Forms.Plants
{
    public class PlantManager : Utilities.Singleton<PlantManager>
    {
        public List<GameObject> HarvestablePlants { get; private set; } = new List<GameObject>();

        public bool HarvestPlant(GameObject _target, PlayerBehaviour _player)
        {
            if (HarvestablePlants.Contains(_target))
            {
                _target.GetComponentInParent<PlanterBehaviour>().PlantType.HarvestPlant(_target, _player);
                HarvestablePlants.Remove(_target);
                return true;
            }
            return false;
        }

        public PlanterBehaviour FindPlanterBehaviour(GameObject _object)
        {
            PlanterBehaviour _planter = _object.GetComponent<PlanterBehaviour>();
            if (_planter == null)
            {
                _planter = _object.GetComponentInParent<PlanterBehaviour>();
                if (_planter == null)
                    return null;
            }

            return _planter;
        }

        public bool IsPlantGrown(GameObject _plant)
        {
            if (_plant && HarvestablePlants.Contains(_plant)) { return true; }
            return false;
        }
    }
}
