using UnityEngine;
using TMPro;


public class ObjectInteractions : MonoBehaviour
{
    public GameObject player;
    public TextMeshProUGUI text;  // Reference to the UI TextMeshProUGUI component
    public float playerSize = 0.2f;
    public float playerSizeCounter = 2f;

    // Variables for the two colliders
    private Collider2D mainCollider;
    private Collider2D triggerCollider;

    // Measurements in mm
    private float grainOfSandMeasurement = 1f;
    private float antMeasurement = 2f;
    private float pebbleMeasurement = 5f;
    private float ladybugMeasurement = 9f;
    private float leafMeasurement = 14f;
    private float coinMeasurement = 19f;
    private float smallFLowerMeasurement = 40f;
    private float featherMeasurement = 60f;
    private float snailMeasurement = 85f;
    private float pineconeMeasurement = 110f;
    // Sizes
    private float grainOfSandSize = 0.1f;
    private float antSize = 0.3f;
    private float pebbleSize = 0.5f;
    private float ladybugSize = 0.9f;
    private float leafSize = 1.4f;
    private float coinSize = 1.9f;
    private float smallFLowerSize = 4.0f;
    private float featherSize = 6.0f;
    private float snailSize = 8.5f;
    private float pineconeSize = 11.0f;

    private AudioManager audioManager; // Reference to AudioManager


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject audioManagerObject = GameObject.FindGameObjectWithTag("AudioManager");
        if (audioManagerObject != null)
        {
            audioManager = audioManagerObject.GetComponent<AudioManager>();
        }
        else
        {
            Debug.LogWarning("AudioManager not found in the scene!");
        }

        // Assuming the first collider is the main and the second is the trigger
        Collider2D[] colliders = player.GetComponents<Collider2D>();

        if (colliders.Length > 1)
        {
            // Assigning the colliders - make sure the order matches how they are added to the GameObject
            mainCollider = colliders[0];  // Main Collider (solid)
            triggerCollider = colliders[1];  // Trigger Collider (80% size)
        }

        if (playerSizeCounter >= 1 && playerSizeCounter < 10) // 1cm = 10mm
        {
            text.text = Mathf.RoundToInt(playerSizeCounter) + " Millimeters";
            Debug.Log("TT");
        }
    }
