using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;

    public void OnEnable()
    {
        SetScoreText(0);
    }

    public void SetScoreText(int score)
    {
        scoreText.text = $"Score: {score}";
    }   
}
