using UnityEngine;

public class MouseManager : MonoBehaviour
{
    [SerializeField]
    ItemSO itemInHand;
    public ItemSO ItemInHand
    {
        get { return itemInHand; }
        set { itemInHand = value; }
    }

}
