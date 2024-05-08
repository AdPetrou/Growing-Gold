using Game;
using Game.Forms.Plants;
using Game.Forms.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.Instance.BootStrap)
            GameManager.Instance.BootStrap = false;
        if (UIManager.Instance.BootStrap)
            UIManager.Instance.BootStrap = false;
        if (PlantManager.Instance.BootStrap)
            PlantManager.Instance.BootStrap = false;

    }
}
