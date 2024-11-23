using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionHandler : MonoBehaviour
{
    [SerializeField]
    private KeyCode interactionKey = KeyCode.E; // Key to interact  
    private bool isOpened = false; // State of the door  
    private bool isPlayerInRange = false; // Check if the player is within interaction range  
    private Transform player; // Player's position  

    [Header("Audio Settings")]
    [SerializeField]
    private AudioClip openSound; // Sound played when opening  
    [SerializeField]
    private AudioClip closeSound; // Sound played when closing  
    private AudioSource audioSource; // Audio source for playing sounds  

    private void Start()
    {
        // Initialize the audio source  
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        // Check if the colliding object is the player  
        if (collision.gameObject.tag=="Player")
        {
            Debug.Log("On collision with player");
            player = collision.transform; // Store the player's transform  
            isPlayerInRange = true; // Player is within interaction range  
            ShowInteractionPrompt(); // Show interaction prompt  
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        // Check if the colliding object is the player  
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("On collision exit with player");
            isPlayerInRange = false; // Player is out of range  
            HideInteractionPrompt(); // Hide interaction prompt  
            player = null; // Clear player reference  
        }
    }

    private void Update()
    {
        // Check if the player is in range and the interaction key is pressed  
        if (isPlayerInRange && player != null && Input.GetKeyDown(interactionKey))
        {
            Debug.Log("Interact with door");
            Interact();
        }
    }

    private void Interact()
    {
        // Toggle door state when interacting  
        if (!isOpened)
        {
            isOpened = true;
            Debug.Log("Opening " + name);
            PlaySound(openSound); // Play open sound  
            OpenAction();
        }
        else
        {
            isOpened = false;
            Debug.Log("Closing " + name);
            PlaySound(closeSound); // Play close sound  
            CloseAction();
        }
    }

    private void OpenAction()
    {
        // Rotate the door to open it  
        transform.rotation = Quaternion.Euler(
            transform.rotation.eulerAngles.x,
            transform.rotation.eulerAngles.y,
            transform.rotation.eulerAngles.z + 90
        );
    }

    private void CloseAction()
    {
        // Rotate the door to close it  
        transform.rotation = Quaternion.Euler(
            transform.rotation.eulerAngles.x,
            transform.rotation.eulerAngles.y,
            transform.rotation.eulerAngles.z - 90
        );
    }

    private void ShowInteractionPrompt()
    {
        // Show interaction prompt in the UI or via Debug  
        Debug.Log("Press " + interactionKey + " to interact with " + name);
    }

    private void HideInteractionPrompt()
    {
        // Hide interaction prompt  
        Debug.Log("Out of range: " + name);
    }

    private void PlaySound(AudioClip clip)
    {
        // Play the specified audio clip if available  
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}