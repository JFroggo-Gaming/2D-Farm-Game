using System.Collections.Generic;
using UnityEngine;

// ScriptableObject to store a list of item details
[CreateAssetMenu(fileName = "so_ItemList", menuName = "Scriptable Objects/Item/ItemList")]
public class SO_ItemList : ScriptableObject
{
    [SerializeField]
    public List<ItemDetails> itemDetails; // List of item details contained in this ScriptableObject
}
