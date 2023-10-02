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
        // �X�R�A�̍X�V��UI�e�L�X�g�ɔ��f
        _scoreText.text = "Score: " + _score;
    }

    // �X�R�A���X�V���郁�\�b�h
    public void UpdateScore(int newScore)
    {
        _score = newScore;
    }

    public void AddScore(int amount)
    {
        _score += amount;
    }
}
