using System;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using System.Linq;
using UnityEngine;

public class TreeGenDebugger : MonoBehaviour
{
    public int levelWidth;
    public int levelHeight;
    public int numRooms = 8;
    public int count = 0;
    List<TreeNode<GameObject>> leaves = new List<TreeNode<GameObject>>();
    List<TreeNode<GameObject>> processedLeaves = new List<TreeNode<GameObject>>();
    List<TreeNode<GameObject>> roomNodes = new List<TreeNode<GameObject>>();
    public GameObject roomPrefab;

    // Use this for initialization
    void Start()
    {
        Tree<GameObject> sampleObjTree = new Tree<GameObject>(roomPrefab);
        //make the rooms recursively
        /*for (int i = 0; i <= numRooms; i++)
        {
            RecursiveRoomMaker(sampleObjTree.Root());
            DisplayRooms(sampleObjTree);
        }*/
        RecursiveRoomMaker(sampleObjTree.Root());
        DisplayRooms(sampleObjTree);

    }

    private Vector3 LCTWC(TreeNode<GameObject> node)
    {
        TreeNode<GameObject> current = node;
        GameObject gameWorld = node.Value();
        Vector3 localPosition = gameWorld.transform.position;
        Vector3 worldPosition = new Vector3(localPosition.x * 5, 0, localPosition.y * 5);

        return worldPosition;
    }

    private void DisplayRooms(Tree<GameObject> currentNode)
    {
        List<TreeNode<GameObject>> leaves = new List<TreeNode<GameObject>>();
        CollectLeaves(currentNode.Root(), leaves);

        string[,] treeArray = new string[levelWidth, levelHeight];

        int currentLeaf = 0;

        foreach (TreeNode<GameObject> leaf in leaves)
        {
            print("current leaf is: " + currentLeaf.ToString());
            Vector3 leafworld = LCTWC(leaf);
            int leafX = (int)leafworld.x;
            int leafY = (int)leafworld.y;
            
            if (count < numRooms)
            { 
                print("making room tile");
                GameObject room = Instantiate(roomPrefab);
                room.transform.position = leafworld;
                count++;
            }

        }
    }
    private void CollectLeaves(TreeNode<GameObject> currentNode, List<TreeNode<GameObject>> leaves)
    {
        if (currentNode == null)
        {
            return;
        }

        if (currentNode.IsEndNode())
        {
            leaves.Add(currentNode);
        }
        else
        {
            CollectLeaves(currentNode.leftChild, leaves);
            CollectLeaves(currentNode.rightChild, leaves);
            CollectLeaves(currentNode.upChild, leaves);
            CollectLeaves(currentNode.downChild, leaves);
        }
    }//end collect leaves

    private void RecursiveRoomMaker(TreeNode<GameObject> treeNode)
    {
        if (treeNode.IsEndNode())
        {
            leaves.Add(treeNode);
            return;
        }
        processedLeaves.Add(treeNode);
        //if its not a end leaf, divide again
        if (treeNode.leftChild != null)
        {
            RecursiveRoomMaker(treeNode.leftChild);
        }
        if (treeNode.rightChild != null)
        {
            RecursiveRoomMaker(treeNode.rightChild);
        }
        if (treeNode.upChild != null)
        {
            RecursiveRoomMaker(treeNode.upChild);
        }
        if (treeNode.downChild != null)
        {
            RecursiveRoomMaker(treeNode.downChild);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
