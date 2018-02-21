using UnityEngine;


public enum NodeState
{
	Obstacle,
	Base,
	Tower,
	Empty
}

public class Node 
{
	public NodeState state;
	public Vector2 worldPosition;
	public int gridX;
	public int gridY;

	public Node(NodeState _state, Vector2 _worldPos, int _gridX, int _gridY)
	{
		state = _state;
		worldPosition = _worldPos;
		gridX = _gridX;
		gridY = _gridY;
    }
}