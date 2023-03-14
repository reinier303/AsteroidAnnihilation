using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidAnnihilation
{
    public class BanditMine : BaseProjectile
    {
        private CameraManager cameraManager;
        [SerializeField] private float ShakeMagnitude;
        [SerializeField] private float ShakeDuration;

        protected override void Start()
        {
            base.Start();
            cameraManager = gameManager.RCameraManager;
        }

        protected override void OnTriggerEnter2D(Collider2D collider)
        {
            PlayerEntity entity = collider.transform.GetComponent<PlayerEntity>();
            if (entity != null && !entity.isDead && entity.canHit)
            {
                cameraManager.StartCoroutine(cameraManager.Shake(ShakeDuration, ShakeMagnitude));
                objectPooler.SpawnFromPool(OnHitEffectName, transform.position, Quaternion.identity);
                Explosion_DamagingGeneric explosion = objectPooler.SpawnFromPool("Explosion_DamagePlayer", transform.position, Quaternion.identity).GetComponent<Explosion_DamagingGeneric>();
                explosion.Initialize(Damage, Size, WeaponIndex, IsCrit, ShakeMagnitude, ShakeDuration);
                audioManager.PlayAudio("");
                gameObject.SetActive(false);
            }
            else if (collider.gameObject.layer == 18)
            {
                GameObject hitEffect = objectPooler.SpawnFromPool(OnHitEffectName, transform.position, Quaternion.identity);
                hitEffect.transform.localScale = new Vector2(Size, Size);
                audioManager.PlayAudio("");
                gameObject.SetActive(false);
            }
        }
    }
}
