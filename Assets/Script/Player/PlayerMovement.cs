using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float _speed; // プレイヤーのスピード
    [SerializeField] float _jumpForce; //プレイヤーのジャンプ力
    [SerializeField] int _maxJumpCount; // 最大ジャンプ数
    [SerializeField] float _jumpPower; // プレイヤーがジャンプしたときに加わる強さ
    
    int _jumpCount;
    bool _isGrounded = true;

    Rigidbody2D _rb2d;
    Animator _anim;
    AudioSource _audio;

    void Start()
    {
        _anim = GetComponent<Animator>();
        _audio = GetComponent<AudioSource>();
        _rb2d = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Move();
        Jump();
    }

    void Move()
    {
        //それぞれの方向の入力を検出する
        float h = Input.GetAxis("Horizontal");
        //入力に応じでプレイヤーを動かす
        Vector2 vector2 = new Vector2(h, 0) * _speed ;
        _rb2d.velocity = vector2;
    }

    void Jump()
    {
        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space)))
        {
            if (_jumpCount < _maxJumpCount)
            {
                // プレイヤーの現在の向きを取得
                Vector2 _playerDirection = Vector2.right * transform.localScale.x; 
                // ジャンプ力を適用する方向ベクトルを計算
                Vector2 _jumpDirection = Vector2.up + _playerDirection * _jumpPower;
                // 力を加える
                _rb2d.AddForce(_jumpDirection * _jumpForce, ForceMode2D.Impulse);
                _jumpCount++;
                _isGrounded =false;
            }
        }
    }
}
