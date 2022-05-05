using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Crafter
{
    [CreateAssetMenu(fileName = "new item", menuName = "Inventory/Item")]
    public class ItemModel : ScriptableObject
    {
        public Sprite ItemSprite => itemSprite;
        public bool IsConsumable => consumable; 

        [SerializeField]
        private Sprite itemSprite;
        [SerializeField]
        private bool consumable;
    }
}