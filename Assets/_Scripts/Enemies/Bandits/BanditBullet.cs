using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidAnnihilation
{
    public class BanditBullet : BaseProjectile
    {
        protected override void OnTriggerEnter2D(Collider2D collider)
        {
            PlayerEntity entity = collider.transform.GetComponent<PlayerEntity>();
            if (entity != null && !entity.isDead && entity.canHit)
            {
                entity.KilledByIndex = WeaponIndex;
                entity.OnTakeDamage?.Invoke(Damage, IsCrit);
                objectPooler.SpawnFromPool(OnHitEffectName, transform.position, Quaternion.identity);
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
