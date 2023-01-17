using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Plants
{
    public class PlantManager : Game.Utilities.Singleton<PlantManager>
    {
        public List<GameObject> HarvestablePlants { get; private set; } = new List<GameObject>();

        public bool HarvestPlant(GameObject _target, Player.PlayerBehaviour _player)
        {
            if (HarvestablePlants.Contains(_target))
            {
                _target.GetComponentInParent<PlanterBehaviour>().PlantType.HarvestPlant(_target, _player);
                HarvestablePlants.Remove(_target);
                return true;
            }
            return false;
        }
    }
}
