using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ObjectInteractions : MonoBehaviour
{
    public GameObject player;
    public TextMeshProUGUI text;  // Reference to the UI TextMeshProUGUI component
    public float playerSize = 0.2f;
    public float playerSizeCounter = 2f;

    private Collider2D mainCollider;
    private Collider2D triggerCollider;

    private AudioManager audioManager;
    private TextMeshProUGUI consumeHistoryText;
    public Image consumeHistoryIcon;

    // Icons
    public Sprite grainOfSandIcon; 
    public Sprite antIcon; 
    // Add other icons as needed

    void Start()
    {
        // Consume History
        GameObject consumeHistoryTextObj = GameObject.FindGameObjectWithTag("ConsumeHistoryText");
        if (consumeHistoryTextObj != null)
            consumeHistoryText = consumeHistoryTextObj.GetComponent<TextMeshProUGUI>();
        else
            Debug.LogError("ConsumeHistoryText GameObject not found!");

        GameObject consumeHistoryIconObj = GameObject.FindGameObjectWithTag("ConsumeHistoryIcon");
        if (consumeHistoryIconObj != null)
            consumeHistoryIcon = consumeHistoryIconObj.GetComponent<Image>();
        else
            Debug.LogError("ConsumeHistoryIcon GameObject not found!");

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

        // Colliders
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
            text.text = (playerSizeCounter / 10).ToString("F2") + " Centimeters"; // Display with two decimal places
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
        string tag = collision.gameObject.tag;
        float measurement;
        float size;

        if (GetMeasurementAndSize(tag, out measurement, out size))
        {
            if (playerSize >= size)
            {
                ConsumeObject(measurement, size, collision);
                consumeHistoryText.text = FormatTag(tag);
                consumeHistoryIcon.sprite = GetIcon(tag);
            }
            else
            {
                Debug.Log($"Player size {playerSize} is too small to consume {tag}.");
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        string tag = collision.gameObject.tag;
        float measurement;
        float size;

        if (GetMeasurementAndSize(tag, out measurement, out size))
        {
            if (playerSize >= size)
            {
                // Set the collided object's collider to trigger
                collision.gameObject.GetComponent<Collider2D>().isTrigger = true;
            }
        }
    }

    private bool GetMeasurementAndSize(string tag, out float measurement, out float size)
    {
        measurement = 0f;
        size = 0f;

        switch (tag)
        {
            case "GrainOfSand":
                measurement = 1f;
                size = 0.1f;
                break;
            case "Ant":
                measurement = 2f;
                size = 0.3f;
                break;
            case "Pebble":
                measurement = 5f;
                size = 0.5f;
                break;
            case "Ladybug":
                measurement = 9f;
                size = 0.9f;
                break;
            case "Leaf":
                measurement = 14f;
                size = 1.4f;
                break;
            case "Coin":
                measurement = 19f;
                size = 1.9f;
                break;
            case "SmallFlower":
                measurement = 40f;
                size = 4.0f;
                break;
            case "Feather":
                measurement = 60f;
                size = 6.0f;
                break;
            case "Snail":
                measurement = 85f;
                size = 8.5f;
                break;
            case "Pinecone":
                measurement = 110f;
                size = 11.0f;
                break;
            default:
                Debug.LogWarning($"Unhandled tag: {tag}");
                return false;
        }

        return true;
    }

    private Sprite GetIcon(string tag)
    {
        switch (tag)
        {
            case "GrainOfSand":
                return grainOfSandIcon;
            case "Ant":
                return antIcon;
            // Add cases for other tags and their corresponding icons
            default:
                return null;
        }
    }

    private void ConsumeObject(float measurement, float size, Collider2D collision)
    {
        audioManager?.Play("Collect");

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

    private string FormatTag(string tag)
    {
        // Format the tag to a more readable form, e.g., "GrainOfSand" to "Grain Of Sand"
        return System.Text.RegularExpressions.Regex.Replace(tag, "([A-Z])", " $1").Trim();
    }
}
