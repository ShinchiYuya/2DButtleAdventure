using UnityEngine;

public class EnemyHP : MonoBehaviour
{
    public static EnemyHP Instance;

    [SerializeField] int maxHP;
    [SerializeField] int currentHP;
    [SerializeField] int deathScore;
    [SerializeField] AudioClip deathClip;
    [SerializeField] public GameObject breakEffect;
    
    private ScoreManager scoreManager;

    bool isDead = false;
    AudioSource audioSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        scoreManager = FindObjectOfType<ScoreManager>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("SlashingAttack"))
        {
            maxHP -= currentHP; 
        }
    }

    void GenerateEffect()
    {
        //エフェクトを生成する
        GameObject effect = Instantiate(breakEffect) as GameObject;
        //エフェクトが発生する場所を決定する(敵オブジェクトの場所)
        effect.transform.position = gameObject.transform.position;
    }

    public void DeathPoint()
    {
        if (currentHP <= 0)
        {
            //スコア加算
            scoreManager.AddScore(deathScore);
            //死んだときの音
            audioSource.PlayOneShot(deathClip);
            //エフェクトの生成
            GenerateEffect();
        }
    }
}

