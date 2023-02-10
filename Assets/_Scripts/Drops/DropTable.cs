using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Sirenix.OdinInspector;

namespace AsteroidAnnihilation
{
    [CreateAssetMenu(menuName = "DropTable", order = 993)]
    public class DropTable : SerializedScriptableObject
    {
        /// <summary>
        /// Random range for amount of drops from table dropped
        /// </summary>
        public Vector2Int DropRange;
        public List<Drop> Drops;

        public void SortDropTable()
        {
            //TODO::Make this sort the drop table by weigth to make sure our drop chances dont get messed up.
        }

        public Drop GetDrop()
        {
            if(Drops.Count < 1) { return default(Drop); }

            float roll = Random.Range(0, 100f);
            Drop droppedItem = default(Drop);

            foreach(Drop drop in Drops)
            {
                if (roll <= drop.Weight)
                {
                    droppedItem = drop;
                    continue;
                }
                else
                {
                    return droppedItem;
                }            
            }
            return droppedItem;
        }
    }

    [System.Serializable]
    public struct Drop
    {
        /// <summary>
        /// Random range for amount of specified item dropped
        /// </summary>
        public float Weight;
        public Vector2Int AmountRange;
        public EnumCollections.ItemType ItemType;
        [ShowIf("ItemType", EnumCollections.ItemType.Material)] public Item Item;
        [ShowIf("ItemType", EnumCollections.ItemType.ShipComponent)] public Equipment Equipment;
        [ShowIf("ItemType", EnumCollections.ItemType.Weapon)] public Weapon Weapon;

    }
}
