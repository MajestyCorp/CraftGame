using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Crafter
{
    [CreateAssetMenu(fileName = "new item", menuName = "Inventory/Item")]
    public class ItemModel : ScriptableObject
    {
        public Sprite ItemSprite { get { return itemSprite; } }
        public bool IsConsumable { get { return consumable; } }

        [SerializeField]
        private Sprite itemSprite;
        [SerializeField]
        private bool consumable;
    }
}