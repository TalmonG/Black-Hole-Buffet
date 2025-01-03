using UnityEngine;
using System.Collections;
using Unity.Cinemachine;

public class CameraZoomController : MonoBehaviour
{
    // levels
    public GameObject[] levels;

    // references
    private ObjectInteractions objectInteractions;
    private CinemachineCamera cineCamCamera;
    private bool isZooming = false;
    private int zoomCount = 0;

    // player size for zoomout to happen
    private float[] zoomThresholds = { 30f, 120f, 200f };
    private float[] zoomIncrements = { 5f, 15f, 30f };
    private float[] playerSpeeds = { 10f, 20f, 50f };


    // CameraBounds scaling values 
    private float[] cameraBoundsScales = { 0.7509878f, 1.23f, 4f };

    public float zoomDuration = 1f;

    private Transform cameraBoundsTransform;
    private PlayerControls playerControls; // Reference to PlayerControls script

    private AudioManager audioManager;


    void Start()
    {
        // Find CineCam
        if (cineCamCamera == null)
        {
            GameObject cineCam = GameObject.FindGameObjectWithTag("CineCam");
            if (cineCam != null)
            {
                cineCamCamera = cineCam.GetComponent<CinemachineCamera>();
            }
        }

        // Find AudioManager
        GameObject audioManagerObject = GameObject.FindGameObjectWithTag("AudioManager");
        if (audioManagerObject != null)
        {
            audioManager = audioManagerObject.GetComponent<AudioManager>();
        }

        // Find Player
        if (objectInteractions == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                objectInteractions = player.GetComponent<ObjectInteractions>();

                // Get PlayerControls
                playerControls = player.GetComponent<PlayerControls>();
            }
        }

        // Find CameraBounds
        GameObject cameraBounds = GameObject.FindGameObjectWithTag("CameraBounds");
        if (cameraBounds != null)
        {
            cameraBoundsTransform = cameraBounds.transform;
        }
    }

    void Update()
    {
        // Check if theres more zoom stages and is not currently zooming
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
        objectInteractions.playerLevel++;

        //Debug.Log("Started zooming");

        // Update CameraBounds scale
        if (zoomCount < cameraBoundsScales.Length)
        {
            float scale = cameraBoundsScales[zoomCount];
            cameraBoundsTransform.localScale = new Vector3(scale, scale, scale);
            //Debug.Log("Bounds Updated");
        }

        // Update Player speed
        if (playerControls != null && zoomCount < playerSpeeds.Length)
        {
            playerControls.maxMoveSpeed = playerSpeeds[zoomCount];
            //Debug.Log("Player Speed Updated");
        }

        // Enable next level
        if (zoomCount + 1 < levels.Length)
        {
            levels[zoomCount + 1].SetActive(true);
            //Debug.Log("Next level enabled");
        }

        // Disable current level
        if (zoomCount < levels.Length)
        {
            levels[zoomCount].SetActive(false);
            //Debug.Log("current level disabled");
        }

        // Start zooming out
        float currentSize = cineCamCamera.Lens.OrthographicSize;
        float targetSize = currentSize + increment;

        float elapsed = 0f;

        // zoom anim
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
