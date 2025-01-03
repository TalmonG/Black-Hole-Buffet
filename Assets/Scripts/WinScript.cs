using UnityEngine;

public class WinScript : MonoBehaviour
{
    public GameObject endScreen; 
    private ObjectInteractions playerInteractions;

    void Start()
    {
        // find player
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            // get ObjectInteractions
            playerInteractions = player.GetComponent<ObjectInteractions>();
 
        }

        // hide endScreen on start
        if (endScreen != null)
        {
            endScreen.SetActive(false);
        }
    }

    void Update()
    {
        // check playerInteractions valid and playerSizeCounter 590 or higher
        if (playerInteractions != null && playerInteractions.playerSizeCounter >= 590)
        {
            // pause game
            Time.timeScale = 0;

            // show endScreen
            if (endScreen != null)
            {
                endScreen.SetActive(true);
            }
        }
    }

    // QUIT
    public void QuitGame()
    {
        Debug.Log("Quitting the game...");
        Application.Quit();
    }
}
