using UnityEngine;

public class PersistentAudio : MonoBehaviour
{
    void Awake()
    {
        GameObject[] musicObjs = GameObject.FindGameObjectsWithTag("GameController");
        if (musicObjs.Length > 1)
        {
            Destroy(this.gameObject);
        }
        else
        {
            DontDestroyOnLoad(this.gameObject);
        }
    }
}