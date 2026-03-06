using UnityEngine;

[CreateAssetMenu(fileName = "Nouvelle Quete", menuName = "FlashAndFriends/Quete Photo")]
public class PhotoQuest : ScriptableObject
{
    public string titreQuete;

    [TextArea(3, 5)] 
    public string description;

    [Tooltip("Le Tag exact que le joueur doit prendre en photo (ex: PNJ, Decor)")]
    public string tagCible;

    public int pointsRecompense = 200;
}