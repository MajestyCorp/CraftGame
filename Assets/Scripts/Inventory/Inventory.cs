using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools;
using System;

namespace Crafter
{
    public class Inventory : MonoBehaviour, IInitializer
    {
        private const string c_load_path = "Items";
        public static Inventory Instance { get; private set; }
        public int ItemsAmount => _items.Count;

        public delegate void SimpleHandler();
        public delegate void ItemHandler(Item item);

        public event SimpleHandler OnReset;
        public event ItemHandler OnItemAdded;
        public event ItemHandler OnItemRemoved;

        [SerializeField]
        private List<Item> initialItems;

        private List<ItemModel> _itemsLibrary = new List<ItemModel>();
        private List<Item> _items = new List<Item>();

        [System.Serializable]
        public class Item
        {
            public ItemModel Model { get => _model; }
            public bool IsConsumable { get => Model.IsConsumable; }

            [SerializeField]
            private ItemModel _model;

            public Item(ItemModel model)
            {
                _model = model;
            }
        }
    

        #region initialization
        public void InitInstance()
        {
            Instance = this;
            LoadItems();
            ResetInventory();
        }

        public void Initialize()
        { }

        private void LoadItems()
        {
            ItemModel[] modelsArray = Resources.LoadAll<ItemModel>(c_load_path);

            _itemsLibrary.Clear();
            if (modelsArray != null && modelsArray.Length > 0)
            {
                for (int i = 0; i < modelsArray.Length; i++)
                    _itemsLibrary.Add(modelsArray[i]);
            }
            else
            {
                Debug.LogWarning("Cant find any items in Resources/Items folder");
            }
        }
        #endregion

        public void ResetInventory()
        {
            _items.Clear();

            for (int i = 0; i < initialItems.Count; i++)
                _items.Add(new Item(initialItems[i].Model));

            OnReset?.Invoke();
        }

        public void AddItem(ItemModel model)
        {
            AddItem(new Item(model));
        }

        public void AddItem(Item newItem)
        {
            if (_items.IndexOf(newItem) >= 0)
                return;

            _items.Add(newItem);

            OnItemAdded?.Invoke(newItem);
        }

        public bool RemoveItem(Item item)
        {
            bool isRemoved = _items.Remove(item);
            if (isRemoved)
                OnItemRemoved?.Invoke(item);
            return isRemoved;
        }

        public bool TryGetItem(int index, out Item item)
        {
            if (index < _items.Count)
            {
                item = _items[index];
                return true;
            }
            else
            {
                item = null;
                return false;
            }
        }
    }
}