using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace AsteroidAnnihilation
{
    public class CraftingIngredientUIElement : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private TextMeshProUGUI amount;
        
        public void InitializeIngredient(Item item, int amount)
        {
            Debug.Log(icon);
            icon.sprite = item.GetIcon();
            this.amount.text = "" + amount;
        }
    }
}
