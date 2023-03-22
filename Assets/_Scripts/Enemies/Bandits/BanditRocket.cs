using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

namespace AsteroidAnnihilation
{
    public class BanditRocket : BaseProjectile
    {
        private CameraManager cameraManager;
        [SerializeField] private float ShakeMagnitude;
        [SerializeField] private float ShakeDuration;
        [SerializeField] private float rotationSpeed;

        private Transform player;

        protected override void Start()
        {
            base.Start();
            cameraManager = gameManager.RCameraManager;
            player = gameManager.RPlayer.transform;
        }

        protected override void Update()
        {
            base.Update();
            Rotate();
        }

        protected override void OnTriggerEnter2D(Collider2D collider)
        {
            PlayerEntity entity = collider.transform.GetComponent<PlayerEntity>();
            if (entity != null && !entity.isDead && entity.canHit)
            {
                entity.KilledByIndex = WeaponIndex;
                entity.OnTakeDamage?.Invoke(Damage, IsCrit);
                cameraManager.StartCoroutine(cameraManager.Shake(ShakeDuration, ShakeMagnitude));
                objectPooler.SpawnFromPool(OnHitEffectName, transform.position, Quaternion.identity);
                Explode();
                audioManager.PlayAudio("");
                gameObject.SetActive(false);
            }
            else if (collider.gameObject.layer == 18)
            {
                GameObject hitEffect = objectPooler.SpawnFromPool(OnHitEffectName, transform.position, Quaternion.identity);
                hitEffect.transform.localScale = new Vector2(Size, Size);
                Explode();
                audioManager.PlayAudio("");
                gameObject.SetActive(false);
            }
        }

        public virtual void Explode()
        {
            Explosion_DamagingGeneric explosion = objectPooler.SpawnFromPool("Explosion_DamagePlayer", transform.position, Quaternion.identity).GetComponent<Explosion_DamagingGeneric>();
            explosion.Initialize(Damage, Size, WeaponIndex, IsCrit, ShakeMagnitude, ShakeDuration);
        }

        protected virtual void Rotate(Vector3 target = default(Vector3), float rotSpeedMultiplier = 1)
        {
            Vector3 difference;
            if (target == default(Vector3)) { difference = player.position - transform.position; }
            else { difference = target - transform.position; }
            float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
            Quaternion desiredRotation = Quaternion.Euler(0.0f, 0.0f, rotationZ - 90f);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, desiredRotation, rotationSpeed * rotSpeedMultiplier * Time.deltaTime * 60);
            if (!gameManager.PlayerAlive)
            {
                transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ + 90f);
            }
        }
    }
}
