using NUnit.Framework;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/Item")]
public class ItemSO : ScriptableObject
{
    //Item nodes will determine the structure of the item. The structue is created from the top left-most node having right &
    //down item nodes.
    protected class ItemNode
    {
        protected ItemNode rightNode;
        protected ItemNode downNode;
    }

    [Header("Basic Item Properties")]

    [SerializeField]
    Sprite itemSprite;
    public Sprite ItemSprite { get { return itemSprite; } }

    [SerializeField]
    ItemStructureSO itemStructureSO;
    public ItemStructureSO ItemStructureSO { get { return itemStructureSO; } }
}
