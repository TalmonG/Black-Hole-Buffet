using UnityEngine;

public class WinScript : MonoBehaviour
{
    public GameObject endScreen; // Reference to the EndScreen GameObject
    private ObjectInteractions playerInteractions; // Reference to the player's script

    void Start()
    {
        // Find the player and get the ObjectInteractions script
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerInteractions = player.GetComponent<ObjectInteractions>();
            if (playerInteractions == null)
            {
                Debug.LogError("ObjectInteractions script not found on Player.");
            }
        }
        else
        {
            Debug.LogError("Player not found in the scene.");
        }

        // Ensure the EndScreen is initially hidden
        if (endScreen != null)
        {
            endScreen.SetActive(false);
        }
        else
        {
            Debug.LogError("EndScreen GameObject not assigned.");
        }
    }

    void Update()
    {
        // Check if playerInteractions is valid and playerSizeCounter is 200 or higher
        if (playerInteractions != null && playerInteractions.playerSizeCounter >= 600)
        {
            // Pause the game
            Time.timeScale = 0;

            // Show the EndScreen
            if (endScreen != null)
            {
                endScreen.SetActive(true);
            }
        }
    }

    // Method to quit the application
    public void QuitGame()
    {
        Debug.Log("Quitting the game...");
        Application.Quit();
    }
}
