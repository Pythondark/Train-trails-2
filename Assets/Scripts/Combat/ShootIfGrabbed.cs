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
        
        if(ovrGrabbable.isGrabbed && OVRInput.GetDown(shootingButton, ovrGrabbable.grabbedBy.GetController()) && maxNumberOfBullet > 0)
        {
            // Shoot!
            // DONT DO THIS:: OVRInput.SetControllerVibration(0.5f, 0.5f, ovrGrabbable.grabbedBy.GetController());
            //vibrationManager.TriggerVibration(shootingAudio, ovrGrabbable.grabbedBy.GetController());


            //OVRInput.SetControllerVibration()

            /* DEPRECIATID? ===
            OVRHapticsClip clip = new OVRHapticsClip(shootingAudio);

            if (ovrGrabbable.grabbedBy.GetController() == OVRInput.Controller.LTouch)
            {
                OVRHaptics.LeftChannel.Preempt(clip);
            }
            else if (ovrGrabbable.grabbedBy.GetController() == OVRInput.Controller.RTouch)
            {
                OVRHaptics.RightChannel.Preempt(clip);
            }
            ==== */

            GetComponent<AudioSource>().PlayOneShot(shootingAudio);            
            GetComponent<ContinuousHaptics>().Play();
            simpleShoot.TriggerShoot();
            maxNumberOfBullet--;
            if (bulletText == null) { return; }
            bulletText.text = maxNumberOfBullet.ToString();
        }
    }
}
