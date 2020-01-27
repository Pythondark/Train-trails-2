using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField] float weaponRange = 12;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public float GetRange()
        {
            return weaponRange;
        }
    }
}

