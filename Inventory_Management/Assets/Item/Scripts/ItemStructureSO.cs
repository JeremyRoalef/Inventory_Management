using System;
using System.Collections.Generic;
using UnityEngine;


/*
 * This will be used to create customizable item structures.
 * Credit for Idea: ChatGPT.
 * Prompts Given: 
 *  1)In unity, are you not allowed to serialize a 2D array for scriptable objects?
 *  2)what exactly is a jagged array? what makes it unique to an array using "[,]?"
 *  3)I do not see my jagged array in the unity inspector
*/
[Serializable]
public class IntArrayWrapper
{
    public int[] row;
}


[CreateAssetMenu(fileName = "ItemStructureSO", menuName = "ScriptableObjects/ItemStructureSO")]
public class ItemStructureSO : ScriptableObject
{
    [SerializeField]
    [Tooltip("Add rows to the list and enter the maximum length for that row. Set the value for the elements to 1 if the element" +
        "should be contained in the structure, and 0 if the element should not be contained for the structure." +
        "Create the structure from the top-left-most node (element 0,0) and work from there.")]
    List<IntArrayWrapper> itemStructure;


    private string objectName;
    private void OnEnable()
    {
        objectName = name;
    }

    private void OnValidate()
    {
        TestItemStructure();
    }

    void TestItemStructure()
    {
        //This method is used to see if there are any nodes in the item structure that do no have an adjacent node
        //(Other than the first node, which is allowed to exist on its own.)

        //Test if the item structure exists
        if (itemStructure == null || itemStructure.Count == 0)
        {
            Debug.Log($"No item structure for {name}");
            return;
        }

        //Test if pivot point exists
        if (itemStructure[0].row.Length == 0)
        {
            Debug.Log($"Pivot row (row at element 0) does not exist for {name}. Need row at element 0 for item structure");
            return;
        }

        if (itemStructure[0].row[0] == 0)
        {
            Debug.Log($"No pivot node for {name}. (node at 0,0 does not exist)");
            return;
        }

        if (TestNodeConnections())
        {
            Debug.Log($"Item structure {name} is fully connected");
        }
        else
        {
            Debug.Log($"There are nodes that do not connect to pivot in {name}");
        }
    }

