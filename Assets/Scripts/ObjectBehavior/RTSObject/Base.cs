using System;
using System.Collections.Generic;
using System.Collections;
using PrototypeScripts;
using UnityEngine;

namespace ObjectBehavior
{
    [Serializable]
    public class Base : AttackObject
    {
        public float CaptureRadius;

        private Unit UnitPrefabs;

        public float spawnRate = 4.0f;
        public float ProduceRate = 4.0f;

        private Vector2 positionSpawn;

        public int MaxCountUnit;
        public int CurrentCountUnit;

        private CircularHPBar bar;
        public bool DrawGizmos;

        private int level = 1;

        private bool isSpawn = false;
        private Coroutine spawnCoroutine;
        private int SpawnCountUnit = 0;
        
        private BaseAnimator animator;
        [NonSerialized] public bool isRuinded = false;
        private Transform UnitTarget;

        private void Awake()
        {
            state = AttackObjectState.isAlive;
            ObjectTransform = transform;
            currentHP = maxHP;
        }

        void Start()
        {
            attack = GetComponent<Attack>();
            animator = GetComponent<BaseAnimator>();
            positionSpawn = (Vector2)GetComponent<Transform>().position + new Vector2(SoftRadius, -SoftRadius); //заглушка для ИИ
            bar = GetComponent<CircularHPBar>();
            InitHUD();

            animator.UpdateCountText();
            animator.CaptureAnimation();
            StartCoroutine(FromPrototype());
            UnitPrefabs = SideUnitPrefabs();
            StartCoroutine(ProduceUnit());
        }

        private void InitHUD()
        {
            GameObject HP_Base = Resources.Load("HUD/HP_Base", typeof(GameObject)) as GameObject;

            HP_Base.GetComponent<UIWidget>().SetAnchor(gameObject);
            
            HP_Base = Instantiate(HP_Base, ObjectTransform.position, Quaternion.identity); //, GameObject.Find("HUD").transform);

            bar.HpBar = HP_Base.GetComponentInChildren<UISprite>();
            animator.HpSprite = bar.HpBar;
            animator.CountText = HP_Base.GetComponentInChildren<UILabel>();
        }

        private void UpdateHP()
        {
            if(side!=Faction.Neutral)
            {
                maxHP += CurrentCountUnit * UnitPrefabs.maxHP;
                currentHP += CurrentCountUnit * UnitPrefabs.maxHP;
            }
            else
            {
                maxHP += CurrentCountUnit * 3.0f;
                currentHP += CurrentCountUnit * 3.0f;
            }
            bar.OnHPCahnged(currentHP);
        }

        private void CreateFromPrototype()
        {
            BasePrototype proto = SaveDataManager.instance.GetBasePrototype(side);
            if (proto != null)
            {
                maxHP = proto.maxHP;
                currentHP = maxHP;
                spawnRate = proto.SpawnRate;
                ProduceRate = proto.ProduceUnitRate;
                MaxCountUnit = proto.MaxUnit;

                attack.damage = proto.Damage;
                attack.damageRate = proto.DamageRate;
                attack.attackRadius = proto.AttackRadius;
                CurrentCountUnit = proto.StartCountUnit;
            }
            UpdateHP();
        }

        public void StartSpawnUnit(Transform currentTargetForSpawnUnits)
        {
            if (UnitTarget == currentTargetForSpawnUnits) //если юниты уже спавняться в этом направлении
            {
                if (!isSpawn) // и спавн не  идет
                { //начать спавн
                    spawnCoroutine = StartCoroutine(SpawnUnit(currentTargetForSpawnUnits));
                }
                else
                {
                    SpawnCountUnit += CurrentCountUnit - SpawnCountUnit;
                }
                
            }
            else //иначе другое направление
            {
                if (!isSpawn)
                {
                    spawnCoroutine = StartCoroutine(SpawnUnit(currentTargetForSpawnUnits));
                }
                else
                {
                    if (spawnCoroutine != null)
                    {
                        StopCoroutine(spawnCoroutine);
                    }
                    spawnCoroutine = StartCoroutine(SpawnUnit(currentTargetForSpawnUnits));
                }
            }
        }

        private IEnumerator SpawnUnit(Transform target)
        {
            UnitTarget = target;
            SpawnCountUnit += CurrentCountUnit - SpawnCountUnit;
            if (SpawnCountUnit > 0)
            {
                isSpawn = true;
                positionSpawn = (ObjectTransform.position - UnitTarget.position).normalized * SoftRadius * -1 +
                                ObjectTransform.position;
                while (SpawnCountUnit > 0 && side != Faction.Neutral)
                {
                    SpawnCountUnit--;
                    CurrentCountUnit--;
                    maxHP -= UnitPrefabs.maxHP;
                    currentHP -= UnitPrefabs.maxHP;
                    bar.OnHPCahnged(currentHP);
                    animator.UpdateCountText();
                    GameObject go = UnitPrefabs.gameObject.Spawn(positionSpawn);
                    go.GetComponent<Unit>().MoveInBase(UnitTarget);
                    yield return new WaitForSeconds(spawnRate);
                }
                isSpawn = false;
            }
        }

        #region kostyl for AI 

