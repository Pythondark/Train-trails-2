using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.UI
{
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Character Dialog", order = 1)]
    public class Dialog : ScriptableObject
    {
        [SerializeField] string charName;
        [SerializeField] string description;

        [TextArea(3, 10)]
        [SerializeField] string[] sentences;

        public string GetCharName()
        {
            return charName;
        }

        public string GetDescription()
        {
            return description;
        }

        public string[] GetSentences()
        {
            return sentences;
        }
    }
}