    bool TestNodeConnections()
    {
        //Debug.Log($"Testing item structure {name}...");

        //Test if nodes connect to pivot point
        int numberOfNodes = 0;

        //Get the number of nodes in the item structure
        for (int row = 0; row < itemStructure.Count; row++)
        {
            for (int col = 0; col < itemStructure[row].row.Length; col++)
            {
                if (itemStructure[row].row[col] == 1)
                {
                    numberOfNodes++;
                }
            }
        }

        //Debug.Log($"Number of nodes in {name}: {numberOfNodes}");

        //Check if all nodes connect to pivot node (node at element 0,0)

        //list of reached positions in the structure that are part of the structure
        List<Vector2Int> reachedPositions = new List<Vector2Int>();

        //Queue of nodes to connect to pivot
        Queue<Vector2Int> nodesToConnect = new Queue<Vector2Int>();

        //Directions for connecting to pivot
        Vector2Int[] directions = {
            new Vector2Int(-1,0), //left
            new Vector2Int(0,1), //down
            new Vector2Int(1,0), //right
            new Vector2Int(0,-1) //up
        };

        //Test node connections
        reachedPositions.Add(new Vector2Int(0,0));
        numberOfNodes--;

        //Debug.Log($"Exploring directions...");
        foreach (Vector2Int direction in directions)
        {
            //get neighbor node position
            int xPosition = 0 + direction.x;
            int yPosition = 0 + direction.y;

            //If either are negative or greater than the length of structure, then continue to next direction
            //(the direction is not in structure & v2int doesn't accept negatives)
            if (xPosition < 0 || xPosition >= itemStructure.Count)
            {
                //Debug.Log($"Node at position ({xPosition}, {yPosition}) out of range of item structure because of x-coordinate");
                continue;
            }
            if (yPosition < 0 || yPosition >= itemStructure[xPosition].row.Length) 
            {
                //Debug.Log($"Node at position ({xPosition}, {yPosition}) out of range of item structure because of y-coordinate");
                continue; 
            }

            //If the value of the node at the given coordinates equals 1, add to queue
            if (itemStructure[xPosition].row[yPosition] == 1)
            {
                //Debug.Log($"Node at position ({xPosition}, {yPosition}) is part of structure. Adding to queue & reached nodes");
                nodesToConnect.Enqueue(new Vector2Int(xPosition,yPosition));
                reachedPositions.Add(new Vector2Int(xPosition, yPosition));
                numberOfNodes--;
                //Debug.Log("Current Nodes Reached:");
                foreach (Vector2Int pos in reachedPositions)
                {
                    //Debug.Log($"{pos.x},{pos.y}");
                }
            }
            else
            {
                //Debug.Log($"Node at position ({xPosition}, {yPosition}) is not part of structure. Moving to next position...");
            }
        }

        //Loop through queue
        while (nodesToConnect.Count > 0)
        {
            Vector2Int nodePos = nodesToConnect.Dequeue();
            //Debug.Log($"Current node position: ({nodePos.x}, {nodePos.y})");

            //Debug.Log($"Exploring directions...");
            foreach (Vector2Int direction in directions)
            {
                //get neighbor node position
                int xPosition = nodePos.x + direction.x;
                int yPosition = nodePos.y + direction.y;

                //If either are negative or greater than the length of structure, then continue to next direction
                //(the direction is not in structure & v2int doesn't accept negatives)
                if (xPosition < 0 || xPosition >= itemStructure.Count) 
                {
                    //Debug.Log($"Node at position ({xPosition}, {yPosition}) out of range of item structure because of x-coordinate");
                    continue;
                }
                if (yPosition < 0 || yPosition >= itemStructure[xPosition].row.Length) 
                {
                    //Debug.Log($"Node at position ({xPosition}, {yPosition}) out of range of item structure because of y-coordinate");
                    continue;
                }

                //Debug.Log("Current Nodes Reached:");
                foreach (Vector2Int pos in reachedPositions)
                {
                    //Debug.Log($"{pos.x},{pos.y}");
                }


                //If the position has already been reached, then continue to next position
                if (reachedPositions.Contains(new Vector2Int(xPosition,yPosition))) 
                {
                    //Debug.Log($"Node at position ({xPosition}, {yPosition}) has already been reached. moving to next position");
                    continue;
                }

                //If the value of the node at the given coordinates equals 1, add to queue, it's been reached, and reduce remaining
                //number of nodes in structure
                if (itemStructure[xPosition].row[yPosition] == 1)
                {
                    //Debug.Log($"Node at position ({xPosition}, {yPosition}) is part of structure. Adding to queue & to reached nodes");
                    nodesToConnect.Enqueue(new Vector2Int(xPosition, yPosition));
                    reachedPositions.Add(new Vector2Int(xPosition, yPosition));
                    numberOfNodes--;
                }
                else
                {
                    //Debug.Log($"Node at position ({xPosition}, {yPosition}) is not part of node structure");
                }
            }
        }



        //If there are still nodes, then theres a node not connected to the pivot
        if (numberOfNodes != 0)
        {
            Debug.Log($"There are disconnected nodes in {name}. # of disconnected nodes: {numberOfNodes}");

            //Display nodes not connected
            for (int row = 0; row < itemStructure.Count; row++)
            {
                for (int col = 0; col < itemStructure[row].row.Length; col++)
                {
                    Vector2Int pos = new Vector2Int(row, col);
                    if (!reachedPositions.Contains(pos) && itemStructure[row].row[col] == 1)
                    {
                        Debug.Log($"Scriptable object name: {name}\n" +
                            $"Node in row {pos.x}, element {pos.y} is not connected to pivot");
                    }
                }
            }
            return false;
        }

        return true;
    }

    public bool IsValidStructure()
    {
        //This method will be used to ensure that the item structure is valid before using it.

        //Test if the item structure exists
        if (itemStructure == null || itemStructure.Count == 0) {return false;}

        //Test if pivot point exists
        if (itemStructure[0].row.Length == 0) {return false;}
        if (itemStructure[0].row[0] == 0) {return false;}

        //Test if all nodes connect to pivot point
        if (!TestNodeConnections()) {return false;}

        //Struture is valid
        return true;
    }
}
