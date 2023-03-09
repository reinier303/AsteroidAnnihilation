using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidAnnihilation
{
    [CreateAssetMenu(menuName = "CraftingRecipe", order = 994)]
    public class CraftingRecipe : ScriptableObject
    {
        public float UnitCost;
        public List<CraftingIngredient> CraftingIngredients;

        public EnumCollections.ItemType ItemType;
        [ShowIf("ItemType", EnumCollections.ItemType.Material)] public Item ResultItem;
        [ShowIf("ItemType", EnumCollections.ItemType.Equipment)] public Equipment ResultEquipment;
        [ShowIf("ItemType", EnumCollections.ItemType.Weapon)] public Weapon ResultWeapon;

        public int Amount;
        public bool StartUnlocked;
    }

    [System.Serializable]
    public struct CraftingIngredient
    {
        public Item ItemNeeded;
        public int Amount;
    }
}
