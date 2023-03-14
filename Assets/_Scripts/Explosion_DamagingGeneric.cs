using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidAnnihilation
{
    public class Explosion_DamagingGeneric : MonoBehaviour
    {
        protected ObjectPooler objectPooler;
        protected GameManager gameManager;
        protected AudioManager audioManager;
        [HideInInspector] public int WeaponIndex;

        public float Damage;
        public bool IsCrit;

        public Vector2 PlayerVelocity;

        private CameraManager cameraManager;
        [SerializeField] private float ShakeMagnitude;
        [SerializeField] private float ShakeDuration;

        private Collider2D collider;
        private WaitForSeconds wait;
        public bool DamagePlayer;
        private bool canDamage;

        private void Awake()
        {
            collider = GetComponent<Collider2D>();
            collider.enabled = false;
            wait = new WaitForSeconds(0.2f);
        }

        public void Initialize(float damage, float size, int weaponIndex, bool isCrit, float shakeMagnitude, float shakeDuration)
        {
            Damage = damage;
            transform.localScale = new Vector2(size, size);
            WeaponIndex = weaponIndex;
            IsCrit = isCrit;
            ShakeMagnitude = shakeMagnitude;
            ShakeDuration = shakeDuration;
            StartCoroutine(DamageZone());
        }

        private IEnumerator DamageZone()
        {
            collider.enabled = true;
            yield return wait;
            collider.enabled = false;
        }

        protected virtual void OnTriggerEnter2D(Collider2D collider)
        {
            if(DamagePlayer)
            {
                PlayerEntity entity = collider.transform.GetComponent<PlayerEntity>();
                if (entity != null && !entity.isDead && entity.canHit)
                {
                    entity.KilledByIndex = WeaponIndex;
                    entity.OnTakeDamage?.Invoke(Damage, IsCrit);
                    cameraManager.StartCoroutine(cameraManager.Shake(ShakeDuration, ShakeMagnitude));
                    audioManager.PlayAudio("");
                    gameObject.SetActive(false);
                }
                else if (collider.gameObject.layer == 18)
                {
                    audioManager.PlayAudio("");
                    gameObject.SetActive(false);
                }
            }
            else
            {
                BaseEntity entity = collider.GetComponent<BaseEntity>();
                if (canDamage && entity != null && !entity.isDead)
                {
                    canDamage = false;
                    entity.KilledByIndex = WeaponIndex;
                    entity.OnTakeDamage?.Invoke(Damage, IsCrit);
                    entity.Aggro = true;
                    audioManager.PlayAudio("");
                    gameObject.SetActive(false);
                    //Environment Hit
                }
                else if (collider.gameObject.layer == 18)
                {
                    audioManager.PlayAudio("");
                    gameObject.SetActive(false);
                }
            }

        }
    }
}
