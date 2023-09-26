using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float _speed; // �v���C���[�̃X�s�[�h
    [SerializeField] float _jumpForce; //�v���C���[�̃W�����v��
    [SerializeField] int _maxJumpCount; // �ő�W�����v��
    [SerializeField] float _jumpPower; // �v���C���[���W�����v�����Ƃ��ɉ���鋭��
    
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
        //���ꂼ��̕����̓��͂����o����
        float h = Input.GetAxis("Horizontal");
        //���͂ɉ����Ńv���C���[�𓮂���
        Vector2 vector2 = new Vector2(h, 0) * _speed ;
        _rb2d.velocity = vector2;
    }

    void Jump()
    {
        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space)))
        {
            if (_jumpCount < _maxJumpCount)
            {
                // �v���C���[�̌��݂̌������擾
                Vector2 _playerDirection = Vector2.right * transform.localScale.x; 
                // �W�����v�͂�K�p��������x�N�g�����v�Z
                Vector2 _jumpDirection = Vector2.up + _playerDirection * _jumpPower;
                // �͂�������
                _rb2d.AddForce(_jumpDirection * _jumpForce, ForceMode2D.Impulse);
                _jumpCount++;
                _isGrounded =false;
            }
        }
    }
}
