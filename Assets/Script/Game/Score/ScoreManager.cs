using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    [SerializeField]
    private TextMeshProUGUI pointsText;

    private int _score = 0;

    private void Awake()
    {
        // Đảm bảo chỉ có một instance của ScoreManager
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Hủy đối tượng mới nếu đã có một ScoreManager
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Đảm bảo không bị hủy khi chuyển scene
    }

    public void AddScore(int points)
    {
        _score += points;
        pointsText.text = _score.ToString() + " POINTS";
    }

    public int GetScore()
    {
        return _score;
    }

    public void ResetScore()
    {
        _score = 0;
    }
    public void SetScore(int score)
    {
        _score = score;
    }
}
