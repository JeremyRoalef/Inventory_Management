using UnityEngine;

public class ItemSlot : MonoBehaviour
{
    [SerializeField]
    ItemSO itemInSlot;

    int index;
    public int Index 
    {
        get { return index; }
        set { index = value; } 
    }

    bool isEmpty = true;
    bool IsEmpty
    {
        get { return isEmpty; }
        set { isEmpty = value; }
    }

    private void Awake()
    {
        //Setting this up for testing purposes
        if (itemInSlot == null)
        {
            IsEmpty = true;
        }
        else
        {
            IsEmpty = false;
        }
    }

    public void HandleItemSlotClicked()
    {
        Debug.Log($"{gameObject.name} at index {Index} was pressed");

        /*
         * When the item slot is clicked, communicate with the mouse manager to see if it has an item in hand
         * Conditions for the interaction:
         * 1) if item in hand is empty, and item in slot is empty, nothing should happen
         * 2) if item in hand is empty, and item in slot is not empty, item in slot should go to item in hand
         * 3) if item in hand is not empty, and item in slot is empty, item in hand should go to item in slot
         * 4) if item in hand is not empty, and item in slot is not empty, item in hand and slot should swap places,
         *    only if placing the item in hand into the item slot is accepted (because of different item structures).
         *    More conditions may come into play for greater complexity (i.e. Same items will be stacked and any overflow
         *    will be kept in hand)
         */

        //Get the mouse manager
        MouseManager mouseManager = FindFirstObjectByType<MouseManager>();
        if (mouseManager == null)
        {
            Debug.LogWarning($"No mouse manager in scene. Generating mouse manager!");
            GameObject newMouseManager = new GameObject();
            newMouseManager.AddComponent<MouseManager>();
            mouseManager = newMouseManager.GetComponent<MouseManager>();
        }
        
        bool itemIsInHand = (mouseManager.ItemInHand != null);

        //No item in hand, and no item in slot
        if (!itemIsInHand && IsEmpty)
        {
            Debug.Log("No item in hand and no item in slot. Doing nothing.");
        }
        //No item in hand, item in slot
        if (!itemIsInHand && !IsEmpty)
        {
            Debug.Log("No item in hand and item is in slot. Removing item from slot and into hand...");
        }
        //Item in hand, no item in slot
        if (itemIsInHand && IsEmpty)
        {
            Debug.Log($"Item in hand and no item in slot. Trying to add item into slot and will go from there.");
        }
        //Item in hand and in slot
        if (itemIsInHand && !IsEmpty)
        {
            Debug.Log("Item in hand and in slot. Trying to replace item in slot with item in hand. If successful," +
                "will swap the items. If not, nothing will happen");
        }
    }
}
