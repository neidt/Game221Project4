using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class TreeGen : MonoBehaviour
{

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}

public class Tree<T>
{
    private TreeNode<T> root;
    public TreeNode<T> Root()
    {
        return root;
    }
    public Tree(T rootValue)
    {
        this.root = new TreeNode<T>(rootValue);
    }

}

public class TreeNode<T>
{
    public TreeNode<T> originNode;
    public TreeNode<T> upChild;
    public TreeNode<T> downChild;
    public TreeNode<T> leftChild;
    public TreeNode<T> rightChild;

    private T innerValue;
    public T Value()
    {
        return innerValue;
    }
    public TreeNode(T nodeValue)
    {
        this.innerValue = nodeValue;
    }

    public TreeNode<T> AddRoom(T roomValue)
    {
        if(leftChild == null)
        {
            leftChild = new TreeNode<T>(roomValue);
            leftChild.originNode = this;
            return leftChild;
        }
        if (rightChild == null)
        {
            rightChild = new TreeNode<T>(roomValue);
            rightChild.originNode = this;
            return rightChild;
        }
        if(upChild == null)
        {
            upChild = new TreeNode<T>(roomValue);
            upChild.originNode = this;
            return upChild;
        }
        if (downChild == null)
        {
            downChild = new TreeNode<T>(roomValue);
            downChild.originNode = this;
            return downChild;
        }
        throw new System.Exception("Cant add more than 4 rooms to a room");
    }

    public bool IsEndNode()
    {
        //path comes from right
        if(upChild == null && downChild == null && leftChild == null)
        {
            return true;
        }
        //path comes from left
        if(upChild == null && downChild == null && rightChild == null)
        {
            return true;
        }
        //path comes from bottom
        if(upChild == null && leftChild == null && rightChild == null)
        {
            return true;
        }
        //path comes from the top
        if(downChild == null && leftChild== null&& rightChild == null)
        {
            return true;
        }

        return false;
    }
}
