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
        
        public void InitializeIngredient(Item item, int amount, int amountAvailable = -1)
        {
            icon.sprite = item.GetIcon();
            if(amountAvailable == -1)
            {
                this.amount.text = "" + amount;
            }
            else { this.amount.text = amountAvailable + "/" + amount; }
        }
    }
}
