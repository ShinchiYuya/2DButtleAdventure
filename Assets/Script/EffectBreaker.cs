using UnityEngine;

public class EffectBreaker : MonoBehaviour
{   
    void Start()
    {
        //�G�t�F�N�g�����������2�b��ɃI�u�W�F�N�g���폜����
        Invoke("BreakEffect", 2.0f);
    }

    //�G�t�F�N�g(�������g)���폜����
    void BreakEffect()
    {
        Destroy(gameObject);
    }
}
