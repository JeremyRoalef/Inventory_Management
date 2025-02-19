using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class InventoryGrid : MonoBehaviour
{
    [SerializeField]
    [Min(1)]
    [Tooltip("Enter the maximum number of columns (the length of a row) for this inventory structure")]
    int numberOfColumns = 1;

    List<GameObject> itemSlots = new List<GameObject>();

    private void Awake()
    {
        InitializeInventoryGrid();
        //DebugItemSlotMethods();
    }

    void InitializeInventoryGrid()
    {
        //Populate the list of item slots based on the number of children in the inventory grid.
        Transform[] childItemSlots = GetComponentsInChildren<Transform>();

        //foreach (Transform itemSlot in childItemSlots)
        //{
        //    Debug.Log($"{itemSlot.name}");
        //}

        if (childItemSlots[0].name == gameObject.name) //Parent object added. Not wanted. index values over by 1
        {
            for (int i = 0; i < childItemSlots.Length; i++)
            {
                if (i == 0) { continue; }
                itemSlots.Add(childItemSlots[i].gameObject);
                SetItemSlotIndex(childItemSlots[i].gameObject, i-1);
            }


            //foreach (Transform item in childItemSlots)
            //{
            //    if (item == childItemSlots[0]) { continue; }
            //    itemSlots.Add(item.gameObject);
            //    SetItemSlotIndex();
            //}
        }
        else //Parent object not added. 
        {
            for (int i = 0; i < childItemSlots.Length; i++)
            {
                itemSlots.Add(childItemSlots[i].gameObject);
                SetItemSlotIndex(childItemSlots[i].gameObject, i);
            }

            //foreach (Transform item in childItemSlots)
            //{
            //    itemSlots.Add(item.gameObject);
            //    SetItemSlotIndex();
            //}
        }

        Debug.Log("Displaying list of item slots...");
        foreach (GameObject item in itemSlots)
        {
            string output = item.name + ": Index at ";
            if (item.TryGetComponent<ItemSlot>(out ItemSlot itemSlot))
            {
                output += $"{itemSlot.Index}";
                Debug.Log(output);
            }
        }
    }

    void SetItemSlotIndex(GameObject itemSlotObj, int index)
    {
        //get the item slot script attached to the game object & tell it that the index value should equal the given index
        if (itemSlotObj.TryGetComponent<ItemSlot>(out ItemSlot itemSlot))
        {
            itemSlot.Index = index;
        }
        else
        {
            Debug.LogWarning($"{itemSlotObj.name} does not have referenced script.");
        }
    }

    /*
     * Methods for getting adjacent item slots in the list
     * Adjacency is determined based on the number of columns (column) and the index of the given item slot (the pivot)
     * 
     * if pivot%column = 0, then there is no left item slot (it's on the left edge of the inventory)
     * if pivot%column = column-1, then there is no right item slot(it's on the right edge of the inventory)
     * up and down item slots determined by pivot-+column (- for up and + for down)
     * left and right item slots determined by pivot-+1 (- for left and + for right), unless the item slot is an edge piece (see above)
     * if calculations become negative or out of index range, then item slot cannot be in the inventory grid.
     */

    public GameObject GetLeftItemSlot(GameObject currentItemSlot)
    {
        //Check if current item slot exists in the list
        if (!itemSlots.Contains(currentItemSlot))
        {
            Debug.Log($"{currentItemSlot.name} does not exist in list");
            return null;
        }

        //Get index of current item slot
        int currentItemSlotIndex = itemSlots.IndexOf(currentItemSlot);
        int leftItemSlotIndex = currentItemSlotIndex - 1;

        //Check if current item lays on the left edge of the grid. If so, there is no left item slot.
        //Also, if the index of left item slot is out of range, then it doesn't eixst.
        if (currentItemSlotIndex % numberOfColumns == 0 || leftItemSlotIndex < 0)
        {
            return null;
        }

        //Left item slot exists
        return itemSlots[leftItemSlotIndex];
    }

    public GameObject GetRightItemSlot(GameObject currentItemSlot)
    {
        //Check if current item slot exists in the list
        if (!itemSlots.Contains(currentItemSlot))
        {
            Debug.Log($"{currentItemSlot.name} does not exist in list");
            return null;
        }

        int currentItemSlotIndex = itemSlots.IndexOf(currentItemSlot);
        int rightItemSlotIndex = currentItemSlotIndex + 1;

        //Check if current item slot lays on the right edge of the grid. If so, there is no right item slot.
        //Also, if the index of right item slot is out of range, then it doesn't exist
        if (currentItemSlotIndex % numberOfColumns == numberOfColumns -1 || rightItemSlotIndex >= itemSlots.Count)
        {
            return null;
        }

        //Right item slot exists
        return itemSlots[rightItemSlotIndex];
    }

    public GameObject GetUpItemSlot(GameObject currentItemSlot)
    {
        //Check if current item slot exists in the list
        if (!itemSlots.Contains(currentItemSlot))
        {
            Debug.Log($"{currentItemSlot.name} does not exist in list");
            return null;
        }

        int currentItemSlotIndex = itemSlots.IndexOf(currentItemSlot);
        int upItemSlotIndex = currentItemSlotIndex - numberOfColumns;

        //Check if upper item slot index is out of range. If so, then there is no upper item slot
        if (upItemSlotIndex < 0)
        {
            return null;
        }

        //Upper item slot exists
        return itemSlots[upItemSlotIndex];
    }

    public GameObject GetDownItemSlot(GameObject currentItemSlot)
    {
        //Check if current item slot exists in the list
        if (!itemSlots.Contains(currentItemSlot))
        {
            Debug.Log($"{currentItemSlot.name} does not exist in list");
            return null;
        }

        int currentItemSlotIndex = itemSlots.IndexOf(currentItemSlot);
        int downItemSlotIndex = currentItemSlotIndex + numberOfColumns;

        //Check if down item slot is index out of range. If so, then there is no down item slot
        if (downItemSlotIndex >= itemSlots.Count)
        {
            return null;
        }

        //Down item slot exists
        return itemSlots[downItemSlotIndex];
    }

    void DebugItemSlotMethods()
    {
        Debug.Log("***TESTING METHODS***");

        int[] indices = { 0, 4, 7, 10, 13, 15, 16, 17, 23 };

        Debug.Log("Retrieving left item slots from indices 0, 4, 7, 10, 13, 15, 16, 17, 23");

        foreach (int i in indices)
        {
            if (GetLeftItemSlot(itemSlots[i]) == null)
            {
                Debug.Log($"NO LEFT NODE FOR {i}");
            }
            else
            {
                Debug.Log($"{i}: {GetLeftItemSlot(itemSlots[i]).name}");
            }
        }

        Debug.Log("************************\n");


        Debug.Log("Retrieving right item slots from indices 0, 4, 7, 10, 13, 15, 16, 17, 23");

        foreach (int i in indices)
        {
            if (GetRightItemSlot(itemSlots[i]) == null)
            {
                Debug.Log($"NO RIGHT NODE FOR {i}");
            }
            else
            {
                Debug.Log($"{i}: {GetRightItemSlot(itemSlots[i]).name}");
            }
        }

        Debug.Log("************************\n");


        Debug.Log("Retrieving upper item slots from indices 0, 4, 7, 10, 13, 15, 16, 17, 23");

        foreach (int i in indices)
        {
            if (GetUpItemSlot(itemSlots[i]) == null)
            {
                Debug.Log($"NO UPPER NODE FOR {i}");
            }
            else
            {
                Debug.Log($"{i}: {GetUpItemSlot(itemSlots[i]).name}");
            }
        }

        Debug.Log("************************\n");


        Debug.Log("Retrieving down item slots from indices 0, 4, 7, 10, 13, 15, 16, 17, 23");

        foreach (int i in indices)
        {
            if (GetDownItemSlot(itemSlots[i]) == null)
            {
                Debug.Log($"NO DOWN NODE FOR {i}");
            }
            else
            {
                Debug.Log($"{i}: {GetDownItemSlot(itemSlots[i]).name}");
            }
        }
    }
}
