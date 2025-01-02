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
                // Fetch the player level dynamically each loop
                playerLevel = Mathf.Clamp(playerInteractions.playerLevel, 1, 3);
                Debug.Log($"Player level: {playerLevel}");

                // Spawn settings based on player level
                float spawnRate = GetSpawnRate(playerLevel);

                // Spawn a creature
                yield return SpawnLevel(spawnRate);
            }
            else
            {
                Debug.LogError("PlayerInteractions is null.");
                yield return null; // Wait until playerInteractions is not null
            }
        }
    }

    private IEnumerator SpawnLevel(float spawnRate)
    {
        // Randomly select a spawner
        Transform spawner = GetRandomSpawner();
        Debug.Log($"Selected spawner: {spawner.name}");

        // Randomly select a creature index using level-based probabilities
        int randomIndex = GetCreatureIndexWithChance(playerLevel);
        Debug.Log($"Spawning creature at index: {randomIndex}");

        // Spawn the selected creature prefab
        GameObject creatureToSpawn = creatures[randomIndex];
        if (creatureToSpawn == null)
        {
            Debug.LogError($"Creature prefab at index {randomIndex} is null!");
        }
        else
        {
            Debug.Log($"Spawning creature: {creatureToSpawn.name}");
            Instantiate(creatureToSpawn, spawner.position, Quaternion.identity);
        }

        // Wait for the spawn rate
        yield return new WaitForSeconds(spawnRate);
    }

    private Transform GetRandomSpawner()
    {
        return spawners[Random.Range(1, spawners.Length)];
    }

    private float GetSpawnRate(int level)
    {
        switch (level)
        {
            case 1: return 0.5f; // Level 1: Spawn every 0.5 seconds
            case 2: return 0.25f; // Level 2: Spawn every 0.25 seconds
            case 3: return 2f; // Level 3: Spawn every 2 seconds
            default: return 1f; // Default spawn rate
        }
    }

    private int GetCreatureIndexWithChance(int level)
    {
        int randomValue = Random.Range(1, 101); // Random value between 1 and 100

        switch (level)
        {
            case 1:
                // Level 1: Ant = 100%, Ladybug = 0%
                return 0; // Always spawn ant
            case 2:
                // Level 2: Ant = 60%, Ladybug = 40%
                if (randomValue <= 80)
                    return 0; // Spawn ant
                else
                    return 1; // Spawn ladybug
            case 3:
                // Level 3: Ant = 30%, Ladybug = 70%
                if (randomValue <= 50)
                    return 0; // Spawn ant
                else
                    return 1; // Spawn ladybug
            default:
                return 0; // Default to ant
        }
    }
}
