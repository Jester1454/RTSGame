using System;
using ObjectBehavior;
using UnityEngine;

namespace PrototypeScripts
{
      [Serializable]
      public class UnitPrototype
      {
            public float maxHP;
            public float Damage;
            public float DamageRate;
            public float AttackRadius;
            public float maxSpeed;
      
            public void LoadFromObject(GameObject unitGameObject)
            {
                  maxHP = unitGameObject.GetComponent<Unit>().maxHP;
                  Attack attack = unitGameObject.GetComponent<Attack>();
                  Damage = attack.damage;
                  DamageRate = attack.damageRate;
                  AttackRadius = attack.attackRadius;
                  maxSpeed = unitGameObject.GetComponent<Movement>().maxSpeed;
            }
      }
}