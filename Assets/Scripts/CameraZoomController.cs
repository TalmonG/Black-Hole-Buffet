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
    private float[] zoomThresholds = { 30f, 120f, 400000f };
    private float[] zoomIncrements = { 5f, 10f, 15f };

    // Duration of each zoom transition in seconds
    public float zoomDuration = 1f;

    // CameraBounds scaling values (single float per scale)
    private float[] cameraBoundsScales = { 0.7509878f, 1.0f, 1.5f };

    // Player speed values (single float per zoom stage)
    private float[] playerSpeeds = { 10f, 15f, 20f };

    private Transform cameraBoundsTransform;
    private PlayerControls playerControls; // Reference to PlayerControls script

    private AudioManager audioManager;


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

                // Get PlayerControls component
                playerControls = player.GetComponent<PlayerControls>();
                if (playerControls == null)
                {
                    Debug.LogError("Player does not have a PlayerControls component.");
                }
            }
            else
            {
                Debug.LogError("No GameObject with tag 'Player' found.");
            }
        }

        // Find the CameraBounds GameObject by tag
        GameObject cameraBounds = GameObject.FindGameObjectWithTag("CameraBounds");
        if (cameraBounds != null)
        {
            cameraBoundsTransform = cameraBounds.transform;
        }
        else
        {
            Debug.LogError("No GameObject with tag 'CameraBounds' found.");
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

    IEnumerator ZoomOut(float increment)
    {
        if (cineCamCamera == null || cameraBoundsTransform == null)
        {
            yield break;
        }

        isZooming = true;
        audioManager?.Play("ZoomOut");

        Debug.Log($"Starting Zoom Out {zoomCount + 1}: Increasing orthographic size by {increment}");

        // Update CameraBounds scale
        if (zoomCount < cameraBoundsScales.Length)
        {
            float scale = cameraBoundsScales[zoomCount];
            cameraBoundsTransform.localScale = new Vector3(scale, scale, scale);
            Debug.Log($"Set CameraBounds scale to: {scale}");
        }

        // Update Player speed
        if (playerControls != null && zoomCount < playerSpeeds.Length)
        {
            playerControls.maxMoveSpeed = playerSpeeds[zoomCount];
            Debug.Log($"Updated Player maxMoveSpeed to: {playerControls.maxMoveSpeed}");
        }

        // Enable next level
        if (zoomCount + 1 < levels.Length)
        {
            levels[zoomCount + 1].SetActive(true);
            Debug.Log($"Enabled level {zoomCount + 2}");
        }

        // Disable current level
        if (zoomCount < levels.Length)
        {
            levels[zoomCount].SetActive(false);
            Debug.Log($"Disabled level {zoomCount + 1}");
        }

        // Start zooming out
        float currentSize = cineCamCamera.Lens.OrthographicSize;
        float targetSize = currentSize + increment;

        float elapsed = 0f;

        while (elapsed < zoomDuration)
        {
            float t = elapsed / zoomDuration;
            t = 1 - Mathf.Pow(1 - t, 3);

            cineCamCamera.Lens.OrthographicSize = Mathf.Lerp(currentSize, targetSize, t);

            elapsed += Time.deltaTime;
            yield return null;
        }

        cineCamCamera.Lens.OrthographicSize = targetSize;

        // Update Cinemachine Confiner
        CinemachineConfiner confiner = cineCamCamera.GetComponent<CinemachineConfiner>();
        if (confiner != null)
        {
            confiner.InvalidatePathCache();
            Debug.Log("Confiner bounds updated.");
        }
        else
        {
            Debug.LogWarning("CinemachineConfiner component not found.");
        }

        Debug.Log($"Completed Zoom Out {zoomCount}: New orthographic size: {cineCamCamera.Lens.OrthographicSize}");

        isZooming = false;
        zoomCount++;
    }
}
