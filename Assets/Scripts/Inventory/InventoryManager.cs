using UnityEngine;
using System.Collections.Generic;

public class InventoryManager : SingletonMonobehaviour<InventoryManager>
{
    private Dictionary<int, ItemDetails> itemDetailsDictionary;  // Stores item code and corresponding item details
    
    public List<InventoryItem>[] inventoryLists;

    [HideInInspector] public int[] inventoryListCapacityInArray; // the index of the array is the inventory list
    // (from the InventoryLocation enum), and the value is the capacity of the inventory list
    [SerializeField] private SO_ItemList itemList = null; //  is a reference to a Scriptable Object called SO_ItemList, which holds a list of ItemDetails.

    protected override void Awake()
    {
        base.Awake();
        // Create Inventory Lists
        CreateInventoryLists();

        // Create item details dictionary
        CreateItemDetailsDictionary();
    }
    
    private void CreateInventoryLists()
    {
        inventoryLists = new List<InventoryItem>[(int)InventoryLocation.count];

        for (int i = 0; i < (int)InventoryLocation.count; i++)
        {
            inventoryLists[i] = new List<InventoryItem>();
        }
        // Initialize inventory list capacity array
        inventoryListCapacityInArray = new int[(int)InventoryLocation.count];

        // Initialize player inventory list capacity
        inventoryListCapacityInArray[(int)InventoryLocation.player] = Settings.playerInitialInventoryCapacity;
    }

    // Populates the itemDetailsDictionary from the scriptable object items list
    private void CreateItemDetailsDictionary()
    {
        itemDetailsDictionary = new Dictionary<int, ItemDetails>();

        foreach (ItemDetails itemDetails in itemList.itemDetails)
        {
            itemDetailsDictionary.Add(itemDetails.itemCode, itemDetails);
        }
    }
    
    /// <summary>
    /// Add an item to the inventory list for the inventoryLocation
    /// </summary>
    public void AddItem(InventoryLocation inventoryLocation, Item item, GameObject gameObjectToDelete)
    {
        AddItem(inventoryLocation, item);

        Destroy(gameObjectToDelete);
    }
    
    /// <summary>
    /// Add an item to the inventory list for the inventoryLocation
    /// </summary>
    public void AddItem(InventoryLocation inventoryLocation, Item item)
    {
        int itemCode = item.ItemCode;
        List<InventoryItem> inventoryList = inventoryLists[(int)inventoryLocation];

        // Check if inventory already contains the item
        int itemPosition = FindItemInInventory(inventoryLocation, itemCode);

        if (itemPosition != -1)
        {
            AddItemAtPosition(inventoryList, itemCode, itemPosition);

        }
        else
        {
            AddItemAtPosition(inventoryList, itemCode);
        }

        // Send event that inventory had been updated
        EventHandler.CallInventoryUptatedEvent(inventoryLocation, inventoryLists[(int)inventoryLocation]);
    }
    
    /// <summary>
    /// Add item to the end of the inventory
    /// </summary>
    private void AddItemAtPosition(List<InventoryItem> inventoryList, int itemCode)
    {
        InventoryItem inventoryItem = new InventoryItem();

        inventoryItem.itemCode = itemCode;
        inventoryItem.itemQuantity = 1;
        inventoryList.Add(inventoryItem);

       // DebugPrintInventoryList(inventoryList);
    }

    /// <summary>
    /// Add item to position in the inventory
    /// </summary>
    private void AddItemAtPosition(List<InventoryItem> inventoryList, int itemCode, int position)
    {
        InventoryItem inventoryItem = new InventoryItem();

        int quantity = inventoryList[position].itemQuantity + 1;
        inventoryItem.itemQuantity = quantity;
        inventoryItem.itemCode = itemCode;
        inventoryList[position] = inventoryItem;

        //DebugPrintInventoryList(inventoryList);
    }

    /// <summary>
    /// Find if an itemCode is already in the inventory. Returns the item position
    /// in the inventory list, or -1 if the item is not in the inventory
    /// </summary>
    public int FindItemInInventory(InventoryLocation inventoryLocation, int itemCode)
    {
        List<InventoryItem> inventoryList = inventoryLists[(int)inventoryLocation];

        for (int i = 0; i < inventoryList.Count; i++)
        {
            if (inventoryList[i].itemCode == itemCode)
            {
                return i;
            }
        }
        return -1;
    }
    
    public ItemDetails GetItemDetails(int itemCode)
    {
        ItemDetails itemDetails;

        if (itemDetailsDictionary.TryGetValue(itemCode, out itemDetails))
        {
            return itemDetails;
        }
        else
        {
            return null;
        }
    }

    /* private void DebugPrintInventoryList(List<InventoryItem> inventoryList)
    {   
        foreach (InventoryItem inventoryItem in inventoryList)
        {
            Debug.Log("Item Description: " + GetItemDetails(inventoryItem.itemCode).itemDescription + "      Item Quantity: " + inventoryItem.itemQuantity);
        }
        Debug.Log("*****************************************************");
    } */
}
