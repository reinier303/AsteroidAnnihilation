using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidAnnihilation
{
    [CreateAssetMenu(menuName = "CraftingRecipe", order = 994)]
    public class CraftingRecipe : ScriptableObject
    {
        public List<CraftingComponent> CraftingComponents;
        public Item Result;
        public int Amount;
    }

    [System.Serializable]
    public struct CraftingComponent
    {
        public Item ItemNeeded;
        public int Amount;
    }
}