        private IEnumerator SpawnUnit()
        {
            if (CurrentCountUnit > 0)
            {
                isSpawn = true;
                animator.UpdateCountText();

                while (CurrentCountUnit > 0 && side != Faction.Neutral)
                {
                    CurrentCountUnit--;
                    maxHP -= UnitPrefabs.maxHP;
                    currentHP -= UnitPrefabs.maxHP;
                    bar.OnHPCahnged(currentHP);
                    animator.UpdateCountText();
                    UnitPrefabs.Spawn(positionSpawn);
                    yield return new WaitForSeconds(spawnRate);
                }
                isSpawn = false;
            }
        }
        public void StartSpawnUnit()
        {
            if (!isSpawn)
            {
                spawnCoroutine = StartCoroutine(SpawnUnit());
            }
            else
            {
                if (spawnCoroutine != null)
                {
                    StopCoroutine(spawnCoroutine);
                }
                spawnCoroutine = StartCoroutine(SpawnUnit());
            }
        }
        #endregion

        private IEnumerator ProduceUnit()
        {
            while (true)
            {
                if (side != Faction.Neutral)
                {
                    while (CurrentCountUnit < MaxCountUnit)
                    {
                        CurrentCountUnit += 1;
                        currentHP += UnitPrefabs.maxHP;
                        maxHP += UnitPrefabs.maxHP;
                        animator.UpdateCountText();
                        bar.OnHPCahnged(currentHP);
                        yield return new WaitForSeconds(ProduceRate);
                    }

                    if (side == Faction.Enemy1) //временная заглушка, чтобы враг пулялся юнитами, когда база полностью заполнена, для ИИ
                    {
                        StartSpawnUnit();
                    }
                }
                yield return null;
            }
        }

        public override void OnDead()
        {
            animator.DestroyAnimation();
            side = Faction.Neutral;
            level = 1;
            CaptureBase();
            if (AudioManager.instance != null)
            {
                AIManager.instance.BaseCapture();
                AudioManager.instance.BaseDestroyedPlay();
            }
        }

        public override void Attack(RTSObject enemy)
        {
            attack.AttackEnemy(enemy);
        }

        public override void EnemyOut()
        {
            attack.Enemy = null;
        }

        public void LevelUpBase()
        {
            if (CurrentCountUnit == MaxCountUnit)
            {
                MaxCountUnit += 10;
                CurrentCountUnit = 0;
                currentHP = maxHP;
                bar.OnHPCahnged(currentHP);
                level++;
                animator.UpdateCountText();
            }
        }

        public void CaptureBase()
        {
            if (spawnCoroutine != null)
                StopCoroutine(spawnCoroutine);
            List<Unit> captureUnits = ColliderManager.instance.ReturnUnitInRadius(ObjectTransform.position, CaptureRadius);

            int PlayerUnits = 0;
            int EnemyUnits = 0;

            for (int i = 0; i < captureUnits.Count; i++)
            {
                if (captureUnits[i].side == Faction.Enemy1)
                    EnemyUnits++;
                if (captureUnits[i].side == Faction.Player)
                    PlayerUnits++;
            }

            if (PlayerUnits > EnemyUnits)
            {
                side = Faction.Player;
                CurrentCountUnit = PlayerUnits;

                UnitPrefabs = SideUnitPrefabs();
            }

            if (EnemyUnits > PlayerUnits)
            {
                side = Faction.Enemy1;
                CurrentCountUnit = EnemyUnits;

                UnitPrefabs = SideUnitPrefabs();
            }

            if (PlayerUnits == EnemyUnits && !isRuinded)
            {
                side = Faction.Neutral;
                isRuinded = true;
                StartCoroutine(BaseCapture());
            }
            else
            {
                for (int i = 0; i < captureUnits.Count; i++)
                {
                    if (captureUnits[i].side == side)
                        captureUnits[i].Recycle();
                }
                CreateFromPrototype();
                bar.OnHPCahnged(maxHP);
            }

            animator.UpdateCountText();
            animator.CaptureAnimation();
        }

        public override void Heal(float healPoints)
        {
            base.Heal(healPoints);
            bar.OnHPCahnged(currentHP);
        }

        private Unit SideUnitPrefabs()
        {
            if (side != Faction.Neutral)
            {
                for (int i = 0; i < ColliderManager.instance.Units.Length; i++)
                {
                    if (ColliderManager.instance.Units[i].GetComponent<Unit>().side == side)
                    {
                        return ColliderManager.instance.Units[i].GetComponent<Unit>();
                    }
                }
            }
            return null;
        }

        private IEnumerator BaseCapture()
        {
            while (side == Faction.Neutral)
            {
                CaptureBase();
                yield return new WaitForSecondsRealtime(0.5f);
            }

            isRuinded = false;
        }

        public void UnitComeInBase()
        {
            CurrentCountUnit++;
            currentHP += UnitPrefabs.maxHP;
            maxHP += UnitPrefabs.maxHP;
            bar.OnHPCahnged(currentHP);
            animator.UpdateCountText();
        }

        private IEnumerator FromPrototype()
        {
            while(!SaveDataManager.instance.IsDowloand)
            {
                yield return null;
            }
            CreateFromPrototype();
        }

        public void OnDrawGizmos()
		{
            if (DrawGizmos)
            {
                Gizmos.color = Color.magenta;
                Gizmos.DrawWireSphere(transform.position, SoftRadius);
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(transform.position, HardRadius);
	            Gizmos.color = Color.blue;
	            Gizmos.DrawWireSphere(transform.position, CaptureRadius);
            }
		}
    }
}
