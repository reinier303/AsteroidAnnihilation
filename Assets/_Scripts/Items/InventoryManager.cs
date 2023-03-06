using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidAnnihilation
{
    public class InventoryManager : SerializedMonoBehaviour
    {
        public static InventoryManager Instance;

        private GameManager gameManager;
        private EquipmentManager equipmentManager;
        private SettingsManager settingsManager;

        public int InventorySlots;
        [SerializeField] Dictionary<int, ItemSlot> ItemSlots;

        public Dictionary<int, ItemData> InventoryItems;
        public Dictionary<int, EquipmentData> InventoryEquipment;
        public Dictionary<int, WeaponData> InventoryWeapons;

        private Transform inventoryPanel;
        Transform weaponSlotParent;
        Transform gearSlotParent;
        Transform componentSlotParent;
        [SerializeField] private GameObject inventorySlot;

        private GameObject DraggedObject;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            settingsManager = SettingsManager.Instance;
            equipmentManager = EquipmentManager.Instance;
            gameManager = GameManager.Instance;
            gameManager.onEndGame += SaveInventory;
        }

        public void InitializeInventory()
        {
            LoadInventory();
            InitializeInventoryItems();
            //TODO::Initialize Components/Trinkets
            InitializeWeapons();
            InitializeGear();
            InitializeShipComponents();
        }

        private void SaveInventory()
        {
            ES3.Save("inventorySlots", InventorySlots);
            ES3.Save("inventoryItems", InventoryItems);
            ES3.Save("inventoryEquipment", InventoryEquipment);
            ES3.Save("inventoryWeapons", InventoryWeapons);
        }

        public void LoadInventory()
        {
            if (!ES3.KeyExists("inventoryItems"))
            {
                InventorySlots = 32;
                InventoryItems = new Dictionary<int, ItemData>();
                InventoryEquipment = new Dictionary<int, EquipmentData>();
                InventoryWeapons = new Dictionary<int, WeaponData>();
                SaveInventory();
            }
            else
            {
                InventorySlots = (int)ES3.Load("inventorySlots");
                InventoryItems = (Dictionary<int, ItemData>)ES3.Load("inventoryItems");
                InventoryEquipment = (Dictionary<int, EquipmentData>)ES3.Load("inventoryEquipment");
                InventoryWeapons = (Dictionary<int, WeaponData>)ES3.Load("inventoryWeapons");
            }
            ItemSlots = new Dictionary<int, ItemSlot>();
            for (int i = 0; i < InventorySlots; i++)
            {
                ItemSlot slot = inventoryPanel.GetChild(i).GetComponent<ItemSlot>();
                slot.gameObject.SetActive(true);
                ItemSlots.Add(i, slot);
            }
        }

        public void OpenInventory()
        {
            InitializeInventoryItems();

            //TODO::Initialize Components/Trinkets
            InitializeWeapons();
            InitializeGear();
            InitializeShipComponents();
        }

        public void SetUIElements(Transform inventoryPanel, Transform weaponSlotParent, Transform gearSlotParent, Transform componentSlotParent)
        {
            if (this.inventoryPanel == null) { this.inventoryPanel = inventoryPanel; }
            if (this.weaponSlotParent == null) { this.weaponSlotParent = weaponSlotParent; }
            if (this.gearSlotParent == null) { this.gearSlotParent = gearSlotParent; }
            if (this.componentSlotParent == null) { this.componentSlotParent = componentSlotParent; }
        }

        private void InitializeInventoryItems()
        {
            foreach(int index in InventoryItems.Keys)
            {
                ItemSlots[index].SetItem(InventoryItems[index]);
                ItemSlots[index].InitializeSlot();
            }
            foreach (int index in InventoryEquipment.Keys)
            {
                ItemSlots[index].SetItem(InventoryEquipment[index]);
                ItemSlots[index].InitializeSlot();
            }
            foreach (int index in InventoryWeapons.Keys)
            {
                ItemSlots[index].SetItem(InventoryWeapons[index]);
                ItemSlots[index].InitializeSlot();
            }
        }

        public void InitializeWeapons()
        {
            int weaponSlots = settingsManager.playerShipSettings.WeaponPositions[EnumCollections.ShipType.Fighter].Count;
            Dictionary<int, WeaponData> weapons = equipmentManager.GetAllEquipedWeapons();
            for (int i = 0; i < weaponSlotParent.childCount; i++)
            {
                GameObject weaponSlot = weaponSlotParent.GetChild(i).gameObject;
                if (i < weaponSlots)
                {
                    weaponSlot.SetActive(true);
                    if (weapons[i].WeaponType == EnumCollections.Weapons.None) { continue; }
                    if (weapons.Count > i) { 
                        weaponSlot.GetComponent<ItemSlot>().SetItem(weapons[i]);
                        weaponSlot.GetComponent<ItemSlot>().InitializeSlot();
                    }
                }
            }
        }

        public void InitializeGear()
        {
            for (int i = 0; i < gearSlotParent.childCount; i++)
            {
                GameObject gearSlot = gearSlotParent.GetChild(i).gameObject;
                ItemSlot slot = gearSlot.GetComponent<ItemSlot>();

                //Debug.Log(equipmentManager.GetGear(slot.slotType).ItemData.ItemName);
                slot.SetItem(equipmentManager.GetGear(slot.slotType));
                slot.GetComponent<ItemSlot>().InitializeSlot();
                //Debug.Log(slot.equipment.ItemData.ItemName);
            }
        }

        public void InitializeShipComponents()
        {
            //Update trinkets/components
        }

        public (bool, ItemData) AddItem(ItemData item, int index = -1, bool fromInventory = false)
        {
            if(!fromInventory)
            {
                int containedItem = GetItemIndex(item.ItemName);
                if (containedItem != -1)
                {
                    ItemData data = InventoryItems[containedItem];
                    data.Amount++;
                    InventoryItems[containedItem] = data;
                    return (true, default);
                }
                else if (!InventoryFull())
                {
                    if (index == -1)
                    {
                        index = GetAvailableSlotIndex();
                        if (index == -1) { return (false, default); }
                    }
                    InventoryItems.Add(index, item);
                    ItemSlots[index].SetItem(item);
                    InitializeInventoryItems();
                    return (true, default);
                }
                else { return (false, default); }
            }
            else
            {
                ItemData data = default;
                if (!InventoryItems.ContainsKey(index))
                {
                    InventoryItems.Add(index, item);
                }
                else
                {
                    data = InventoryItems[index];
                    InventoryItems[index] = item;
                }
                ItemSlots[index].SetItem(item);
                InitializeInventoryItems();
                return (true, data);
            }
        }

        public (bool, EquipmentData) AddItem(EquipmentData equipment, int index = -1)
        {
            if (!InventoryFull())
            {
                EquipmentData data = default;
                if (index == -1)
                {
                    index = GetAvailableSlotIndex();
                    if (index == -1) { return (false, default); }
                }
                if (!InventoryEquipment.ContainsKey(index)) 
                {
                    InventoryEquipment.Add(index, equipment);           
                }
                else { 
                    data = InventoryEquipment[index];
                    InventoryEquipment[index] = equipment;
                }
                ItemSlots[index].SetItem(equipment);
                InitializeInventoryItems();
                InitializeGear();
                InitializeShipComponents();
                equipmentManager.SetEquipmentStats();
                return (true, data);
            }
            else { return (false, default); }
        }

        public (bool, WeaponData) AddItem(WeaponData weapon, int index = -1)
        {
            if (!InventoryFull())
            {
                WeaponData data = default;
                if (index == -1)
                {
                    index = GetAvailableSlotIndex();
                    if (index == -1) { return (false, default); }
                }

                if (!InventoryWeapons.ContainsKey(index)) { InventoryWeapons.Add(index, weapon);}
                else { 
                    data = InventoryWeapons[index];
                    InventoryWeapons[index] = weapon; 
                }
                inventoryPanel.GetChild(index).GetComponent<ItemSlot>().SetItem(weapon);

                InitializeInventoryItems();
                return (true, data);
            }
            else { return (false, default); }
        }

        public ItemData RemoveItem(ItemData item, int index)
        {
            ItemData data = default;
            if (InventoryItems.ContainsKey(index)) { data = InventoryItems[index]; }
            InventoryItems.Remove(index);
            ItemSlots[index].ClearSlot();
            InitializeInventoryItems();
            return data;
        }

        public EquipmentData RemoveItem(EquipmentData equipment, int index)
        {
            EquipmentData data = default;
            if (InventoryEquipment.ContainsKey(index)) { data = InventoryEquipment[index]; }
            InventoryEquipment.Remove(index);
            ItemSlots[index].ClearSlot();
            InitializeInventoryItems();
            InitializeGear();
            InitializeShipComponents();
            equipmentManager.SetEquipmentStats();
            return data;
        }

        public WeaponData RemoveItem(WeaponData weapon, int index)
        {
            WeaponData data = default;
            if (InventoryWeapons.ContainsKey(index)) { data = InventoryWeapons[index]; }
            InventoryWeapons.Remove(index);
            ItemSlots[index].ClearSlot();
            InitializeInventoryItems();
            return data;
        }

        public int GetItemCount()
        {
            return (InventoryItems.Count + InventoryEquipment.Count + InventoryWeapons.Count);
        }

        public bool InventoryFull()
        {
            Debug.Log(GetItemCount() >= InventorySlots);
            return GetItemCount() >= InventorySlots;
        }

        public int GetItemAmountInInventory(EnumCollections.ItemType itemType, string itemName)
        {
            int amount = 0;
            switch (itemType)
            {
                case EnumCollections.ItemType.Material:
                    foreach (int key in InventoryItems.Keys)
                    {
                        if (InventoryItems[key].ItemName == itemName) { return InventoryItems[key].Amount; }
                    }
                    break;
                case EnumCollections.ItemType.Equipment:
                    foreach (int key in InventoryItems.Keys)
                    {
                        if (InventoryEquipment[key].ItemData.ItemName == itemName) { amount++; }
                    }
                    break;
                case EnumCollections.ItemType.Weapon:
                    amount = 0;
                    foreach (int key in InventoryWeapons.Keys)
                    {
                        if (InventoryWeapons[key].EquipmentData.ItemData.ItemName == itemName) { amount++; }
                    }
                    break;
            }
            if (amount > 0) { return amount; }
            return -1;
        }

        public int GetItemIndex(string itemName)
        {
            foreach (int key in InventoryItems.Keys)
            {
                if (InventoryItems[key].ItemName == itemName) { return key; }
            }
            return -1;
        }

        public int GetEquipmentIndex(string itemName)
        {
            foreach (int key in InventoryEquipment.Keys)
            {
                if (InventoryEquipment[key].ItemData.ItemName == itemName) { return key; }
            }
            return -1;
        }

        public int GetWeaponIndex(string itemName)
        {
            foreach (int key in InventoryWeapons.Keys)
            {
                if (InventoryWeapons[key].EquipmentData.ItemData.ItemName == itemName) { return key; }
            }
            return -1;
        }

        public void ReduceItemAmount(EnumCollections.ItemType itemType, string itemName, int reduction)
        {
            int index;
            switch (itemType)
            {
                case EnumCollections.ItemType.Material:
                    index = GetItemIndex(itemName);
                    ItemData data = InventoryItems[index];
                    data.Amount -= reduction;
                    if (data.Amount == 0) { RemoveItem(data, index); }
                    else { InventoryItems[index] = data; }
                    break;
                case EnumCollections.ItemType.Equipment:
                    index = GetEquipmentIndex(itemName);
                    EquipmentData equipmentData = InventoryEquipment[index];
                    RemoveItem(equipmentData, index);
                    break;
                case EnumCollections.ItemType.Weapon:
                    index = GetWeaponIndex(itemName);
                    WeaponData weaponData = InventoryWeapons[index];
                    RemoveItem(weaponData, index);
                    break;
            }

        }

        private int GetAvailableSlotIndex()
        {
            for(int i = 0; i < ItemSlots.Count; i++)
            {
                Debug.Log(ItemSlots[i].slotDataType.ToString());
                if (!ItemSlots[i].ContainsItem())
                {
                    return i;
                }
            }
            return -1;
        }
    }

    public struct ItemData
    {
        public string ItemName;
        public int Tier;
        public EnumCollections.Rarities Rarity;
        public EnumCollections.ItemType ItemType;
        public Sprite Icon;
        public int Amount;
    }
}
