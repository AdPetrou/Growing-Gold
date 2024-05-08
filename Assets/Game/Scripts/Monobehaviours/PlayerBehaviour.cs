using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Forms.Plants;
using Game.Forms.Tools;
using Game.Forms.UI;
using Game.Forms.Wallets;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;
using Game.Forms;

namespace Game
{
    public class PlayerBehaviour : MonoBehaviour
    {
        [MyBox.Separator][Header("Tools")]
        [SerializeField] private IrrigationScriptable _wateringTool;
        [SerializeField] private HarvestingScriptable _farmingTool;
        [MyBox.Separator][Header("Hotbar")]
        [SerializeField] private GameObject _hotbarPrefab;
        [SerializeField] private VisualTreeAsset _slotTree;
        [SerializeField] private PanelSettings _panel;

        private Wallet _wallet = null;
        private ToolScriptable _activeTool = null;
        private int _hotbarSlots = 2;
        private Hotbar _hotbar;
        private Queue<System.Tuple<ToolScriptable, GameObject>> _toolQueue 
            = new Queue<System.Tuple<ToolScriptable, GameObject>>();
        private bool _toolReady = true;

        public List<IShopItem> PersistentItems { get; } = new List<IShopItem>();
        public PlantScriptable TempPlantDefault;
        public int Gold { get; private set; } = 0;
        public Wallet Wallet { get => _wallet; }
        public bool ToolReady { get => _toolReady; set => _toolReady = value; }
        public PanelSettings Panel { get => _panel; }

        // Start is called before the first frame update
        void Start()
        {
            if(TempPlantDefault == null)
            { Debug.LogWarning("Default Plant is not assigned to Player"); return; }

            PlanterBehaviour[] _planters = FindObjectsOfType<PlanterBehaviour>();
            foreach (var _planter in _planters)
                _planter.AddPlant(TempPlantDefault);

            _hotbar = new Hotbar(_hotbarPrefab, _slotTree, 
                _panel, _hotbarSlots);
        }

        // Update is called once per frame
        void Update()
        {
            if(_toolReady && _toolQueue.Count > 0)
            {
                _toolReady = false;
                var _tuple = _toolQueue.Dequeue();
                if (!_tuple.Item1.UseObject(_tuple.Item2, 0.5f))
                    _toolReady = true;
            }
        }

        public void UseTool(InputAction.CallbackContext _context)
        {
            if (!_activeTool)
                return;

            lock (_activeTool)
            {
                if (_context.started)
                {
                    GameObject _object = SpherecastFromMouse();
                    var _tuple = System.Tuple.Create(_activeTool, _object);
                    if (_toolQueue.Contains(_tuple)) return;
                    _toolQueue.Enqueue(_tuple);
                }
            }
        }

        public void SetActiveTool(ToolScriptable _tool) =>  _activeTool = _tool;
        public void RemoveActiveTool() => _activeTool = null;

        /// <summary>
        /// Delete this before release.
        /// Dont use this, this is only for the input system.
        /// </summary>
        /// <param name="_amount"></param>
        public void AddMoney(int _amount) => _wallet.AddAmount(_amount);

        public void EquipTool(ToolScriptable _tool)
        {
            if (_hotbar.ToolTypeList.Contains(_tool.GetType()))
            {
                _hotbar.ReplaceTool(_tool, _hotbar.ToolTypeList.IndexOf(_tool.GetType()));
                return;
            }
            _hotbar.AddToSlot(_tool);
        }

        public void EquipWallet(WalletScriptable _prefab)
        {
            if (_wallet == null)
                _wallet = new Wallet(_prefab, 0);
            else if (_wallet.GetWalletType() == _prefab)
                return;
            else
            {
                _wallet.Destroy();
                if(_wallet.GetAmount() > _prefab.MaxAmount)
                    _wallet = new Wallet(_prefab, _prefab.MaxAmount);
                else
                    _wallet = new Wallet(_prefab, _wallet.GetAmount());
            }      
        }

        private GameObject SpherecastFromMouse()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit rayOut; Physics.SphereCast(ray, 0.5f, out rayOut, 50);

            if (rayOut.point == null)
                return null;

            return rayOut.transform.gameObject;
        }
    }
}