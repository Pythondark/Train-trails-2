using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class SimpleShoot : MonoBehaviour
    {

        public GameObject bulletPrefab;
        public GameObject casingPrefab;
        public GameObject muzzleFlashPrefab;
        public Transform barrelLocation;
        public Transform casingExitLocation;


        public float shotPower = 100f;
        public float shotDamage = 100f;

        void Start()
        {
            if (barrelLocation == null)
                barrelLocation = transform;
        }


        public void TriggerShoot()
        {
            GetComponent<Animator>().SetTrigger("Fire");
        }

        void Shoot()
        {
            //  GameObject bullet;
            //  bullet = Instantiate(bulletPrefab, barrelLocation.position, barrelLocation.rotation);
            // bullet.GetComponent<Rigidbody>().AddForce(barrelLocation.forward * shotPower);


            //bullet = Instantiate(bulletPrefab, barrelLocation.position, barrelLocation.rotation).GetComponent<Rigidbody>().AddForce(barrelLocation.forward * shotPower);
            //Destroy(bullet, 5.0);

            GameObject bullet;
            bullet = Instantiate(bulletPrefab, barrelLocation.position, barrelLocation.rotation) as GameObject;
            bullet.GetComponent<Rigidbody>().AddForce(barrelLocation.forward * shotPower);
            bullet.GetComponent<Projectile>().SetProjectileDamage(shotDamage);
            Destroy(bullet, 5f);

            GameObject tempFlash;
            tempFlash = Instantiate(muzzleFlashPrefab, barrelLocation.position, barrelLocation.rotation);

            // Destroy(tempFlash, 0.5f);
            //  Instantiate(casingPrefab, casingExitLocation.position, casingExitLocation.rotation).GetComponent<Rigidbody>().AddForce(casingExitLocation.right * 100f);

            GetComponent<AudioSource>().Play();

        }

        void CasingRelease()
        {
            GameObject casing;
            casing = Instantiate(casingPrefab, casingExitLocation.position, casingExitLocation.rotation) as GameObject;
            casing.GetComponent<Rigidbody>().AddExplosionForce(550f, (casingExitLocation.position - casingExitLocation.right * 0.3f - casingExitLocation.up * 0.6f), 1f);
            casing.GetComponent<Rigidbody>().AddTorque(new Vector3(0, Random.Range(100f, 500f), Random.Range(10f, 1000f)), ForceMode.Impulse);
        }


    }
}

