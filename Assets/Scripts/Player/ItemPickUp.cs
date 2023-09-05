using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Upewnij się, że obiekt, który wchodzi w kolizję, ma komponent Item
        Item item = collision.GetComponent<Item>();

        if (item != null)
        {
            // Pobierz szczegóły przedmiotu
            ItemDetails itemDetails = InventoryManager.Instance.GetItemDetails(item.ItemCode);

            if (itemDetails != null)
            {
                // Wyświetl opis przedmiotu w konsoli
                Debug.Log(itemDetails.itemDescription);
            }
            else
            {
                // Jeśli nie można znaleźć szczegółów przedmiotu, wyświetl błąd
                Debug.LogError("Nie można znaleźć szczegółów przedmiotu dla kodu: " + item.ItemCode);
            }
        }
    }
}
