using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Crafter
{
    [CreateAssetMenu(fileName = "new item", menuName = "Inventory/Schema")]
    public class Schema : ScriptableObject
    {
        [SerializeField]
        private List<ItemModel> inputItems;
        [SerializeField]
        private ItemModel craftedItem; 

        public bool CanCraft(List<Inventory.Item> itemsOnTable)
        {
            List<ItemModel> existModels = new List<ItemModel>(inputItems);

            for (int i = 0; i < itemsOnTable.Count; i++)
                existModels.Remove(itemsOnTable[i].Model);

            return existModels.Count == 0;
        }

        public Inventory.Item CraftItem()
        {
            return new Inventory.Item(craftedItem);
        }
    }
}