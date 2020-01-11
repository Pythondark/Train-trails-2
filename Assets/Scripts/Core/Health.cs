using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{

    public class Health : MonoBehaviour
    {
        [SerializeField] float healthPoints = 100f;

        bool isDead = false;

        public bool IsDead()
        {
            return isDead;
        }

        public void TakeDamage(float damage)
        {
            healthPoints = Mathf.Max(healthPoints - damage, 0);
            if (healthPoints <= 0)
            {
                Die();
            }
            //print(healthPoints);

        }

        private void Die()
        {
            if (isDead) return;

            isDead = true;
            GetComponent<Animator>().SetTrigger("die");
            GetComponent<ActionScheduler>().CancelCurrentAction();
            DisableColliders();
        }

        private void DisableColliders()
        {
            Collider[] colliders = GetComponents<Collider>();
            foreach (Collider collider in colliders)
            {
                collider.enabled = false;
            }
        }

        // === SAVING ===
        // object ISaveable.CaptureState()
        // {
        //     return healthPoints;
        // }


        // void ISaveable.RestoreState(object state)
        // {
        //     healthPoints = (float)state;
        //     if (healthPoints == 0)
        //     {
        //         Die();
        //     }
        // }
    }
}
