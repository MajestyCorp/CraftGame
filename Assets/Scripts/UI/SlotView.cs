using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Crafter
{
    public class SlotView : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
    {
        public Inventory.Item StoredItem => _storedItem;
        public bool AllowDropIn => allowDropIn;

        [SerializeField]
        private Image slotImage;
        [SerializeField]
        private Image itemIcon;
        [SerializeField]
        private RectTransform dragObject;
        [SerializeField]
        private bool allowDropIn = true;

        private Inventory.Item _storedItem;
        private Canvas _canvas;
        private Inventory _inventory;

        public void Init(Canvas canvas, Inventory inventory)
        {
            _canvas = canvas;
            _inventory = inventory;

            _inventory.OnItemRemoved += OnItemRemoved;

            SetItem(null);
            gameObject.SetActive(true);
        }
        #region drag interfaces
        public void OnBeginDrag(PointerEventData eventData)
        {
            if (_storedItem == null)
                return;

            dragObject.parent = _canvas.transform;
            dragObject.SetAsLastSibling();
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            dragObject.parent = this.transform;
            dragObject.anchoredPosition = Vector2.zero;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (_storedItem == null)
                return;

            dragObject.anchoredPosition += eventData.delta / _canvas.scaleFactor;
        }

        public void OnDrop(PointerEventData eventData)
        {
            Inventory.Item swapItem;
            SlotView draggedSlot;
            if (eventData.pointerDrag == null || !eventData.pointerDrag.TryGetComponent<SlotView>(out draggedSlot) ||
                !allowDropIn)
                return;

            //if dragged slot is locked and player tries to swap items - forbid the action
            if (StoredItem != null && !draggedSlot.AllowDropIn)
                return;

            swapItem = StoredItem;
            this.SetItem(draggedSlot.StoredItem);
            draggedSlot.SetItem(swapItem);
        }
        #endregion
        public void SetItem(Inventory.Item item)
        {
            _storedItem = item;
            UpdateData();
        }

        public void SetSlotColor(Color color)
        {
            slotImage.color = color;
        }

        private void UpdateData()
        {
            if (_storedItem != null)
                itemIcon.sprite = _storedItem.Model.ItemSprite;

            itemIcon.gameObject.SetActive(_storedItem != null);
        }

        private void OnItemRemoved(Inventory.Item removedItem)
        {
            if (StoredItem != null && StoredItem == removedItem)
                SetItem(null);
        }

        private void OnDestroy()
        {
            _inventory.OnItemRemoved -= OnItemRemoved;
        }
    }
}