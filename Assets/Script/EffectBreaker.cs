using UnityEngine;

public class EffectBreaker : MonoBehaviour
{   
    void Start()
    {
        //エフェクトが生成されて2秒後にオブジェクトを削除する
        Invoke("BreakEffect", 2.0f);
    }

    //エフェクト(自分自身)を削除する
    void BreakEffect()
    {
        Destroy(gameObject);
    }
}
