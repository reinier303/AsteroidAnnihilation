using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidAnnihilation
{
    public class CraftingRecipeUIElement : MonoBehaviour
    {
        private CraftingRecipe recipe;
        [SerializeField] private CraftingIngredientUIElement result;
        [SerializeField] private List<CraftingIngredientUIElement> Ingredients;

        public void InitializeRecipe(CraftingRecipe recipe)
        {
            this.recipe = recipe;
            result.InitializeIngredient(recipe.Result, recipe.Amount);

            for(int i = 0; i < recipe.CraftingIngredients.Count; i++)
            {
                Ingredients[i].gameObject.SetActive(true);
                CraftingIngredient ingredient = recipe.CraftingIngredients[i];
                Ingredients[i].InitializeIngredient(ingredient.ItemNeeded, ingredient.Amount);
            }
        }
    }
}
