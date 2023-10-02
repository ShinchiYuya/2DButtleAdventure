using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHP : MonoBehaviour
{
    public static PlayerHP instance;

    [SerializeField] int _maxHP;
    [SerializeField] int currentHP;
    [SerializeField] AudioClip deathClip;
    [SerializeField] string targetSceneName;
    [SerializeField] float stepTime = 2f;

    int damageAmount = 2147483647;
    bool isDead = false;
    AudioSource audioSource;
    bool hasSceneChange = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (isDead && !hasSceneChange)
        {
            //Debug.Log("1");
            StartCoroutine(DelayedSceneChange());
            hasSceneChange = true;
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return; // 既に死亡している場合は処理を中断

        currentHP -= damage; // ダメージを体力から減算

        // 体力が0以下になった場合は敵を破壊
        if (currentHP <= 0)
        {
            isDead = true; // 死亡フラグを設定

            // 死亡時のサウンドを再生
            if (deathClip != null)
            {
                audioSource.PlayOneShot(deathClip);
            }
        }
    }

    public void InflictDamage()
    {
        TakeDamage(damageAmount);
    }

    IEnumerator DelayedSceneChange()
    {
        // 死亡時の待機秒数を指定（stepTime変数の値）
        yield return new WaitForSeconds(stepTime);

        // シーンを切り替え
        SceneManager.LoadScene(targetSceneName);
        //Debug.Log("2");
    }
}
