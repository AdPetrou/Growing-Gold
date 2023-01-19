using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Forms.Plants 
{
    public class PlanterBehaviour : MonoBehaviour
    {
        public PlantScriptable PlantType { get; private set; }
        public GameObject Plant { get; private set; }

        public void AddPlant(PlantScriptable _plantType)
        {
            if (PlantType != null || Plant != null)
                RemovePlant();

            PlantType = _plantType;
            Vector3 pos = transform.position;
            Plant = PlantType.CreateObject(transform, new Vector3(pos.x, pos.y + 0.6f, pos.z));
        }

        public void RemovePlant()
        {
            Destroy(Plant);
            PlantType = null;
            Plant = null;
        }
    } 
}
