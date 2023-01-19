using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Forms.Plants;
using Game.Forms.Tools;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;

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

        private ToolScriptable _activeTool = null;
        private int _hotbarSlots = 2;
        private Hotbar _hotbar;
        private Queue<System.Tuple<ToolScriptable, GameObject>> _toolQueue 
            = new Queue<System.Tuple<ToolScriptable, GameObject>>();
        private bool _toolReady = true;

        public PlantScriptable TempPlantDefault;
        public int Gold { get; private set; } = 0;

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

            _hotbar.AddToSlot(_wateringTool);
            _hotbar.AddToSlot(_farmingTool);
        }

        // Update is called once per frame
        void Update()
        {
            if(_toolReady && _toolQueue.Count > 0)
            {
                _toolReady = false;
                StartCoroutine(ToolCoroutine(_toolQueue.Peek(), 
                    (_returnBool) => 
                    { 
                        _toolQueue.Dequeue(); 
                        _toolReady = _returnBool; 
                    }));
            }
        }

        private IEnumerator ToolCoroutine(System.Tuple<ToolScriptable, GameObject> _tuple,
            System.Action<bool> _callback)
        {
            lock (_toolQueue)
            {
                var _tool = _tuple.Item1; var _target = _tuple.Item2;
                bool _running = _tool.UseObject(_target, 0.5f);
                if (_running)
                    yield return new WaitForSeconds(_tool.GetTime());

                _callback(true);
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

        public void SetActiveTool(ToolScriptable _tool) { _activeTool = _tool; }
        public void RemoveActiveTool() { _activeTool = null; }

        private GameObject SpherecastFromMouse()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit rayOut; Physics.SphereCast(ray, 0.5f, out rayOut, 50);

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