using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ObjectInteractions : MonoBehaviour
{
    public GameObject player;
    public int playerLevel = 1;
    public TextMeshProUGUI text;
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
    public Sprite pebbleIcon;
    public Sprite ladybugIcon;
    public Sprite leafIcon;
    public Sprite coinIcon;
    // public Sprite antIcon;

    void Start()
    {
        // Consume History
        GameObject consumeHistoryTextObj = GameObject.FindGameObjectWithTag("ConsumeHistoryText");
        if (consumeHistoryTextObj != null)
        {
            consumeHistoryText = consumeHistoryTextObj.GetComponent<TextMeshProUGUI>();
        }
        GameObject consumeHistoryIconObj = GameObject.FindGameObjectWithTag("ConsumeHistoryIcon");
        if (consumeHistoryIconObj != null)
        {
            consumeHistoryIcon = consumeHistoryIconObj.GetComponent<Image>();
        }
        // find AudioManager
        GameObject audioManagerObject = GameObject.FindGameObjectWithTag("AudioManager");
        if (audioManagerObject != null)
        {
            audioManager = audioManagerObject.GetComponent<AudioManager>();
        }

        // origanising colliders
        Collider2D[] colliders = player.GetComponents<Collider2D>();
        if (colliders.Length > 1)
        {
            mainCollider = colliders[0];  // main collider
            triggerCollider = colliders[1];  // trigger collider
        }

        UpdateText();
        //Debug.Log("Initial player size: " + playerSize);
        //Debug.Log("Initial player size counter: " + playerSizeCounter);
    }

    void UpdateText()
    {
        // THIS MATH NOT MATHING UGHHHH, still works tho
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

    // consume
    private void OnTriggerEnter2D(Collider2D collision)
    {
        string tag = collision.gameObject.tag;
        float measurement;
        float size;
        // do checks to consume
        if (GetMeasurementAndSize(tag, out measurement, out size))
        {
            if (playerSize >= size)
            {
                ConsumeObject(measurement, size, collision);
                consumeHistoryText.text = FormatTag(tag);
                consumeHistoryIcon.sprite = GetIcon(tag);
            }
        }
    }

    // allows player to break collision to consume
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

    // list of all objects
    public bool GetMeasurementAndSize(string tag, out float measurement, out float size)
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
                size = 3.5f;
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

    // get icon for history consume
    private Sprite GetIcon(string tag)
    {
        switch (tag)
        {
            case "GrainOfSand":
                return grainOfSandIcon;
            case "Ant":
                return antIcon;
            case "Pebble":
                return pebbleIcon;
            case "Ladybug":
                return ladybugIcon;
            case "Leaf":
                return leafIcon;
            // case "Coin":
            //     return coinIcon;
            // case "SmallFlower":
            //     return smallFlowerIcon;
            // case "Feather":
            //     return featherIcon;
            default:
                return null;
        }
    }

    // actual consume part
    private void ConsumeObject(float measurement, float size, Collider2D collision)
    {
        audioManager?.Play("Collect");

        playerSizeCounter += measurement;
        playerSize += size / 10;

        ScalePlayer(size);
        Destroy(collision.gameObject);
        UpdateText();
    }


    private void ScalePlayer(float sizeIncrement)
    {
        float scaleIncrement = sizeIncrement / 10;
        player.transform.localScale += new Vector3(scaleIncrement, scaleIncrement, 0);
    }

    private string FormatTag(string tag)
    {
        // formats the tag to more readable form. example "GrainOfSand" to "Grain Of Sand"
        return System.Text.RegularExpressions.Regex.Replace(tag, "([A-Z])", " $1").Trim();
    }
}
