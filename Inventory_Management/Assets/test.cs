using UnityEngine;

public class test : MonoBehaviour
{
    [SerializeField]
    ItemSO item;

    private void Start()
    {
        if (item.PivotNode == null)
        {
            Debug.Log($"Item structure for {item.name} not functioning.");
            return;
        }

        Debug.Log($"Testing {item.name} structure\nPosition at (0,0) exists.\nAre there neighbor nodes for" +
            $"this postion?");

        if (item.PivotNode.upNode != null)
        {
            Debug.Log("Up node exists");
        }
        if (item.PivotNode.downNode != null)
        {
            Debug.Log("Down node exists");
        }
        if (item.PivotNode.leftNode != null)
        {
            Debug.Log("left node exists");
        }
        if (item.PivotNode.rightNode != null)
        {
            Debug.Log("right node exists");
        }
    }

}
