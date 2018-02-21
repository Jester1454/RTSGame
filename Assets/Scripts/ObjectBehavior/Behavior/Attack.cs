using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ObjectBehavior
{
	[Serializable]
    public class Attack : MonoBehaviour
    {
        private bool isAttack = false;

        public bool IsAttack 
        {
            get
		        {
		            return isAttack;
		        }
		        set
		        {
		            isAttack = value;
		        }
	    }

        public float damage;
        public float damageRate;
        public float attackRadius;
        private float timeSinceLastAttack = 0;

	    public CannonBall cannonbal;
        private AttackObject rtsObject;
	    
	    [NonSerialized]
        public RTSObject Enemy;

        private void Start()
        {
            rtsObject = GetComponent<AttackObject>();
        }

        private void Update()
        {
            if(isAttack)
            {
                AttackEnemy();
            }
        }

        public void AttackEnemy(RTSObject enemy)
        {
            isAttack = true;
            Enemy = enemy;
        }

        private void AttackEnemy()
        {
			if (Enemy != null && Enemy.currentHP > 0 && rtsObject.state!=AttackObjectState.isDead)
			{
				if (Enemy.side != rtsObject.side)
				{	

					if (timeSinceLastAttack == 0)
					{
						StartShoot(Enemy);
						if (AudioManager.instance != null)
							AudioManager.instance.CannoballShotPlay();
					}

					timeSinceLastAttack += Time.deltaTime;

					if (timeSinceLastAttack >= damageRate)
					{
						Enemy.Attacked(damage);
						timeSinceLastAttack = 0;
					}
				}
				else
				{
					isAttack = false;
				}
			}
			else
			{
				isAttack = false;
			}
        }

        void StartShoot(RTSObject Enemy)
		{
			Vector3 startPosition = new Vector3(rtsObject.ObjectTransform.position.x + rtsObject.HardRadius / 3, rtsObject.ObjectTransform.position.y + rtsObject.HardRadius / 3, -0.5f); //стрелям с края борта
            CannonBall currentball = cannonbal.Spawn(startPosition);
			Vector3 FinishPosition = new Vector3(Enemy.ObjectTransform.position.x + Random.Range(-Enemy.HardRadius / 2, Enemy.HardRadius / 2),
												 Enemy.ObjectTransform.position.y + Random.Range(-Enemy.HardRadius / 2, Enemy.HardRadius / 2), -0.5f); //стреляем в случайное место на корабле

			Vector2  directionVector = FinishPosition - startPosition;
			float angle = Mathf.Atan2(directionVector.y, directionVector.x) * Mathf.Rad2Deg - 90;
			currentball.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
			
			currentball.Finish = FinishPosition;
		}
    }
}
