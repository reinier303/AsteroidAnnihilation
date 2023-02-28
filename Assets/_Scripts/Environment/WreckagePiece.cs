using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidAnnihilation
{
    public class WreckagePiece : BaseEnemy
    {
        private Environment wreckage;

        protected override void Awake()
        {
            base.Awake();
            wreckage = transform.parent.GetComponent<Environment>();
        }

        protected override void Die()
        {
            wreckage.RemoveActivePiece(gameObject);
            base.Die();
        }
    }
}
