using System.Collections.Generic;
using ObjectBehavior;
using UnityEngine;

public class Quadtree
{
    public int MAX_OBJECTS = 3; //максимальное число объектов до разделения
    public int MAX_LEVELS = 5; //максимальный уровень

	public int level;
    public List<CollisionObject> Objects;
	public Rectangle bounds;
	public Quadtree[] nodes;

	public Quadtree(int _level, Rectangle _bounds)
	{
		level = _level;
        Objects = new List<CollisionObject>();
		bounds = _bounds;
		nodes = new Quadtree[4];
	}

	public void Clear() //  очистить
	{
		Objects.Clear();
		for (int i = 0; i < nodes.Length; i++)
		{
			if (nodes[i] != null)
			{
				nodes[i].Clear();
				nodes[i] = null;
			}
		}
	}
    public void Split() //разделить еще на 4 узла
	{
        float subWidth = (bounds.width / 2);
		float subHeight = (bounds.height / 2);

		float x = bounds.x;
		float y = bounds.y;

        nodes[0] = new Quadtree(level + 1, new Rectangle(x + subWidth, y + subHeight, subWidth, subHeight));
        nodes[1] = new Quadtree(level + 1, new Rectangle(x, y + subHeight, subWidth, subHeight));
		nodes[2] = new Quadtree(level + 1, new Rectangle(x, y, subWidth, subHeight));
		nodes[3] = new Quadtree(level + 1, new Rectangle(x + subWidth, y, subWidth, subHeight));
	}
	
	private int getIndex(CollisionObject obj)
	{
		int index = -1;
		double verticalMidpoint = bounds.x + (bounds.width / 2);
		double horizontalMidpoint = bounds.y + (bounds.height / 2);
 
		// Object can completely fit within the top quadrants
		bool topQuadrant = (obj.ObjectTransform.position.y  - obj.SoftRadius > horizontalMidpoint);
		// Object can completely fit within the bottom quadrants
		bool bottomQuadrant = (obj.ObjectTransform.position.y + obj.SoftRadius < horizontalMidpoint);
 
		// Object can completely fit within the left quadrants
		if (obj.ObjectTransform.position.x + obj.SoftRadius < verticalMidpoint ) {
			if (topQuadrant) {
				index = 1;
			}
			else if (bottomQuadrant) {
				index = 2;
			}
		}
		// Object can completely fit within the right quadrants
		else if (obj.ObjectTransform.position.x - obj.SoftRadius > verticalMidpoint) {
			if (topQuadrant) {
				index = 0;
			}
			else if (bottomQuadrant) {
				index = 3;
			}
		}
 
		return index;
	}
	
	private int getIndex(Vector2 position, float radius)
	{
		int index = -1;
		double verticalMidpoint = bounds.x + (bounds.width / 2);
		double horizontalMidpoint = bounds.y + (bounds.height / 2);
 
		// Object can completely fit within the top quadrants
		bool topQuadrant = (position.y  - radius > horizontalMidpoint);
		// Object can completely fit within the bottom quadrants
		bool bottomQuadrant = (position.y + radius < horizontalMidpoint);
 
		// Object can completely fit within the left quadrants
		if (position.x + radius < verticalMidpoint ) {
			if (topQuadrant) {
				index = 1;
			}
			else if (bottomQuadrant) {
				index = 2;
			}
		}
		// Object can completely fit within the right quadrants
		else if (position.x - radius > verticalMidpoint) {
			if (topQuadrant) {
				index = 0;
			}
			else if (bottomQuadrant) {
				index = 3;
			}
		}
 
		return index;
	}
	
	public List<CollisionObject> Retrieve(List<CollisionObject> returnObjects, Vector2 position, float radius)
	{
		if (nodes[0] != null)
		{
			int index = getIndex(position, radius);
			if (index != -1)
			{
				nodes[index].Retrieve(returnObjects, position, radius);
			}
			else
			{
				for (int i = 0; i < nodes.Length; i++)
				{
					nodes[i].Retrieve(returnObjects, position, radius);
				}
			}
		}

		returnObjects.AddRange(Objects);

		return returnObjects;
	}
	
	public List<CollisionObject> Retrieve(List<CollisionObject> returnObjects, CollisionObject obj)
	{
		if (nodes[0] != null)
		{
			int index = getIndex(obj);
			if (index != -1)
			{
				nodes[index].Retrieve(returnObjects, obj);
			}
			else
			{
				for (int i = 0; i < nodes.Length; i++)
				{
					nodes[i].Retrieve(returnObjects, obj);
				}
			}
		}

        returnObjects.AddRange(Objects);

        return returnObjects;
	}

	public void Insert(CollisionObject obj)
	{
		if (nodes[0] != null)
		{
			int index = getIndex(obj);

			if (index != -1)
			{
				nodes[index].Insert(obj);

				return;
			}
		}
		Objects.Add(obj);

		if (Objects.Count > MAX_OBJECTS && level < MAX_LEVELS)
		{
			if (nodes[0] == null)
			{
				Split();
			}

			int i = 0;
			while (i < Objects.Count)
			{
				int index = getIndex(Objects[i]);
				if (index != -1)
				{
					nodes[index].Insert(Objects[i]);
					Objects.RemoveAt(i);
				}
				else
				{
					i++;
				}
			}
		}
	}
}
