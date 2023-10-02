using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    [SerializeField] Text _scoreText;
    int _score = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    void Update()
    {
        // スコアの更新をUIテキストに反映
        _scoreText.text = "Score: " + _score;
    }

    // スコアを更新するメソッド
    public void UpdateScore(int newScore)
    {
        _score = newScore;
    }

    public void AddScore(int amount)
    {
        _score += amount;
    }
}
