using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.InputSystem;

public class AlbumManager : MonoBehaviour
{
    public static bool isOpen = false;

    [Header("UI Références")]
    public GameObject albumPanel;
    public Transform gridContent;
    public GameObject photoPrefab;

    [Header("UI Plein Écran")]
    public GameObject fullscreenPanel;
    public RawImage bigPhotoDisplay;
    public Button closeButton;

    void Start()
    {
        albumPanel.SetActive(false);
        fullscreenPanel.SetActive(false);

        if (closeButton != null)
        {
            closeButton.onClick.AddListener(CloseFullscreen);
        }
    }

    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.tabKey.wasPressedThisFrame)
        {
            ToggleAlbum();
        }
    }

    public void ToggleAlbum()
    {
        bool isActive = !albumPanel.activeSelf;
        albumPanel.SetActive(isActive);
        isOpen = isActive;

        if (isActive)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            LoadPhotosFromDisk();
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            CloseFullscreen(); 
        }
    }

    void LoadPhotosFromDisk()
    {
        foreach (Transform child in gridContent)
        {
            Destroy(child.gameObject);
        }

        string saveDirectory = Path.Combine(Application.persistentDataPath, "Photos");
        if (!Directory.Exists(saveDirectory)) return;

        string[] files = Directory.GetFiles(saveDirectory, "*.png");

        foreach (string file in files)
        {
            byte[] bytes = File.ReadAllBytes(file);
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(bytes);

            GameObject newPhoto = Instantiate(photoPrefab, gridContent);
            RawImage rawImage = newPhoto.GetComponent<RawImage>();
            rawImage.texture = texture;

            Button btn = newPhoto.GetComponent<Button>();
            if (btn != null)
            {
                Texture2D tempTex = texture; 
                btn.onClick.AddListener(() => OpenFullscreen(tempTex));
            }
        }
    }

    public void OpenFullscreen(Texture2D photoToDisplay)
    {
        bigPhotoDisplay.texture = photoToDisplay; 
        fullscreenPanel.SetActive(true); 
    }

    public void CloseFullscreen()
    {
        fullscreenPanel.SetActive(false); 
    }
}