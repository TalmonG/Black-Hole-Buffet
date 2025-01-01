using System.Collections;
using UnityEngine;

public class SpawnerManager : MonoBehaviour
{
    public GameObject[] creatures; // Array of creatures to spawn (drag and drop prefabs in the Inspector)

    private ObjectInteractions playerInteractions; // Reference to the player's script
    private int playerLevel;
    private Transform[] spawners; // Array of child spawners

    void Start()
    {
        // Get all child spawners
        spawners = GetComponentsInChildren<Transform>();

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

        // Start spawning creatures
        StartCoroutine(SpawnCreatures());
    }

    IEnumerator SpawnCreatures()
    {
        while (true)
        {
            if (playerInteractions != null)
            {
                // Update the player level
                playerLevel = Mathf.Clamp(playerInteractions.playerLevel, 1, 5);

                // Spawn settings based on player level
                switch (playerLevel)
                {
                    case 1:
                        yield return SpawnLevel(new int[] { 0 }, 0.5f); // Level 1: Spawn creature at index 0 every 5 seconds
                        break;
                    case 2:
                        yield return SpawnLevel(new int[] { 0 }, 0.25f); // Level 2: Spawn creatures at indices 0 and 1 every 3 seconds
                        break;
                    // case 3:
                    //     yield return SpawnLevel(new int[] { 0, 1 }, 2f); // Level 3: Spawn creatures at indices 0 and 1 every 2 seconds
                    //     break;
                    // case 4:
                    //     yield return SpawnLevel(new int[] { 0, 1, 2 }, 1.5f); // Level 4: Spawn creatures at indices 0, 1, and 2 every 1.5 seconds
                    //     break;
                    // case 5:
                    //     yield return SpawnLevel(new int[] { 0, 1, 2 }, 1f); // Level 5: Spawn creatures at indices 0, 1, and 2 every 1 second
                    //     break;
                }
            }
            else
            {
                yield return null;
            }
        }
    }

    private IEnumerator SpawnLevel(int[] creatureIndices, float spawnRate)
    {
        // Randomly select a spawner
        Transform spawner = GetRandomSpawner();

        // Randomly select a creature index from the level's allowed indices
        int randomIndex = creatureIndices[Random.Range(0, creatureIndices.Length)];

        // Spawn the selected creature prefab
        GameObject creatureToSpawn = creatures[randomIndex];
        Instantiate(creatureToSpawn, spawner.position, Quaternion.identity);

        // Wait for the spawn rate
        yield return new WaitForSeconds(spawnRate);
    }

    private Transform GetRandomSpawner()
    {
        return spawners[Random.Range(0, spawners.Length)];
    }
}
