using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidAnnihilation
{
    public class BulletProjectile : BaseProjectile
    {
        [SerializeField] private GameObject secondaryEffect;
        [SerializeField] private Sprite primarySprite, secondarySprite;
        private SpriteRenderer spriteRenderer;

        protected override void Awake()
        {
            base.Awake();
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public override void Initialize(float size, float damage, float speed, float lifeTime, bool isCrit, bool secondary = false)
        {
            base.Initialize(size, damage, speed, lifeTime, isCrit, secondary);
            if (secondary) { spriteRenderer.sprite = secondarySprite; } else { spriteRenderer.sprite = primarySprite; }
        }
    }
}
