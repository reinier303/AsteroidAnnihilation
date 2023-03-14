using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidAnnihilation
{
    public class BanditShooterGrunt : BaseEnemy
    {
        [SerializeField] protected float moveSpeed;
        [SerializeField] protected float aggroMove;
        protected float currSpeed;
        [SerializeField] EnumCollections.EnemyProjectiles projectileType;
        [SerializeField] private float projectileDamage;
        [SerializeField] protected float fireRate = 1.5f;
        [SerializeField] private float projectileSize = 1;
        [SerializeField] private float projectileLifeTime = 10f;
        [SerializeField] private float projectileSpeed = 7.5f;

        [SerializeField] private bool randomRotation = true;
        [SerializeField] protected float fireTimer;
        [SerializeField] private bool randomSizeEnabled = true;
        [SerializeField] private float burstFireDelay;
        [SerializeField] private WaitForSeconds burstFireWait;



        private float baseStopDistance;

        protected override void Start()
        {
            base.Start();

            if (randomRotation) { transform.localRotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, Random.Range(0, 360)); }
            float sizeMultiplier = Random.Range(sizeRange.x, sizeRange.y);
            if (randomSizeEnabled)
            {
                Vector2 randomSize = new Vector2(transform.localScale.x * sizeMultiplier, transform.localScale.y * sizeMultiplier);
                transform.localScale = randomSize;
            }
            currSpeed = moveSpeed * Random.Range(0.998f, 1.002f);
            RotationSpeed *= Random.Range(0.998f, 1.002f);
            fireTimer = fireRate;
            baseStopDistance = StopDistance;
            StopDistance = baseStopDistance + Random.Range(-5, 3);

        }

        protected virtual void Update()
        {
            if (spawnManager != null && spawnManager.BossActive)
            {
                MoveAwayFromBoss();
                return;
            }

            CheckAggroDistance();

            if (Aggro)
            {
                AggroMove();
                fireTimer -= Time.deltaTime;
                if (fireTimer <= 0)
                {
                    StartCoroutine(Fire());
                    fireTimer = fireRate;
                }
            }
            else
            {
                IdleMove();
            }
        }


        protected virtual void IdleMove()
        {
            if (Vector2.Distance(transform.position, Player.position) > StopDistance) { transform.position += transform.up * Time.deltaTime * moveSpeed; }
        }

        protected virtual void AggroMove()
        {
            if (Vector2.Distance(transform.position, Player.position) > StopDistance) { transform.position += transform.up * Time.deltaTime * aggroMove; }
            Rotate(rotSpeedMultiplier: 175);
        }

        protected virtual IEnumerator Fire()
        {
            if(burstFireWait == null) { burstFireWait = new WaitForSeconds(burstFireDelay); }
            SwarmProjectile projectile = objectPooler.SpawnFromPool(projectileType.ToString(), transform.position + transform.up + -transform.right * 0.3f, transform.rotation).GetComponent<SwarmProjectile>();
            SwarmProjectile projectile2 = objectPooler.SpawnFromPool(projectileType.ToString(), transform.position + transform.up + transform.right * 0.3f, transform.rotation).GetComponent<SwarmProjectile>();
            projectile.Initialize(projectileSize, projectileDamage, projectileSpeed, projectileLifeTime, false);
            projectile2.Initialize(projectileSize, projectileDamage, projectileSpeed, projectileLifeTime, false);
            yield return burstFireWait;
            SwarmProjectile projectile3 = objectPooler.SpawnFromPool(projectileType.ToString(), transform.position + transform.up + -transform.right * 0.3f, transform.rotation).GetComponent<SwarmProjectile>();
            SwarmProjectile projectile4 = objectPooler.SpawnFromPool(projectileType.ToString(), transform.position + transform.up + transform.right * 0.3f, transform.rotation).GetComponent<SwarmProjectile>();
            projectile3.Initialize(projectileSize, projectileDamage, projectileSpeed, projectileLifeTime, false);
            projectile4.Initialize(projectileSize, projectileDamage, projectileSpeed, projectileLifeTime, false);
        }
    }
}
