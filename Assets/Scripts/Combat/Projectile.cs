using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] float damage = 100f;
        [SerializeField] GameObject hitEffect = null;

        private void OnTriggerEnter(Collider other) 
        {
            Health health = other.GetComponent<Health>();
            if (health == null) { return; }
            health.TakeDamage(damage);

            if (hitEffect != null)
            {
                GameObject hitFX;
                hitFX = Instantiate(hitEffect, gameObject.transform.position, transform.rotation) as GameObject;
                Destroy(hitFX, 2f);
            }

            Destroy(gameObject);
        }

        public void SetProjectileDamage(float damage)
        {
            this.damage = damage;
        }
    }

}
