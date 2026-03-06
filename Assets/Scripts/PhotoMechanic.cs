using UnityEngine;
using UnityEngine.InputSystem;
using System.IO;
using System;
using TMPro;
using System.Collections;
using UnityEngine.UI; 

public class PhotoMechanic : MonoBehaviour
{
    [Header("Références Caméra")]
    public Camera mainCamera;
    public Camera photoCamera;
    public RenderTexture renderTexture;

    [Header("Paramètres Raycast & Score")]
    public float maxDistance = 50f;
    public float optimalDistanceMin = 3f;
    public float optimalDistanceMax = 10f;

    [Header("UI Score")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI feedbackText;
    public static int totalScore = 0;

    [Header("Game Feel")]
    public CanvasGroup flashPanel; 
    public AudioSource audioSource; 
    public AudioClip shutterSound; 

    [Header("Input")]
    public InputAction shootAction;

    private string saveDirectory;

    void Start()
    {
        shootAction.Enable();
        shootAction.performed += ctx => TakePhoto();
        saveDirectory = Path.Combine(Application.persistentDataPath, "Photos");
        if (!Directory.Exists(saveDirectory)) Directory.CreateDirectory(saveDirectory);

        UpdateScoreUI();
    }

    void Update()
    {
        if (AlbumManager.isOpen || QuestManager.isEndScreenOpen || PauseMenu.isPaused || PlayerController.isIntroPlaying) return;

        Ray ray = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxDistance))
        {
            if (hit.collider.CompareTag("PNJ"))
            {
                NPCBehavior npc = hit.collider.GetComponent<NPCBehavior>();
                if (npc != null) npc.ReactToCamera(transform.position);
            }
        }
    }

    void TakePhoto()
    {
        if (AlbumManager.isOpen || QuestManager.isEndScreenOpen || PauseMenu.isPaused || PlayerController.isIntroPlaying) return;

        if (audioSource != null && shutterSound != null)
        {
            audioSource.PlayOneShot(shutterSound);
        }

        if (flashPanel != null)
        {
            StartCoroutine(FlashEffect());
        }

        EvaluatePhoto();
        StartCoroutine(CaptureProcess());
    }

    IEnumerator FlashEffect()
    {
        flashPanel.alpha = 1f;

        float duration = 0.2f; 
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            flashPanel.alpha = Mathf.Lerp(1f, 0f, timer / duration);
            yield return null; 
        }

        flashPanel.alpha = 0f; 
    }

    void EvaluatePhoto()
    {
        Ray ray = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        int photoScore = 0;
        string feedbackMsg = "Raté...";

        if (Physics.Raycast(ray, out hit, maxDistance))
        {
            if (hit.collider.CompareTag("PNJ"))
            {
                NPCBehavior npc = hit.collider.GetComponent<NPCBehavior>();

                if (npc != null)
                {
                    if (npc.isPosing) { photoScore += 50; feedbackMsg = "Superbe Pose !"; }
                    else { feedbackMsg = "Pris au dépourvu..."; }

                    float distance = Vector3.Distance(transform.position, hit.transform.position);
                    if (distance >= optimalDistanceMin && distance <= optimalDistanceMax)
                    {
                        photoScore += 50; feedbackMsg += "\nCadrage Parfait !";
                    }
                    else if (distance < optimalDistanceMin) { feedbackMsg += "\nUn peu trop près..."; }
                    else { feedbackMsg += "\nUn peu trop loin..."; }
                }
            }
            else
            {
                feedbackMsg = "Joli décor !";
                photoScore += 10;
            }

            if (QuestManager.instance != null) QuestManager.instance.VerifierQuete(hit.collider.tag);
        }

        totalScore += photoScore;
        UpdateScoreUI();
        StartCoroutine(ShowFeedback(feedbackMsg + " (+" + photoScore + ")"));
    }

    void UpdateScoreUI() { if (scoreText != null) scoreText.text = "Good Vibes : " + totalScore; }

    IEnumerator ShowFeedback(string message)
    {
        if (feedbackText != null) { feedbackText.text = message; yield return new WaitForSeconds(2f); feedbackText.text = ""; }
    }

    IEnumerator CaptureProcess()
    {
        yield return new WaitForEndOfFrame();
        photoCamera.gameObject.SetActive(true);
        photoCamera.Render();
        RenderTexture.active = renderTexture;
        Texture2D photoTexture = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGB24, false);
        photoTexture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        photoTexture.Apply();
        RenderTexture.active = null;
        photoCamera.gameObject.SetActive(false);
        byte[] bytes = photoTexture.EncodeToPNG();
        string fileName = "Photo_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".png";
        string fullPath = Path.Combine(saveDirectory, fileName);
        File.WriteAllBytes(fullPath, bytes);
        Destroy(photoTexture);
    }

    void OnDisable() { shootAction.Disable(); }
}