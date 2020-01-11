using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Core
{
    public class TextManager : MonoBehaviour
    {
        Text text;

        private void Start() 
        {
            text = GetComponent<Text>(); 
        }

        public void SetText(string str)
        {
            text.text = str;
        }
    }
}

