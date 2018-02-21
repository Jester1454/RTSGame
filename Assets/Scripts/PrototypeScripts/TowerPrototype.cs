using System;
using ObjectBehavior;
using UnityEngine;

namespace PrototypeScripts
{
    [Serializable]
    public class TowerPrototype
    {
        public float maxHP;
        public float Damage;
        public float DamageRate;
        public float AttackRadius;
        public float RotateSpeed;

        public void LoadFromObject(GameObject towerGameObject)
        {
            DefenseTower unit = towerGameObject.GetComponent<DefenseTower>();
            maxHP = towerGameObject.GetComponent<DefenseTower>().maxHP;
            Attack attack = towerGameObject.GetComponent<Attack>();
            Damage = attack.damage;
            DamageRate = attack.damageRate;
            AttackRadius = attack.attackRadius;
            RotateSpeed = unit.rotateSpeed;
        }
    }
}