void UpdateText()
{
    if (playerSizeCounter >= 1 && playerSizeCounter < 10) // 1cm = 10mm
    {
        text.text = Mathf.RoundToInt(playerSizeCounter) + " Millimeters";
    }
    else if (playerSizeCounter >= 10 && playerSizeCounter < 1000) // 100cm = 1000mm
    {
        text.text = (playerSizeCounter / 10).ToString("F2") + " Centimeters";
    }
    else if (playerSizeCounter >= 1000 && playerSizeCounter < 100000) // 1m = 100cm = 100000mm
    {
        text.text = (playerSizeCounter / 1000).ToString("F2") + " Meters";
    }
    else if (playerSizeCounter >= 100000)
    {
        text.text = (playerSizeCounter / 100000).ToString("F2") + " Kilometers";
    }
}

    private void OnCollisionEnter2D(Collision2D collision) // Handles Main Collider
    {
        if (collision.gameObject.CompareTag("GrainOfSand"))
        {
            if (playerSize > grainOfSandSize)
            {

                // If player is larger, allow the player to pass through by turning the object into a trigger
                collision.gameObject.GetComponent<Collider2D>().isTrigger = true;
            }
        }
        else if (collision.gameObject.CompareTag("Ant"))
        {
            if (playerSize > antSize)
            {
                
                // If player is larger, allow the player to pass through by turning the object into a trigger
                collision.gameObject.GetComponent<Collider2D>().isTrigger = true;
            }
        }
        else if (collision.gameObject.CompareTag("Pebble"))
        {
            if (playerSize > pebbleSize)
            {
                // If player is larger, allow the player to pass through by turning the object into a trigger
                collision.gameObject.GetComponent<Collider2D>().isTrigger = true;
            }
        }
        else if (collision.gameObject.CompareTag("Ladybug"))
        {
            if (playerSize > ladybugSize)
            {
                // If player is larger, allow the player to pass through by turning the object into a trigger
                collision.gameObject.GetComponent<Collider2D>().isTrigger = true;
            }
        }
        else if (collision.gameObject.CompareTag("Leaf"))
        {
            if (playerSize > leafSize)
            {
                // If player is larger, allow the player to pass through by turning the object into a trigger
                collision.gameObject.GetComponent<Collider2D>().isTrigger = true;
            }
        }
        else if (collision.gameObject.CompareTag("Coin"))
        {
            if (playerSize > coinSize)
            {
                // If player is larger, allow the player to pass through by turning the object into a trigger
                collision.gameObject.GetComponent<Collider2D>().isTrigger = true;
            }
        }
        else if (collision.gameObject.CompareTag("SmallFlower"))
        {
            if (playerSize > smallFLowerSize)
            {
                // If player is larger, allow the player to pass through by turning the object into a trigger
                collision.gameObject.GetComponent<Collider2D>().isTrigger = true;
            }
        }
        else if (collision.gameObject.CompareTag("Feather"))
        {
            if (playerSize > featherSize)
            {
                // If player is larger, allow the player to pass through by turning the object into a trigger
                collision.gameObject.GetComponent<Collider2D>().isTrigger = true;
            }
        }
        else if (collision.gameObject.CompareTag("Snail"))
        {
            if (playerSize > snailSize)
            {
                // If player is larger, allow the player to pass through by turning the object into a trigger
                collision.gameObject.GetComponent<Collider2D>().isTrigger = true;
            }
        }
        else if (collision.gameObject.CompareTag("Pinecone"))
        {
            if (playerSize > pineconeSize)
            {
                // If player is larger, allow the player to pass through by turning the object into a trigger
                collision.gameObject.GetComponent<Collider2D>().isTrigger = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) // This is for the trigger collider // Actual eating
    {
        if (collision.gameObject.CompareTag("GrainOfSand"))
        {
            if (playerSize > grainOfSandSize)
            {
                audioManager.Play("Collect");
                float sizeMultiplier = grainOfSandSize / 100;
                // Increase player size and destroy the grain of sand
                playerSizeCounter += grainOfSandMeasurement;
                Debug.Log(playerSizeCounter + grainOfSandMeasurement);
                playerSize += grainOfSandSize / 10;
                player.transform.localScale += new Vector3(grainOfSandSize * sizeMultiplier, grainOfSandSize * sizeMultiplier, player.transform.localScale.z);
                Destroy(collision.gameObject);
            }
        }
        else if (collision.gameObject.CompareTag("Ant"))
        {
            if (playerSize > antSize)
            {
                audioManager.Play("Collect");
                float sizeMultiplier = playerSize + (antSize / 100);
                // Increase player size and destroy the ant
                playerSizeCounter += antMeasurement;
                playerSize += antSize / 10;
                player.transform.localScale += new Vector3(antSize * sizeMultiplier, antSize * sizeMultiplier, player.transform.localScale.z);
                Destroy(collision.gameObject);
            }
        }
        else if (collision.gameObject.CompareTag("Pebble"))
        {
            if (playerSize > pebbleSize)
            {
                float sizeMultiplier = playerSize + (pebbleSize / 100);

                // Increase player size and destroy the ant
                playerSize += pebbleSize / 10;
                playerSizeCounter += pebbleMeasurement;
                player.transform.localScale += new Vector3(pebbleSize * sizeMultiplier, pebbleSize * sizeMultiplier, player.transform.localScale.z);
                Destroy(collision.gameObject);
            }
        }
        else if (collision.gameObject.CompareTag("Ladybug"))
        {
            if (playerSize > ladybugSize)
            {
                float sizeMultiplier = playerSize + (ladybugSize / 100);

                // Increase player size and destroy the ant
                playerSizeCounter += ladybugMeasurement;
                playerSize += ladybugSize / 10;
                player.transform.localScale += new Vector3(ladybugSize * sizeMultiplier, ladybugSize * sizeMultiplier, player.transform.localScale.z);
                Destroy(collision.gameObject);
            }
        }
        else if (collision.gameObject.CompareTag("Leaf"))
        {
            if (playerSize > leafSize)
            {
                float sizeMultiplier = playerSize + (leafSize / 100);

                // Increase player size and destroy the ant
                playerSizeCounter += leafMeasurement;
                playerSize += leafSize / 10;
                player.transform.localScale += new Vector3(leafSize * sizeMultiplier, leafSize * sizeMultiplier, player.transform.localScale.z);
                Destroy(collision.gameObject);
            }
        }
        else if (collision.gameObject.CompareTag("Coin"))
        {
            if (playerSize > coinSize)
            {
                float sizeMultiplier = playerSize + (coinSize / 100);

                // Increase player size and destroy the ant
                playerSizeCounter += coinMeasurement;
                playerSize += coinSize / 10;
                player.transform.localScale += new Vector3(coinSize * sizeMultiplier, coinSize * sizeMultiplier, player.transform.localScale.z);
                Destroy(collision.gameObject);
            }
        }
        else if (collision.gameObject.CompareTag("SmallFlower"))
        {
            if (playerSize > smallFLowerSize)
            {
                float sizeMultiplier = playerSize + (smallFLowerSize / 100);

                // Increase player size and destroy the ant
                playerSizeCounter += smallFLowerMeasurement;
                playerSize += smallFLowerSize / 10;
                player.transform.localScale += new Vector3(smallFLowerSize * sizeMultiplier, smallFLowerSize * sizeMultiplier, player.transform.localScale.z);
                Destroy(collision.gameObject);
            }
        }
        else if (collision.gameObject.CompareTag("Feather"))
        {
            if (playerSize > featherSize)
            {
                float sizeMultiplier = playerSize + (featherSize / 100);

                // Increase player size and destroy the ant
                playerSizeCounter += featherMeasurement;
                playerSize += featherSize / 10;
                player.transform.localScale += new Vector3(featherSize * sizeMultiplier, featherSize * sizeMultiplier, player.transform.localScale.z);
                Destroy(collision.gameObject);
            }
        }
        else if (collision.gameObject.CompareTag("Snail"))
        {
            if (playerSize > snailSize)
            {
                float sizeMultiplier = playerSize + (snailSize / 100);

                // Increase player size and destroy the ant
                playerSizeCounter += snailMeasurement;
                playerSize += snailSize / 10;
                player.transform.localScale += new Vector3(snailSize * sizeMultiplier, snailSize * sizeMultiplier, player.transform.localScale.z);
                Destroy(collision.gameObject);
            }
        }
        else if (collision.gameObject.CompareTag("Pinecone"))
        {
            if (playerSize > pineconeSize)
            {
                float sizeMultiplier = playerSize + (pineconeSize / 100);

                // Increase player size and destroy the ant
                playerSizeCounter += pineconeMeasurement;
                playerSize += pineconeSize / 10;
                player.transform.localScale += new Vector3(pineconeSize * sizeMultiplier, pineconeSize * sizeMultiplier, player.transform.localScale.z);
                Destroy(collision.gameObject);
            }
        }
        UpdateText();
    }
}
