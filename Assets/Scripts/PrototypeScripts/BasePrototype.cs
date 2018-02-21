using System;
using ObjectBehavior;
using UnityEngine;

namespace PrototypeScripts
{
    [Serializable]
    public class BasePrototype
    {
        public float maxHP;
        public float SpawnRate;
        public float ProduceUnitRate;
        public int MaxUnit;
        public float Damage;
        public float DamageRate;
        public float AttackRadius;
        public int StartCountUnit;

        public void LoadFromObject(GameObject baseGameObject)
        {
            Base _base = baseGameObject.GetComponent<Base>();
            Attack attack = baseGameObject.GetComponent<Attack>();
            Damage = attack.damage;
            DamageRate = attack.damageRate;
            AttackRadius = attack.attackRadius;
            maxHP = _base.maxHP;
            SpawnRate = _base.spawnRate;
            ProduceUnitRate = _base.ProduceRate;
            MaxUnit = _base.MaxCountUnit;
            StartCountUnit = _base.CurrentCountUnit;
        }
    }
}