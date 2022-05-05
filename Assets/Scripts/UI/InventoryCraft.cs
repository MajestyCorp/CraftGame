using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Tools;

namespace Crafter
{
    public class InventoryCraft : MonoBehaviour, IInitializer
    {
        private const string c_load_path = "Schemas";

        [SerializeField]
        private Canvas canvas;
        [SerializeField]
        private List<SlotView> slotsIn;
        [SerializeField]
        private SlotView slotOut;
        [SerializeField, Header("Failed craft settings")]
        private float blinkTime = 0.5f;
        [SerializeField]
        private Color blinkColor = Color.red;

        private List<Schema> _schemasLibrary = new List<Schema>();

        private Inventory _inventory;
        private Coroutine _blinkCoroutine;

        #region initialization
        public void InitInstance()
        {
            LoadSchemas();
        }

        public void Initialize()
        {
            _inventory = Inventory.Instance;
            _inventory.OnItemAdded += OnItemAdded;
            _inventory.OnReset += OnInventoryReset;

            InitSlots();
        }

        private void LoadSchemas()
        {
            Schema[] schemasArray = Resources.LoadAll<Schema>(c_load_path);

            _schemasLibrary.Clear();
            if (schemasArray != null && schemasArray.Length > 0)
            {
                for (int i = 0; i < schemasArray.Length; i++)
                    _schemasLibrary.Add(schemasArray[i]);
            }
            else
            {
                Debug.LogWarning("Cant find any schema in Resources/Schemas folder");
            }
        }

        private void InitSlots()
        {
            for (int i = 0; i < slotsIn.Count; i++)
                slotsIn[i].Init(canvas, _inventory);

            slotOut.Init(canvas, _inventory);
        }
        #endregion

        public void ButtonCraft()
        {
            Inventory.Item craftedItem = null;
            List<Inventory.Item> itemsOnTable = GetItemsFromSlots();

            if (slotOut.StoredItem != null)
            {
                CraftFailed();
                return;
            }

            //find correct craft schema
            for (int i = 0; i < _schemasLibrary.Count && craftedItem == null; i++)
                if(_schemasLibrary[i].CanCraft(itemsOnTable))
                    craftedItem = _schemasLibrary[i].CraftItem();

            if(craftedItem != null)
            {
                ConsumeItems();

                _inventory.AddItem(craftedItem);
            } else
            {
                CraftFailed();
            }
        }

        private void CraftFailed()
        {
            if (_blinkCoroutine != null)
                StopCoroutine(_blinkCoroutine);

            _blinkCoroutine = StartCoroutine(AnimateBlinking(slotOut));
        }

        private IEnumerator AnimateBlinking(SlotView slot)
        {
            Timer blinkTimer = new Timer();

            blinkTimer.Activate(blinkTime);

            while (blinkTimer.IsActive)
            {
                slot.SetSlotColor(Color.Lerp(blinkColor, Color.white, blinkTimer.Progress));
                yield return null;
            }

            _blinkCoroutine = null;
        }

        private void ConsumeItems()
        {
            Inventory.Item storedItem;
            for (int i = 0; i < slotsIn.Count; i++)
            {
                storedItem = slotsIn[i].StoredItem;
                if (storedItem != null)
                    if (storedItem.IsConsumable)
                        _inventory.RemoveItem(storedItem);
            }
            
        }

        private List<Inventory.Item> GetItemsFromSlots()
        {
            List<Inventory.Item> items = new List<Inventory.Item>();

            for (int i = 0; i < slotsIn.Count; i++)
                if (slotsIn[i].StoredItem != null)
                    items.Add(slotsIn[i].StoredItem);

            return items;
        }

        private void OnItemAdded(Inventory.Item addedItem)
        {
            slotOut.SetItem(addedItem);
        }

        private void OnInventoryReset()
        {
            for (int i = 0; i < slotsIn.Count; i++)
                slotsIn[i].SetItem(null);

            slotOut.SetItem(null);
        }

        private void OnDestroy()
        {
            _inventory.OnItemAdded -= OnItemAdded;
            _inventory.OnReset -= OnInventoryReset;
        }
    }
}