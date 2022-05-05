using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools;

namespace Crafter
{
    public class InventoryView : MonoBehaviour, IInitializer
    {
        public static InventoryView Instance { get; private set; }

        [SerializeField]
        private Canvas canvas;
        [SerializeField]
        private SlotView slotPrefab;
        [SerializeField]
        private Transform slotsHolder;
        [SerializeField]
        private int slotsAmount = 20;

        private Inventory _inventory;
        private Pooler<SlotView> _slotsPool;
        private List<SlotView> _slots;

        #region initialization
        public void InitInstance()
        {
            Instance = this;
        }

        public void Initialize()
        {
            _inventory = Inventory.Instance;
            _inventory.OnReset += OnInventoryReset;

            BuildSlots();
            OnInventoryReset();
        }

        private void BuildSlots()
        {
            SlotView newSlot;
            _slotsPool = new Pooler<SlotView>(slotPrefab);
            _slots = new List<SlotView>();

            for(int i=0; i<slotsAmount;i++)
            {
                newSlot = _slotsPool.GetPooledObject();

                if (newSlot.transform.parent != slotsHolder)
                    newSlot.transform.parent = slotsHolder;

                newSlot.Init(canvas, _inventory);
                _slots.Add(newSlot);
            }
        }
        #endregion

        private void OnInventoryReset()
        {
            Inventory.Item item;

            for (int i = 0; i < _slots.Count; i++)
            {
                if (_inventory.TryGetItem(i, out item))
                    _slots[i].SetItem(item);
                else
                    _slots[i].SetItem(null);
            }
        }

        public void ButtonReset()
        {
            _inventory.ResetInventory();
        }

        private void OnDestroy()
        {
            _inventory.OnReset -= OnInventoryReset;
        }
    }
}