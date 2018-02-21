using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Grid : MonoBehaviour
{

	public bool displayGridGizmos;

	public Vector2 gridWorldSize;
	public float nodeRadius;
    public static Grid instance; 
	private Node[,] grid;
	float nodeDiameter;

    [HideInInspector]
	public int gridSizeX, gridSizeY;
    Vector2 worldBottomLeft;

    void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
		else
			if (instance != this)
		{
			Destroy(gameObject);
		}
		nodeDiameter = nodeRadius * 2;
		gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
		gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
	}

	public 	void CreateGrid()
	{
		grid = new Node[gridSizeX, gridSizeY];
		worldBottomLeft = (Vector2)transform.position - Vector2.right * gridWorldSize.x / 2 - Vector2.up * gridWorldSize.y / 2;

		for (int x = 0; x < gridSizeX; x++)
		{
			for (int y = 0; y < gridSizeY; y++)
			{
				Vector2 worldPoint = worldBottomLeft + Vector2.right * (x * nodeDiameter + nodeRadius) + Vector2.up * (y * nodeDiameter + nodeRadius);
				//bool walkable = ();
				grid[x, y] = new Node(ColliderManager.instance.CloseNode(worldPoint), worldPoint, x, y);
			}
		}
	}

	public bool CloseNode(Vector2 position)
	{
		Node node = NodeFromWorldPoint(position);
		if (node != null)
		{
			if (node.state == NodeState.Empty)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
		return false;
	}

	public bool CloseNodeWithOutBase(Vector2 position)
	{
		Node node = NodeFromWorldPoint(position);
		if (node != null)
		{
			if (node.state == NodeState.Empty || node.state == NodeState.Base)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
		return false;
	}

	public bool NodeWithBase(Vector2 position)
	{
		Node node = NodeFromWorldPoint(position);
		if (node != null)
		{
			if (node.state == NodeState.Base)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
		return false;
	}
	
	public bool NodeWithTower(Vector2 position)
	{
		Node node = NodeFromWorldPoint(position);
		if (node != null)
		{
			if (node.state == NodeState.Tower)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
		return false;
	}
	
    public Node NodeFromWorldPoint(Vector2 worldPosition)
	{
		float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
		float percentY = (worldPosition.y + gridWorldSize.y / 2) / gridWorldSize.y;
		percentX = Mathf.Clamp01(percentX);
		percentY = Mathf.Clamp01(percentY);

		int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
		int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
		return grid[x, y];
	}

    void OnDrawGizmos()
	{
		Gizmos.DrawWireCube(transform.position, new Vector2(gridWorldSize.x, gridWorldSize.y));
		if (grid != null && displayGridGizmos)
		{
            Color temp;

			foreach (Node n in grid)
			{
               
				temp = Color.white;
				if (n.state==NodeState.Base)
					temp = Color.red;
				temp.a = 0.5f;
				Gizmos.color = temp;
				Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter -0.1f));
			}
		}
	}

}