using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools;

namespace Crafter
{
    public class InventoryView : MonoBehaviour, IInitializer
    {
        public static InventoryView Instance { get; private set; }

        public Transform ItemsHolder { get { return slotsHolder; } }

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
            for (int i = 0; i < _slots.Count; i++)
                _slots[i].SetItem(_inventory.GetItem(i));
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