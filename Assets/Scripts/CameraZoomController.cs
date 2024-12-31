using UnityEngine;
using System.Collections;
using Unity.Cinemachine;

public class CameraZoomController : MonoBehaviour
{
    // Levels
    public GameObject[] levels;

    private ObjectInteractions objectInteractions; // Reference to ObjectInteractions script
    private CinemachineCamera cineCamCamera; // Reference to the Cinemachine Virtual Camera
    private bool isZooming = false; // Flag to prevent multiple zooms at the same time
    private int zoomCount = 0; // To track the current zoom stage

    // Define zoom thresholds and corresponding orthographic size increments
    private float[] zoomThresholds = { 30f, 100f, 400f };
    private float[] zoomIncrements = { 5f, 10f, 15f };

    // Duration of each zoom transition in seconds
    public float zoomDuration = 1f;

    void Start()
    {
        // Find the CineCam GameObject by tag and get its CinemachineVirtualCamera component
        if (cineCamCamera == null)
        {
            GameObject cineCam = GameObject.FindGameObjectWithTag("CineCam");
            if (cineCam != null)
            {
                cineCamCamera = cineCam.GetComponent<CinemachineCamera>();
                if (cineCamCamera == null)
                {
                    Debug.LogError("CineCam does not have a CinemachineVirtualCamera component.");
                }
            }
            else
            {
                Debug.LogError("No GameObject with tag 'CineCam' found.");
            }
        }

        // Find the Player GameObject by tag and get its ObjectInteractions component
        if (objectInteractions == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                objectInteractions = player.GetComponent<ObjectInteractions>();
                if (objectInteractions == null)
                {
                    Debug.LogError("Player does not have an ObjectInteractions component.");
                }
            }
            else
            {
                Debug.LogError("No GameObject with tag 'Player' found.");
            }
        }
    }

    void Update()
    {
        // Check if we have more zoom stages and are not currently zooming
        if (objectInteractions != null && zoomCount < zoomThresholds.Length && !isZooming)
        {
            // Check if playerSizeCounter exceeds the current zoom threshold
            if (objectInteractions.playerSizeCounter > zoomThresholds[zoomCount])
            {
                // Start the zoom out Coroutine with the corresponding increment
                StartCoroutine(ZoomOut(zoomIncrements[zoomCount]));
            }
        }
    }

    void FadeBackground(GameObject level)
    {
        // Find the script in the scene
        FadeOut fadeScript = FindObjectOfType<FadeOut>();

        // Call the fade-out method on the target GameObject
        // fadeScript.FadeOutBackground(myGameObject);
    }

    /// <summary>
    /// Coroutine to smoothly zoom out the camera.
    /// </summary>
    /// <param name="increment">The amount to increase the orthographic size.</param>
    /// <returns></returns>
    IEnumerator ZoomOut(float increment)
    {
        if (cineCamCamera == null)
        {
            yield break;
        }

        isZooming = true;
        Debug.Log($"Starting Zoom Out {zoomCount + 1}: Increasing orthographic size by {increment}");

        // Get current orthographic size
        float currentSize = cineCamCamera.Lens.OrthographicSize;
        float targetSize = currentSize + increment;

        float elapsed = 0f;

        // Fade Bg Out
        FadeBackground(levels[zoomCount]);

        while (elapsed < zoomDuration)
        {
            // Calculate normalized time
            float t = elapsed / zoomDuration;
            // Apply ease-out cubic easing
            t = 1 - Mathf.Pow(1 - t, 3);

            // Lerp the orthographic size
            cineCamCamera.Lens.OrthographicSize = Mathf.Lerp(currentSize, targetSize, t);

            elapsed += Time.deltaTime;
            yield return null;
        }

        // Ensure the final size is set
        cineCamCamera.Lens.OrthographicSize = targetSize;
        Debug.Log($"Completed Zoom Out {zoomCount}: New orthographic size: {cineCamCamera.Lens.OrthographicSize}");

        // Increase playerLevel in the ObjectInteractions script
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            ObjectInteractions playerInteractions = player.GetComponent<ObjectInteractions>();
            if (playerInteractions != null)
            {
                playerInteractions.playerLevel++;
                Debug.Log($"Player level increased to: {playerInteractions.playerLevel}");
            }
            else
            {
                Debug.LogError("ObjectInteractions script not found on Player!");
            }
        }
        else
        {
            Debug.LogError("Player GameObject not found!");
        }

        // Reset zooming flag and increment zoom count
        isZooming = false;
        zoomCount++;
    }
}
