using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RandomMapGenerator
{
	public class MapLinker : MonoBehaviour
	{		
		public Vector2 GridWorldSize;
		public int CountBase;
		public float BaseRadius;
		
		public GameObject NeutralBase;
		public GameObject EnemyBase;
		public GameObject PlayerBase;
		
		void Start()
		{
			GenerateMap();
		}

		public void GenerateMap()
		{	
			MapCreator creator= new MapCreator(GridWorldSize, CountBase, BaseRadius, transform.position);
			
			Vector2[] BaseCoordinate = creator.CreateBaseMap();
			
			for (int i = 0; i < BaseCoordinate.Length; i++)
			{

				if (i == 3)
				{
					SpawnBase(EnemyBase, BaseCoordinate[i]);
				}
				else
				{
					if (i == 2)
					{
						SpawnBase(PlayerBase, BaseCoordinate[i]);

					}
					else
					{
						SpawnBase(NeutralBase, BaseCoordinate[i]);

					}
				}
			}
		}

		private void SpawnBase(GameObject _base, Vector2 pos)
		{
			GameObject base_ = Instantiate(_base, pos, Quaternion.identity);
			ColliderManager.instance.AddBase(base_);
		}
	}
}
