using UnityEngine;

public class ChangeLevels : MonoBehaviour
{
    // Method to set the playerSizeCounter in ObjectInteractions
    public void SetPlayerSize(int newSize)
    {
        // Find the GameObject with the "Player" tag
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        // Ensure a player with the tag exists
        if (player != null)
        {
            // Get the ObjectInteractions script from the player
            ObjectInteractions objectInteractions = player.GetComponent<ObjectInteractions>();

            // Ensure the ObjectInteractions script exists on the player
            if (objectInteractions != null)
            {
                // Set the playerSizeCounter variable
                objectInteractions.playerSizeCounter = newSize;
            }
            else
            {
                Debug.LogError("ObjectInteractions script not found on Player.");
            }
        }
        else
        {
            Debug.LogError("No GameObject found with the 'Player' tag.");
        }
    }
}
