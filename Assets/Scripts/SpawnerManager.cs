using System.Collections;
using UnityEngine;

public class SpawnerManager : MonoBehaviour
{
    public GameObject[] creatures;

    private ObjectInteractions playerInteractions;
    private int playerLevel;
    private Transform[] spawners;

    void Start()
    {
        // Get all child spawners
        spawners = GetComponentsInChildren<Transform>();

        // find player and get ObjectInteractions
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerInteractions = player.GetComponent<ObjectInteractions>();

        }
        else
        {
            // Spawn Creatures
            StartCoroutine(SpawnCreatures());
        }
    }

    // Spawn Creatures
    IEnumerator SpawnCreatures()
    {
        while (true)
        {
            if (playerInteractions != null)
            {
                // get player level dynamically each loop
                playerLevel = Mathf.Clamp(playerInteractions.playerLevel, 1, 3);

                // Spawnrate based on player level
                float spawnRate = GetSpawnRate(playerLevel);

                // Spawn a creature
                yield return SpawnLevel(spawnRate);
            }
            else
            {
                yield return null; // Wait until playerInteractions not null
            }
        }
    }

    // spawn at spawners
    private IEnumerator SpawnLevel(float spawnRate)
    {
        // Randomly select spawner
        Transform spawner = GetRandomSpawner();

        // randomly select creature index
        int randomIndex = GetCreatureIndexWithChance(playerLevel);

        // Spawn selected creature
        GameObject creatureToSpawn = creatures[randomIndex];
        if (creatureToSpawn == null)
        {
            //Debug.Log("Creature is null!");
        }
        else
        {
            //Debug.Log("Spawning creature");
            Instantiate(creatureToSpawn, spawner.position, Quaternion.identity);
        }

        // Wait for spawn rate
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
                // Level 2: Ant = 80%, Ladybug = 20%
                if (randomValue <= 80)
                    return 0; // Spawn ant
                else
                    return 1; // Spawn ladybug
            case 3:
                // Level 3: Ant = 50%, Ladybug = 50%
                if (randomValue <= 50)
                    return 0; // Spawn ant
                else
                    return 1; // Spawn ladybug
            default:
                return 0; // Default to ant
        }
    }
}
