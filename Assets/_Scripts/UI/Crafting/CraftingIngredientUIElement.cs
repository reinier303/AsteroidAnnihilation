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
            RectTransform rect = GetComponent<RectTransform>();
            rect.localScale = new Vector3(1, 1, 1);
            rect.anchoredPosition = new Vector3(0, 0, 0);

        }
    }
}
