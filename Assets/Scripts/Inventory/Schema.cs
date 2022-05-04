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

        public Inventory.Item CraftItem(List<Inventory.Item> itemsOnTable)
        {
            List<ItemModel> existModels = new List<ItemModel>(inputItems);

            for (int i = 0; i < itemsOnTable.Count; i++)
                existModels.Remove(itemsOnTable[i].Model);

            if (existModels.Count > 0)
                return null;
            else
                return new Inventory.Item(craftedItem);
        }
    }
}