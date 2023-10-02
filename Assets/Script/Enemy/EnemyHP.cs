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
        //�G�t�F�N�g�𐶐�����
        GameObject effect = Instantiate(breakEffect) as GameObject;
        //�G�t�F�N�g����������ꏊ�����肷��(�G�I�u�W�F�N�g�̏ꏊ)
        effect.transform.position = gameObject.transform.position;
    }

    public void DeathPoint()
    {
        if (currentHP <= 0)
        {
            //�X�R�A���Z
            scoreManager.AddScore(deathScore);
            //���񂾂Ƃ��̉�
            audioSource.PlayOneShot(deathClip);
            //�G�t�F�N�g�̐���
            GenerateEffect();
        }
    }
}

