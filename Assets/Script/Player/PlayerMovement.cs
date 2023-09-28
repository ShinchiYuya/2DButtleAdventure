using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float _speed; // プレイヤーのスピード
    [SerializeField] float _jumpForce; //プレイヤーのジャンプ力
    [SerializeField] float _jumpPower; // プレイヤーがジャンプしたときに加わる強さ
    [SerializeField] int _jumpCount; // プレイヤーのジャンプカウント
    [SerializeField] int damageAmount; // ダメージアモウント
    //[SerializeField] AudioClip _jumpAudio; // ジャンプしたときの音
    //[SerializeField] AudioClip _runAudio; // 走っているときの音
    //[SerializeField] AudioClip _deathAudio; // 死んだときの音
    
    int _maxJump = 2; // 最大ジャンプ数
    int _damage; // ダメージ
    float _h = 0; // 向き
    bool _isGrounded = true; //地面判定
    bool isDead = false; // 死亡フラグ
    bool isRunning = true; // 走ってるかフラグ
    bool isJumpping = false; // ジャンプしているかのフラグ

    SpriteRenderer _sprtRdr;
    Vector3 transPos;
    Rigidbody2D _rb2d;
    Animator _anim;
    AudioSource audioSource;

    void Start()
    {
        _anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        _rb2d = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Move();
        Jump();
    }

    void Move()
    {
        _h = Input.GetAxis("Horizontal");
        //_sprtRdr.flipX = _h < 0;
        Vector2 velocity = new Vector2(_h * _speed, _rb2d.velocity.y);
        _rb2d.velocity = velocity;

        // 走っているかどうかをアニメーションに伝える
        //this._anim.SetBool("isRunning", Mathf.Abs(_h) > 0 && _isGrounded);

        if (Mathf.Abs(_h) > 0 && _isGrounded)
        {
            /*if (_runAudio != null && audioSource != null && !audioSource.isPlaying)
            {
                isRunning = true;
                audioSource.PlayOneShot(_runAudio); // 走っている時のサウンド再生
            }*/
        }
        else
        {
            /*if (audioSource != null)
            {
                isRunning = false;
                audioSource.Stop();
            }*/
        }
    }

    void Jump()
    {
        //this._anim.SetTrigger("isJumpping");

        if (Input.GetKeyDown(KeyCode.Space) || (Input.GetKeyDown(KeyCode.W)))
        {
            if (_isGrounded || _jumpCount < _maxJump)
            {
                _rb2d.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
                _jumpCount++;
                _isGrounded = false;

                /*if (_jumpAudio != null && audioSource != null) // ジャンプ時のサウンド再生
                {
                    isJumpping = true;
                    audioSource.PlayOneShot(_jumpAudio);
                    Debug.Log("aaa0");
                }
                else
                {
                    isJumpping = false;
                    audioSource.Stop();
                }*/
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            _jumpCount = 0;
            _isGrounded = true;
        }
    }
}
