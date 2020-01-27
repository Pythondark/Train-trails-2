using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RPG.Combat;
using RPG.Core;

public class ShootIfGrabbed : MonoBehaviour
{

    private SimpleShoot simpleShoot;
    private OVRGrabbable ovrGrabbable;
    public OVRInput.Button shootingButton;

    [SerializeField] int maxNumberOfBullet = 30;
    [SerializeField] Text bulletText;
    [SerializeField] AudioClip shootingAudio;

    private bool aimingGun = false;
    Health target;

    //VibrationManager vibrationManager;


    // Start is called before the first frame update
    void Start()
    {
        simpleShoot = GetComponent<SimpleShoot>();
        ovrGrabbable = GetComponent<OVRGrabbable>();
        if (bulletText == null) {return;}
        bulletText.text = maxNumberOfBullet.ToString();
        //vibrationManager = FindObjectOfType<VibrationManager>();
    }

    // Update is called once per frame
    void Update()
    {

        if (ovrGrabbable.isGrabbed)
        {            
            bulletText.text = maxNumberOfBullet.ToString();
        } else {
            bulletText.text = "";
        }
        
        if(ovrGrabbable.isGrabbed && OVRInput.GetDown(shootingButton, ovrGrabbable.grabbedBy.GetController()))
        {
            // Shoot!
            ShootGun();
        }

        if (aimingGun) {AimGun();}
    }

    public void ShootGun()
    {
        //if(maxNumberOfBullet > 0)
        //{
            GetComponent<AudioSource>().PlayOneShot(shootingAudio);
            GetComponent<ContinuousHaptics>().Play();
            simpleShoot.TriggerShoot();
            maxNumberOfBullet--;
            if (bulletText == null) { return; }
            bulletText.text = maxNumberOfBullet.ToString();
        //}
        
    }

    public void StartAiming()
    {
        aimingGun = true;
    }

    public void StopAiming()
    {
        aimingGun = false;
    }

    public void SetTarget(Health target)
    {
        this.target = target;
    }

    private void AimGun()
    {
        if (target == null) {return; }
        transform.LookAt(target.GetTargetLocation());
    }
}
