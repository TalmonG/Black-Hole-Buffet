using UnityEngine;
using TMPro;

public class ScaleCounter : MonoBehaviour
{
    private ObjectInteractions objectInteractions; // Reference to the player script
    public TextMeshProUGUI text;  // Reference to the UI TextMeshProUGUI component

    // Start is called before the first frame update
    void Start()
    {
        // Get the TextMeshProUGUI component attached to the object
        text = GetComponent<TextMeshProUGUI>();

        // Make sure the text component exists
        if (text == null)
        {
            Debug.LogError("TextMeshProUGUI component not found on this GameObject.");
        }

        // Find the first ObjectInteractions script on the player
        objectInteractions = Object.FindFirstObjectByType<ObjectInteractions>();

        if (objectInteractions == null)
        {
            Debug.LogError("ObjectInteractions script not found in the scene.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (objectInteractions != null && text != null)
        {
            // Update the UI with the player's scale size
            text.text = "Scale: " + objectInteractions.playerSize.ToString("F2");
        }
    }
}

// fix this later
