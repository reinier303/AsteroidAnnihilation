using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Sirenix.OdinInspector;

namespace AsteroidAnnihilation
{
    public class CraftingRecipeUIElement : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
    {
        protected CraftingManager craftingManager;
        protected CraftingRecipe recipe;
        [SerializeField] protected CraftingIngredientUIElement result;
        [SerializeField] protected List<CraftingIngredientUIElement> Ingredients;
        protected Image image;

        [SerializeField] protected bool pointerEventsEnabled = true;
        [SerializeField] protected bool resetScaleAndPosition = true;

        [ShowIf("pointerEventsEnabled")] [SerializeField] private Color enterColor;


        public virtual void InitializeRecipe(CraftingRecipe recipe)
        {
            craftingManager = CraftingManager.Instance;
            image = GetComponent<Image>();
            this.recipe = recipe;
            result.InitializeIngredient(recipe.Result, recipe.Amount);
            DisableAllIngredients();

            for (int i = 0; i < recipe.CraftingIngredients.Count; i++)
            {
                RectTransform rect = GetComponent<RectTransform>();
                if (resetScaleAndPosition) { ResetScaleAndPosition(); }
                Ingredients[i].gameObject.SetActive(true);
                CraftingIngredient ingredient = recipe.CraftingIngredients[i];
                Ingredients[i].InitializeIngredient(ingredient.ItemNeeded, ingredient.Amount);
            }
        }

        protected void DisableAllIngredients()
        {
            foreach(CraftingIngredientUIElement ingredient in Ingredients)
            {
                ingredient.gameObject.SetActive(false);
            }
        }

        protected void ResetScaleAndPosition()
        {
            transform.localScale = new Vector3(1, 1, 1);
            transform.localPosition = new Vector3(0, 0, 0);
        }

        public virtual void OnPointerDown(PointerEventData eventData)
        {
            if (!pointerEventsEnabled) { return; }
            image.color = new Color(1, 1, 1, 1);
            craftingManager.SetSelectedRecipe(recipe);
        }

        public virtual void OnPointerEnter(PointerEventData eventData)
        {
            if (!pointerEventsEnabled) { return; }
            image.color = enterColor;
        }

        public virtual void OnPointerExit(PointerEventData eventData)
        {
            if (!pointerEventsEnabled) { return; }
            image.color = new Color(0, 0, 0, 0);
        }

        public virtual void OnPointerUp(PointerEventData eventData)
        {
            if (!pointerEventsEnabled) { return; }
            image.color = enterColor;
        }
    }
}
