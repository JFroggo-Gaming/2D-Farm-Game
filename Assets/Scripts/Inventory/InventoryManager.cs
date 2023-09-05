using UnityEngine;
using System.Collections.Generic;

public class InventoryManager : SingletonMonobehaviour<InventoryManager>
{
    private Dictionary<int, ItemDetails> itemDetailsDictionary;  // Stores item code and corresponding item details
    
    [SerializeField] private SO_ItemList itemList = null; //  is a reference to a Scriptable Object called SO_ItemList, which holds a list of ItemDetails.


    protected override void Awake()
    {
        base.Awake();

        // Create item details dictionary
        CreateItemDetailsDictionary();
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

    // Returns the itemDetails (from the SO_ItemList) for the itemCode, or null if the item code doesn't exist
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
}
