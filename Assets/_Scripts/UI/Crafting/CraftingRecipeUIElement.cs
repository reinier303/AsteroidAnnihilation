using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AsteroidAnnihilation
{
    public class CraftingRecipeUIElement : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
    {
        private CraftingManager craftingManager;
        private CraftingRecipe recipe;
        [SerializeField] private CraftingIngredientUIElement result;
        [SerializeField] private List<CraftingIngredientUIElement> Ingredients;
        private Image image;

        [SerializeField] private Color enterColor;

        public void InitializeRecipe(CraftingRecipe recipe)
        {
            craftingManager = CraftingManager.Instance;
            image = GetComponent<Image>();
            this.recipe = recipe;
            result.InitializeIngredient(recipe.Result, recipe.Amount);

            for(int i = 0; i < recipe.CraftingIngredients.Count; i++)
            {
                RectTransform rect = GetComponent<RectTransform>();
                transform.localScale = new Vector3(1, 1, 1);
                transform.localPosition = new Vector3(0, 0, 0);
                Ingredients[i].gameObject.SetActive(true);
                CraftingIngredient ingredient = recipe.CraftingIngredients[i];
                Ingredients[i].InitializeIngredient(ingredient.ItemNeeded, ingredient.Amount);
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            image.color = new Color(1, 1, 1, 1);
            craftingManager.SetSelectedRecipe(recipe);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            image.color = enterColor;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            image.color = new Color(0, 0, 0, 0);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            image.color = enterColor;
        }
    }
}
