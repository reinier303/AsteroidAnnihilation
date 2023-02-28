using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidAnnihilation
{
    [CreateAssetMenu(menuName = "CraftingRecipe", order = 994)]
    public class CraftingRecipe : ScriptableObject
    {
        public List<CraftingIngredient> CraftingIngredients;
        public Item ResultItem;
        public Equipment ResultEquipment;
        public Weapon Result;

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
