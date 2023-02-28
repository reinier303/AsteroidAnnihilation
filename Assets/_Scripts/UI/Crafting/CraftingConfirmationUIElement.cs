using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace AsteroidAnnihilation
{
    public class CraftingConfirmationUIElement : CraftingRecipeUIElement
    {
        private InventoryManager inventoryManager;

        private void Start()
        {
            inventoryManager = InventoryManager.Instance;
            craftingManager = CraftingManager.Instance;
        }

        public override void InitializeRecipe(CraftingRecipe recipe)
        {
            this.recipe = recipe;
            result.InitializeIngredient(recipe.Result, recipe.Amount);
            DisableAllIngredients();

            for (int i = 0; i < recipe.CraftingIngredients.Count; i++)
            {
                RectTransform rect = GetComponent<RectTransform>();
                if (resetScaleAndPosition) { ResetScaleAndPosition(); }
                Ingredients[i].gameObject.SetActive(true);
                CraftingIngredient ingredient = recipe.CraftingIngredients[i];
                Ingredients[i].InitializeIngredient(ingredient.ItemNeeded, ingredient.Amount, Mathf.Clamp(inventoryManager.GetItemAmountInInventory(ingredient.ItemNeeded.ItemName), 0, 100000));
            }
        }
    }
}
