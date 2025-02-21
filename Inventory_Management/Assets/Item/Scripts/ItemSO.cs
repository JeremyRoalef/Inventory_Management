using NUnit.Framework;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/Item")]
public class ItemSO : ScriptableObject
{
    [Header("Basic Item Properties")]

    [SerializeField]
    Sprite itemSprite;
    public Sprite ItemSprite { get { return itemSprite; } }

    [SerializeField]
    ItemStructureSO itemStructureSO;
    public ItemStructureSO ItemStructureSO { get { return itemStructureSO; } }

    ItemNode pivotNode;
    public ItemNode PivotNode { get { return pivotNode; } }

    private void OnValidate()
    {
        pivotNode = itemStructureSO.TranslateItemStructureToNodes();
        if (pivotNode == null)
        {
            Debug.LogWarning($"{name} does not have a valid structure!");
        }
    }
}
