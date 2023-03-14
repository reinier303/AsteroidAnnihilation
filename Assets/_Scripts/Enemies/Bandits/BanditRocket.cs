using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidAnnihilation
{
    public class BanditRocket : BaseProjectile
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
                entity.KilledByIndex = WeaponIndex;
                entity.OnTakeDamage?.Invoke(Damage, IsCrit);
                cameraManager.StartCoroutine(cameraManager.Shake(ShakeDuration, ShakeMagnitude));
                objectPooler.SpawnFromPool(OnHitEffectName, transform.position, Quaternion.identity);
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
