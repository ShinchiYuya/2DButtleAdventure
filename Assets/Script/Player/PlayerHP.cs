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
        if (isDead) return; // ���Ɏ��S���Ă���ꍇ�͏����𒆒f

        currentHP -= damage; // �_���[�W��̗͂��猸�Z

        // �̗͂�0�ȉ��ɂȂ����ꍇ�͓G��j��
        if (currentHP <= 0)
        {
            isDead = true; // ���S�t���O��ݒ�

            // ���S���̃T�E���h���Đ�
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
        // ���S���̑ҋ@�b�����w��istepTime�ϐ��̒l�j
        yield return new WaitForSeconds(stepTime);

        // �V�[����؂�ւ�
        SceneManager.LoadScene(targetSceneName);
        //Debug.Log("2");
    }
}
