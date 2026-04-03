using TMPro.EditorUtilities;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public UIManager uiManager;
    public bool IsGameOver {get; private set; }

    private int score = 0;
    void Start()
    {
        uiManager.SetScoreText(score);
    }

    
}
