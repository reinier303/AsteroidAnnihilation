using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AsteroidAnnihilation
{
    public class CraftingManager : MonoBehaviour
    {
        public static CraftingManager Instance;

        private InventoryManager inventoryManager;
        private ObjectPooler objectPooler;
        private SettingsManager settingsManager;

        [SerializeField] private Transform recipesHolder;

        //boolean is for unlocked state
        private Dictionary<CraftingRecipe, bool> recipes;

        private CraftingRecipe selectedRecipe;
        [SerializeField] private CraftingRecipeUIElement craftingConfirmationUIElement;

        // Start is called before the first frame update
        private void Awake()
        {
            Instance = this;
            LoadUnlockedRecipes();
            LoadRecipesFromResources();
        }

        private void Start()
        {
            inventoryManager = InventoryManager.Instance;
            objectPooler = ObjectPooler.Instance;
            settingsManager = SettingsManager.Instance;
            ShowRecipes();
        }

        private void LoadRecipesFromResources()
        {
            Object[] resourcesRecipes = Resources.LoadAll("CraftingRecipes", typeof(CraftingRecipe));
            foreach (CraftingRecipe recipe in resourcesRecipes)
            {
                //Check if recipe info is filled.
                if (recipe.CraftingIngredients.Count > 0 &&  recipe.Amount > 0  && recipe.ResultWeapon != null || recipe.ResultEquipment != null || recipe.ResultItem != null)
                {
                    //We check it this way to make sure new recipes will be added when updating while old recipes will remain properly saved.
                    if (!recipes.ContainsKey(recipe)) { recipes.Add(recipe, false); }
                    if (recipe.StartUnlocked) { recipes[recipe] = true; }
                }
                else
                {
                    Debug.LogWarning("Recipe: " + recipe.name + " is missing some information. \n Please go back to Resources/CraftingRecipes and fill in the information correctly");
                }
            }
        }

        private void LoadUnlockedRecipes()
        {
            if (!ES3.KeyExists("unlockedRecipes"))
            {
                recipes = new Dictionary<CraftingRecipe, bool>();
                SaveRecipes();
            }
            else
            {
                recipes = ES3.Load("unlockedRecipes", defaultValue: new Dictionary<CraftingRecipe, bool>());
            }
        }

        private void SaveRecipes()
        {
            ES3.Save("unlockedRecipes", recipes);
        }

        public void ShowRecipes()
        {
            foreach(CraftingRecipe recipe in recipes.Keys)
            {
                if (recipes[recipe])
                {
                    //Make this without Objectpooler
                    GameObject recipeUI = objectPooler.SpawnFromPool("CraftingRecipeUI", Vector2.zero, Quaternion.identity);
                    recipeUI.transform.SetParent(recipesHolder);
                    recipeUI.GetComponent<CraftingRecipeUIElement>().InitializeRecipe(recipe);
                }
            }
        }

        public void SetSelectedRecipe(CraftingRecipe recipe)
        {
            selectedRecipe = recipe;
            craftingConfirmationUIElement.InitializeRecipe(selectedRecipe);
        }

        public void TryCraftItem()
        {
            if(CheckRecipeRequirements())
            {
                CraftItem();
            }
        }

        public bool CheckRecipeRequirements()
        {
            foreach(CraftingIngredient ingredient in selectedRecipe.CraftingIngredients)
            {
                if(inventoryManager.GetItemAmountInInventory(ingredient.ItemNeeded.ItemName) < ingredient.Amount)
                {
                    return false;
                }
            }
            return true;
        }

        public void CraftItem()
        {
            foreach (CraftingIngredient ingredient in selectedRecipe.CraftingIngredients)
            {
                inventoryManager.ReduceItemAmount(ingredient.ItemNeeded.ItemName, ingredient.Amount);
            }
            switch(selectedRecipe.ItemType)
            {
                case EnumCollections.ItemType.Material:
                    inventoryManager.AddItem(selectedRecipe.ResultItem.GenerateItemData(1));
                    break;
                case EnumCollections.ItemType.Equipment:
                    inventoryManager.AddItem(selectedRecipe.ResultEquipment.GenerateEquipmentData(settingsManager.generalItemSettings));
                    break;
                case EnumCollections.ItemType.Weapon:
                    inventoryManager.AddItem(selectedRecipe.ResultWeapon.GenerateWeaponData(settingsManager.generalItemSettings));
                    break;
            }
        }
    }
}
