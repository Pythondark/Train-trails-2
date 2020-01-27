using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace RPG.UI
{
    public class CharacterUIManager : MonoBehaviour
    {
        [SerializeField] Dialog dialogue;
        [SerializeField] TextMeshProUGUI dialogBox;
        [SerializeField] TextMeshProUGUI descriptionField;
        [SerializeField] TextMeshProUGUI nameField;
        [SerializeField] bool characterUIEnabled = false;
        [SerializeField] Canvas dialogCanvas;
        [SerializeField] float talkDistance = 2f;
        [SerializeField] bool talkDistanceGizmo = true;

        private Queue<string> sentences;

        private bool conversationIsHappening = false;
        private bool conversationEnded = false;

        GameObject player;

        // Start is called before the first frame update
        void Start()
        {
            sentences = new Queue<string>();
            player = GameObject.FindGameObjectWithTag("Player");
        }

        private void Update()
        {
            if (InTalkDistanceOfPlayer() && characterUIEnabled)
            {
                dialogCanvas.enabled = true;
                if (!conversationIsHappening)
                {
                    StartDialogue(dialogue);
                    conversationIsHappening = true;
                }
            }
            else
            {
                dialogCanvas.enabled = false;
            }

             
        }


        public void StartDialogue(Dialog dialogue)
        {
            Debug.Log("Starting conversation with " + dialogue.name);
            descriptionField.text = dialogue.GetDescription();
            nameField.text = dialogue.GetCharName();

            sentences.Clear();

            foreach (string sentence in dialogue.GetSentences())
            {
                sentences.Enqueue(sentence);
            }

            DisplayNextSentence();
        }

        public void DisplayNextSentence()
        {
            if (sentences.Count == 0)
            {
                EndDialogue();
                return;
            }

            string sentence = sentences.Dequeue();
            dialogBox.text = sentence;
        }

        private void EndDialogue()
        {
            dialogBox.text = "End of conversation.";
            conversationIsHappening = false;
        }

        private bool InTalkDistanceOfPlayer()
        {
            return Vector3.Distance(transform.position, player.transform.position) < talkDistance;
        }

        private void OnDrawGizmosSelected()
        {
            if (talkDistanceGizmo)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawWireSphere(transform.position, talkDistance);
            }
        }
    }
}

