using UnityEngine;

public class ItemSlot : MonoBehaviour
{
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
}
