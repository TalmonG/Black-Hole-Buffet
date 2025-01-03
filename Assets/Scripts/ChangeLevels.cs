using UnityEngine;

public class ChangeLevels : MonoBehaviour
{
    public void SetPlayerSize(int newSize)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            ObjectInteractions objectInteractions = player.GetComponent<ObjectInteractions>();

            if (objectInteractions != null)
            {
                objectInteractions.playerSizeCounter = newSize;
            }
        }
    }
}
