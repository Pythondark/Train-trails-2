using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using RPG.Core;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {

        [SerializeField] float timeBetweenAttacks = 5f;
        [SerializeField] Transform rightHandTransform = null;
        [SerializeField] Transform leftHandTransform = null;
        [SerializeField] Weapon equippedWeapon;
        [SerializeField] Health target;
        
        float timeSinceLastAttack = Mathf.Infinity;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            timeSinceLastAttack += Time.deltaTime;

            

            if (target == null) return;
            
            if (target.IsDead()) return;
            

            if (!GetIsInRange())
            {
                Debug.Log("Not in Range");
                GetComponent<Mover>().MoveTo(target.transform.position, 1f);
            }
            else
            {
                GetComponent<Mover>().Cancel();
                AttackBehaviour();
            }
        }

        private void AttackBehaviour()
        {
            // Look at Target
            transform.LookAt(target.transform);
            transform.Rotate(0, -60, 0);

            equippedWeapon.GetComponent<ShootIfGrabbed>().SetTarget(target);

            if (timeSinceLastAttack >= timeBetweenAttacks)
            {
                TriggerAttack();
                timeSinceLastAttack = 0f;
            }
        }

        private void TriggerAttack()
        {
            GetComponent<Animator>().ResetTrigger("stopAttack");
            GetComponent<Animator>().SetTrigger("attack");
        }

        public void Cancel()
        {
            StopAttack();
            target = null;
            GetComponent<Mover>().Cancel();
        }
        
        private void StopAttack()
        {
            GetComponent<Animator>().ResetTrigger("attack");
            GetComponent<Animator>().SetTrigger("stopAttack");
        }
        
        // Called from animation
        public void Attack()
        {
            Debug.Log("Shoot.");
            
            equippedWeapon.GetComponent<ShootIfGrabbed>().ShootGun();
        }

        public void AttackPlayer(GameObject combatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.GetComponent<Health>();
        }

        public bool CanAttack(GameObject combatTarget)
        {
            if (combatTarget == null) { return false; }
            Health targetToTest = combatTarget.GetComponent<Health>();
            return targetToTest != null && !targetToTest.IsDead();
        }

        public void StartAim()
        {
            equippedWeapon.GetComponent<ShootIfGrabbed>().StartAiming();
        }

        public void FinishAim()
        {
            equippedWeapon.GetComponent<ShootIfGrabbed>().StopAiming();
        }

        private bool GetIsInRange()
        {
            return Vector3.Distance(transform.position, target.transform.position) < equippedWeapon.GetRange();
        }

        public Weapon GetEquippedWeapon()
        {
            return equippedWeapon;
        }

    }

}

