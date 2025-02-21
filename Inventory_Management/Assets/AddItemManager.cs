using UnityEngine;

//A simple script that accepts items to add to inventory and will try to add them.
public class AddItemManager : MonoBehaviour
{
    [SerializeField]
    InventoryGrid inventory;

    [SerializeField]
    ItemSO[] itemsToAdd;

    private void Start()
    {
        //try to add each item to the inventory. If item cannot be added. Skip it.

        foreach (ItemSO item in itemsToAdd)
        {
            inventory.TryAddingItem(item);
        }
    }
}
