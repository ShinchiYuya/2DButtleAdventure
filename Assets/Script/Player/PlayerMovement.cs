using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float _speed; // �v���C���[�̃X�s�[�h
    [SerializeField] float _jumpForce; //�v���C���[�̃W�����v��
    [SerializeField] float _jumpPower; // �v���C���[���W�����v�����Ƃ��ɉ���鋭��
    [SerializeField] int _jumpCount; // �v���C���[�̃W�����v�J�E���g
    [SerializeField] int damageAmount; // �_���[�W�A���E���g
    //[SerializeField] AudioClip _jumpAudio; // �W�����v�����Ƃ��̉�
    //[SerializeField] AudioClip _runAudio; // �����Ă���Ƃ��̉�
    //[SerializeField] AudioClip _deathAudio; // ���񂾂Ƃ��̉�
    
    int _maxJump = 2; // �ő�W�����v��
    int _damage; // �_���[�W
    float _h = 0; // ����
    bool _isGrounded = true; //�n�ʔ���
    bool isDead = false; // ���S�t���O
    bool isRunning = true; // �����Ă邩�t���O
    bool isJumpping = false; // �W�����v���Ă��邩�̃t���O

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

        // �����Ă��邩�ǂ������A�j���[�V�����ɓ`����
        //this._anim.SetBool("isRunning", Mathf.Abs(_h) > 0 && _isGrounded);

        if (Mathf.Abs(_h) > 0 && _isGrounded)
        {
            /*if (_runAudio != null && audioSource != null && !audioSource.isPlaying)
            {
                isRunning = true;
                audioSource.PlayOneShot(_runAudio); // �����Ă��鎞�̃T�E���h�Đ�
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

                /*if (_jumpAudio != null && audioSource != null) // �W�����v���̃T�E���h�Đ�
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
