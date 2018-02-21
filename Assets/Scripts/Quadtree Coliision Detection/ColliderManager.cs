using System.Collections.Generic;
using Cards;
using UnityEngine;
using ObjectBehavior;
using UnityEngine.Profiling;

public class ColliderManager : MonoBehaviour 
{
	#region Field
	
	public bool DrawGizmos;
    Quadtree quad;
	public float width;
	public float height;
	public static ColliderManager instance;

    List<CollisionObject> Objects = new List<CollisionObject>();

	public List<Unit> UnitList = new List<Unit>();

    public List<Base> BaseList;

    public List<DefenseTower> TowerList;

    public List<CollisionObject> ObstacleList;

    public GameObject[] Units;

    private List<GameObject> tempList = new List<GameObject>();

    private List<CollisionObject> ReturnedList = new List<CollisionObject>();

	public List<CollisionObject> naturalDisasters = new List<CollisionObject>();
		
	private bool FirstUpdate = true;
	
	#endregion

	private void Awake()
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
    
		quad= new Quadtree(0, new Rectangle(transform.position.x - width/2, transform.position.y - height/2, width, height));
    }

    public void Update()
    {
	    Profiler.BeginSample("Cleat quad tree");
        UnitList.Clear();
        Objects.Clear();
		quad.Clear();
		Profiler.EndSample();
	    
	    Profiler.BeginSample("Fill quad tree");
	    FiilObjectsList();
        FillQuadTree();
	    Profiler.EndSample();
	    
	    if (FirstUpdate)
	    {
		    Grid.instance.CreateGrid();
		    FirstUpdate = false;
	    }
	    
	    Profiler.BeginSample("Detection collistion");
	    BaseCollision();
       	UnitCollision();
		TowerCollision();
	    Profiler.EndSample();
    }

	public NodeState CloseNode(Vector2 position)
	{
		List<CollisionObject> objects = new List<CollisionObject>();
		quad.Retrieve(objects, position, Grid.instance.nodeRadius);

		for (int i = 0; i < objects.Count; i++)
		{
			float dist = Vector2.Distance(position, objects[i].ObjectTransform.position);
			if (dist <= Grid.instance.nodeRadius + objects[i].SoftRadius)
			{
				if(objects[i].GetType() == typeof(Base))
					return NodeState.Base;
				if(objects[i].GetType() == typeof(Obstacle))
					return  NodeState.Obstacle;
				if(objects[i].GetType() == typeof(DefenseTower))
					return NodeState.Tower;
			}
		}
		return NodeState.Empty;
	}
	
	void TowerCollision()
	{
		for (int i = 0; i < TowerList.Count; i++)
		{
			ReturnedList.Clear();

			quad.Retrieve(ReturnedList, TowerList[i]);

			for (int j = 0; j < ReturnedList.Count; j++)
			{
				if (ReturnedList[j] != TowerList[i])
				{
					float dist = Vector2.Distance(TowerList[i].ObjectTransform.position, ReturnedList[j].ObjectTransform.position);
					if (ReturnedList[j].GetType() != typeof(Obstacle) && ReturnedList[j].GetType()!= typeof(Maelstorm))
						CollisionAttackOrResponse(TowerList[i], (RTSObject)ReturnedList[j], dist);
				}
			}
		}

	}
	
	void UnitCollision()
	{
		for (int i = 0; i < UnitList.Count; i++)
		{
			ReturnedList.Clear();
			quad.Retrieve(ReturnedList, UnitList[i]);
			UnitList[i].move.Group.Clear();
			UnitList[i].move.Group.AddRange(ReturnedList);

			for (int j = 0; j < ReturnedList.Count; j++)
			{
				if (ReturnedList[j] != UnitList[i])
				{
					float dist = Vector2.Distance(UnitList[i].ObjectTransform.position, ReturnedList[j].ObjectTransform.position);
					if (ReturnedList[j].GetType() != typeof(Obstacle))
					{
						if (ReturnedList[j].GetType() == typeof(Maelstorm))
						{
							CollisionWithNatureDisaster(UnitList[i], (NaturalDisaster)ReturnedList[j], dist);
						}
						else
						{
							CollisionAttackOrResponse(UnitList[i], (RTSObject)ReturnedList[j], dist);							
						}

						if (ReturnedList[j].GetType() == typeof(Base))
						{
							CollisionWithBase(UnitList[i], (Base) ReturnedList[j], dist);
						}
					}
				}
			}
		}
	}

	void BaseCollision()
	{
		for (int i = 0; i < BaseList.Count; i++)
		{
			ReturnedList.Clear();
			quad.Retrieve(ReturnedList, BaseList[i]);
			for (int j = 0; j < ReturnedList.Count; j++)
			{
				if (ReturnedList[j] != BaseList[i])
				{
					float dist = Vector2.Distance(BaseList[i].ObjectTransform.position, ReturnedList[j].ObjectTransform.position);
					if (ReturnedList[j].GetType() != typeof(Obstacle) && ReturnedList[j].GetType() != typeof(Maelstorm))
					{
						CollisionAttackOrResponse(BaseList[i], (RTSObject)ReturnedList[j], dist);
					}	
				}
			}
		}
	}
	
	void CollisionWithBase(Unit unit, Base _base, float distance)
	{
		if (distance <= unit.SoftRadius + _base.SoftRadius) // и база не равна текущей цели для юнитов
		{
			if (_base.side == unit.side && unit.move.target == _base.ObjectTransform)
			{
				unit.Recycle();
				_base.UnitComeInBase();
			}
		}		
	}
	
	void CollisionWithNatureDisaster(Unit unit, NaturalDisaster storm, float distance)
	{
		if(distance <= unit.SoftRadius + storm.SoftRadius)
			unit.isNatureDisaster(storm);
		if(distance <= unit.HardRadius + storm.HardRadius)
			unit.OnDead();
	}
	
	void FiilObjectsList()
	{
		Profiler.BeginSample("Fill unit list");

		for (int i = 0; i < Units.Length; i++)
		{
			
			tempList.Clear();
			Units[i].GetSpawned(tempList);
			foreach (GameObject obj in tempList)
			{
				UnitList.Add(obj.GetComponent<Unit>());
				Objects.Add(obj.GetComponent<Unit>());
			}
		}
		Profiler.EndSample();
		
		for (int i = 0; i < BaseList.Count; i++)
		{
			if(BaseList[i].isActiveAndEnabled)
				Objects.Add(BaseList[i]);
		}

		for (int i = 0; i < TowerList.Count; i++)
		{
			if (TowerList[i].isActiveAndEnabled)
				Objects.Add(TowerList[i]);
		}
		
		Objects.AddRange(ObstacleList);
		
		Objects.AddRange(naturalDisasters);
	}
	
	void FillQuadTree()
	{
		for (int i = 0; i < Objects.Count; i++)
		{
			quad.Insert(Objects[i]);
		}
	}
	
    void CollisionAttackOrResponse(AttackObject unit, RTSObject enemy, float dist)
    {
        if (unit.side != enemy.side)
		{
			if (dist <= unit.attack.attackRadius + enemy.SoftRadius)
			{
				unit.Attack(enemy);
			}
		}
	    
	    if(dist > unit.attack.attackRadius + enemy.SoftRadius && unit.attack.IsAttack && enemy==unit.attack.Enemy)
	    {
		    unit.EnemyOut();
	    }
    }

    void OnDrawGizmos()
	{
        if (quad != null)
        {
            DrawTree(quad);
//            DrawTree(quad);
//            DrawTree(quad);
//            DrawTree(quad);
		}
    }

    void DrawTree(Quadtree tree)
    {
        if (DrawGizmos)
        {
            if (tree.nodes[0] != null)
            {
                DrawTree(tree.nodes[0]);
                DrawTree(tree.nodes[1]);
                DrawTree(tree.nodes[2]);
                DrawTree(tree.nodes[3]);
            }
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(new Vector2(tree.bounds.x + tree.bounds.width/2, tree.bounds.y + tree.bounds.height/2), new Vector2(tree.bounds.width, tree.bounds.height));
        }
    }

	public void Shelling(Vector2 position, float radius, float damage)
	{
		List<CollisionObject> objects = new List<CollisionObject>();
		quad.Retrieve(objects, position, radius);

		for (int i = 0; i < objects.Count; i++)
		{	
			if (objects[i].GetType() != typeof(Obstacle) && objects[i].GetType()!= typeof(Maelstorm))
			{
				float dist = Vector2.Distance(position, objects[i].ObjectTransform.position);
				ShellingAttack((RTSObject)objects[i], dist, damage, radius);
			}
		}
	}

	public List<Unit> ReturnUnitInRadius(Vector2 position, float radius)
	{
		List<CollisionObject> objects = new List<CollisionObject>();
		quad.Retrieve(objects, position, radius);
		List<Unit> units = new List<Unit>();
		for (int i = 0; i < objects.Count; i++)
		{
			if (objects[i].GetType() == typeof(Unit))
			{
				float dist = Vector2.Distance(position, objects[i].ObjectTransform.position);
				if (dist <= radius)
					units.Add((Unit)objects[i]);
			}
		}
		return units;
	}

	public Base ReturnBaseInRadius(Vector2 position, float radius)
	{
		List<CollisionObject> objects = new List<CollisionObject>();
		quad.Retrieve(objects, position, radius);
		for (int i = 0; i < objects.Count; i++)
		{
			if (objects[i].GetType() == typeof(Base))
			{
				float dist = Vector2.Distance(position, objects[i].ObjectTransform.position);
				if (dist <= radius)
					return (Base)objects[i];

			}
		}
		return null;
	}
	
	private void ShellingAttack(RTSObject enemyRtsObject, float dist, float damage, float radius)
	{
		if (dist <= radius && enemyRtsObject.side!= Faction.Player)
		{
			enemyRtsObject.Attacked(damage);
		}
	}

	public void DestroyAllUnits()
	{
		for (int i = 0; i < UnitList.Count; i++)
		{
			UnitList[i].Attacked(UnitList[i].maxHP);
		}
	}
	
	public void AddnatureDisaster(CollisionObject obj)
	{
		naturalDisasters.Add(obj);
	}

	public void AddBase(GameObject _base)
	{
		BaseList.Add(_base.GetComponent<Base>());
		AIManager.instance.AddBase(_base);
	}
}
 