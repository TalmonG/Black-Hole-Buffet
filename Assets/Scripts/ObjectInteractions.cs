using UnityEngine;
using TMPro;

public class ObjectInteractions : MonoBehaviour
{
    public GameObject player;
    public TextMeshProUGUI text;  // Reference to the UI TextMeshProUGUI component
    public float playerSize = 0.2f;
    public float playerSizeCounter = 2f;

    private Collider2D mainCollider;
    private Collider2D triggerCollider;

    // Measurements and sizes
    private float grainOfSandMeasurement = 1f;
    private float antMeasurement = 2f;
    private float pebbleMeasurement = 5f;
    private float ladybugMeasurement = 9f;
    private float leafMeasurement = 14f;
    private float coinMeasurement = 19f;
    private float smallFlowerMeasurement = 40f;
    private float featherMeasurement = 60f;
    private float snailMeasurement = 85f;
    private float pineconeMeasurement = 110f;

    private float grainOfSandSize = 0.1f;
    private float antSize = 0.3f;
    private float pebbleSize = 0.5f;
    private float ladybugSize = 0.9f;
    private float leafSize = 1.4f;
    private float coinSize = 1.9f;
    private float smallFlowerSize = 4.0f;
    private float featherSize = 6.0f;
    private float snailSize = 8.5f;
    private float pineconeSize = 11.0f;

    private AudioManager audioManager;
    private TextMeshProUGUI consumeHistoryText;
    private GameObject consumeHistoryIcon;

    void Start()
    {
        // Consume History
        GameObject consumeHistoryTextObj = GameObject.FindGameObjectWithTag("ConsumeHistoryText");
        consumeHistoryText = consumeHistoryTextObj.GetComponent<TextMeshProUGUI>();

        GameObject consumeHistoryIcon = GameObject.FindGameObjectWithTag("ConsumeHistoryIcon");

        // AudioManager
        GameObject audioManagerObject = GameObject.FindGameObjectWithTag("AudioManager");
        if (audioManagerObject != null)
        {
            audioManager = audioManagerObject.GetComponent<AudioManager>();
        }
        else
        {
            Debug.LogWarning("AudioManager not found in the scene!");
        }

        Collider2D[] colliders = player.GetComponents<Collider2D>();
        if (colliders.Length > 1)
        {
            mainCollider = colliders[0];  // Main Collider
            triggerCollider = colliders[1];  // Trigger Collider
        }

        UpdateText();
        Debug.Log("Initial Player Size: " + playerSize);
        Debug.Log("Initial Player Size Counter: " + playerSizeCounter);
    }

    void UpdateText()
    {
        if (playerSizeCounter >= 1 && playerSizeCounter < 10) // 1cm = 10mm
        {
            text.text = Mathf.RoundToInt(playerSizeCounter) + " Millimeters";
        }
        else if (playerSizeCounter >= 10 && playerSizeCounter < 1000) // 100cm = 1000mm
        {
            text.text = Mathf.FloorToInt(playerSizeCounter / 10).ToString("F2") + " Centimeters"; // Display whole centimeters
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

    private void OnTriggerEnter2D(Collider2D collision) // Consume 
    {
        if (collision.gameObject.CompareTag("GrainOfSand") && playerSize >= grainOfSandSize)
        {
            ConsumeObject(grainOfSandMeasurement, grainOfSandSize, collision);
            consumeHistoryText.text = "Grain Of Sand";
            
        }
        else if (collision.gameObject.CompareTag("Ant") && playerSize >= antSize)
        {
            ConsumeObject(antMeasurement, antSize, collision);
        }
        else if (collision.gameObject.CompareTag("Pebble") && playerSize >= pebbleSize)
        {
            ConsumeObject(pebbleMeasurement, pebbleSize, collision);
        }
        else if (collision.gameObject.CompareTag("Ladybug") && playerSize >= ladybugSize)
        {
            ConsumeObject(ladybugMeasurement, ladybugSize, collision);
        }
        else if (collision.gameObject.CompareTag("Leaf") && playerSize >= leafSize)
        {
            ConsumeObject(leafMeasurement, leafSize, collision);
        }
        else if (collision.gameObject.CompareTag("Coin") && playerSize >= coinSize)
        {
            ConsumeObject(coinMeasurement, coinSize, collision);
        }
        else if (collision.gameObject.CompareTag("SmallFlower") && playerSize >= smallFlowerSize)
        {
            ConsumeObject(smallFlowerMeasurement, smallFlowerSize, collision);
        }
        else if (collision.gameObject.CompareTag("Feather") && playerSize >= featherSize)
        {
            ConsumeObject(featherMeasurement, featherSize, collision);
        }
        else if (collision.gameObject.CompareTag("Snail") && playerSize >= snailSize)
        {
            ConsumeObject(snailMeasurement, snailSize, collision);
        }
        else if (collision.gameObject.CompareTag("Pinecone") && playerSize >= pineconeSize)
        {
            ConsumeObject(pineconeMeasurement, pineconeSize, collision);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("GrainOfSand") && playerSize >= grainOfSandSize)
        {
            collision.gameObject.GetComponent<Collider2D>().isTrigger = true;
        }
        else if (collision.gameObject.CompareTag("Ant") && playerSize >= antSize)
        {
            collision.gameObject.GetComponent<Collider2D>().isTrigger = true;
        }
        else if (collision.gameObject.CompareTag("Pebble") && playerSize >= pebbleSize)
        {
            collision.gameObject.GetComponent<Collider2D>().isTrigger = true;
        }
        else if (collision.gameObject.CompareTag("Ladybug") && playerSize >= ladybugSize)
        {
            collision.gameObject.GetComponent<Collider2D>().isTrigger = true;
        }
        else if (collision.gameObject.CompareTag("Leaf") && playerSize >= leafSize)
        {
            collision.gameObject.GetComponent<Collider2D>().isTrigger = true;
        }
        else if (collision.gameObject.CompareTag("Coin") && playerSize >= coinSize)
        {
            collision.gameObject.GetComponent<Collider2D>().isTrigger = true;
        }
        else if (collision.gameObject.CompareTag("SmallFlower") && playerSize >= smallFlowerSize)
        {
            collision.gameObject.GetComponent<Collider2D>().isTrigger = true;
        }
        else if (collision.gameObject.CompareTag("Feather") && playerSize >= featherSize)
        {
            collision.gameObject.GetComponent<Collider2D>().isTrigger = true;
        }
        else if (collision.gameObject.CompareTag("Snail") && playerSize >= snailSize)
        {
            collision.gameObject.GetComponent<Collider2D>().isTrigger = true;
        }
        else if (collision.gameObject.CompareTag("Pinecone") && playerSize >= pineconeSize)
        {
            collision.gameObject.GetComponent<Collider2D>().isTrigger = true;
        }
    }


    private void ConsumeObject(float measurement, float size, Collider2D collision)
    {
        audioManager.Play("Collect");

        // Increment player size and counter
        playerSizeCounter += measurement;
        playerSize += size / 10;

        // Update player scale
        ScalePlayer(size);

        Debug.Log($"Consumed {collision.gameObject.tag}: PlayerSize={playerSize}, PlayerSizeCounter={playerSizeCounter}");

        // Destroy the consumed object
        Destroy(collision.gameObject);

        // Update the displayed text for the player's size
        UpdateText();
    }

    private void ScalePlayer(float sizeIncrement)
    {
        float scaleIncrement = sizeIncrement / 10;
        player.transform.localScale += new Vector3(scaleIncrement, scaleIncrement, 0);
    }
}
