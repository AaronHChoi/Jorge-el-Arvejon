using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private float typingSpeed = 0.05f;

    [SerializeField] private bool nPCSpeakingFirst;
    [Header("Dialogue TMP text")]
    [SerializeField] private TextMeshProUGUI playerDialogueText;
    [SerializeField] private TextMeshProUGUI nPCDialogueText;

    [Header("Continue Button")]
    [SerializeField] private GameObject playerContinueButton;
    [SerializeField] private GameObject nPCContinueButton;

    [Header("Animation Controller")]
    [SerializeField] private Animator playerSpeechBubbleAnimator;
    [SerializeField] private Animator nPCSpeechBubbleAnimator;

    [Header("Dialogue Sentences")]
    [TextArea]
    [SerializeField] private string[] playerDialogueSentences; 
    [TextArea]
    [SerializeField] private string[] nPCDialogueSentences;

    private bool dialogueStarted;

    private int playerIndex;
    private int nPCIndex;

    private float speechBubbleAnimationDelay = 0.5f;

    private PlayerMovement playerMovement;

    private void Start()
    {
        playerMovement = FindAnyObjectByType<PlayerMovement>();
        StartCoroutine(StartDialogue());
    }

    private void Update()
    {
        if (playerContinueButton.activeSelf)
        {
            if(Input.GetKeyDown(KeyCode.E)) 
            {
                TriggerContinueNPCDialogue();
            }
        }

        if (nPCContinueButton.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                TriggerContinuePlayerDialogue();
            }
        }
    }

    private IEnumerator StartDialogue()
    {

        playerMovement.ToggleInteraction();

        if (nPCSpeakingFirst)
        {
            nPCSpeechBubbleAnimator.SetTrigger("Open");

            yield return new WaitForSeconds(speechBubbleAnimationDelay);

            StartCoroutine(TypeNPCDialogue());
        }
        else 
        {
            playerSpeechBubbleAnimator.SetTrigger("Open");

            yield return new WaitForSeconds(speechBubbleAnimationDelay);

            StartCoroutine(TypePlayerDialogue());
        }
    }

    // Update is called once per frame
    private IEnumerator TypePlayerDialogue()
    {
        foreach (char letter in playerDialogueSentences[playerIndex].ToCharArray())
        {
            playerDialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        playerContinueButton.SetActive(true);
    }

    private IEnumerator TypeNPCDialogue()
    {
        foreach (char letter in nPCDialogueSentences[nPCIndex].ToCharArray())
        {
            nPCDialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);

            nPCContinueButton.SetActive(true);
        }
    }

    private IEnumerator ContinuePlayerDialogue()
    {
        nPCDialogueText.text = string.Empty;

        nPCSpeechBubbleAnimator.SetTrigger("Close");

        yield return new WaitForSeconds(speechBubbleAnimationDelay);

        playerDialogueText.text =string.Empty;

        playerSpeechBubbleAnimator.SetTrigger("Open");

        yield return new WaitForSeconds(speechBubbleAnimationDelay);

        
            if (dialogueStarted)
            {
                playerIndex++;
            }
            else
            {
                dialogueStarted = true;
            }

            
            StartCoroutine(TypePlayerDialogue());

    }
    private IEnumerator ContinueNPCDialogue()
    {
        playerDialogueText.text = string.Empty;

        playerSpeechBubbleAnimator.SetTrigger("Close");

        yield return new WaitForSeconds(speechBubbleAnimationDelay);

        playerDialogueText.text = string.Empty;

        nPCSpeechBubbleAnimator.SetTrigger("Open");

        yield return new WaitForSeconds(speechBubbleAnimationDelay);

        
            if (dialogueStarted)
            {
                playerIndex++;
            }
            else
            {
                dialogueStarted = true;
            }

            nPCIndex++;

            
            StartCoroutine(TypeNPCDialogue());
        
    }

    public void TriggerContinuePlayerDialogue()
    {
        nPCContinueButton.SetActive(false);

        if (playerIndex >= playerDialogueSentences.Length - 1) 
        {
            nPCDialogueText.text = string.Empty;

            nPCSpeechBubbleAnimator.SetTrigger("Close");

            playerMovement.ToggleInteraction();
        }
        else
        {
            StartCoroutine(ContinuePlayerDialogue());
        }
    }

    public void TriggerContinueNPCDialogue()
    {
        playerContinueButton.SetActive(false);

        if (nPCIndex >= nPCDialogueSentences.Length - 1)
        {
            playerDialogueText.text = string.Empty;

            playerSpeechBubbleAnimator.SetTrigger("Close");

            playerMovement.ToggleInteraction();
        }
        else
        {
            StartCoroutine(ContinueNPCDialogue());
        }
    }
}
