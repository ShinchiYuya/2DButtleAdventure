using System;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] static float _timer;
    [SerializeField] float _maxTime;
    [SerializeField] Text _textCountDown;
    [SerializeField] PlayerHP playerHP;
 
    public static GameManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    void Start()
    {
        _timer = _maxTime;
    }

    void Update()
    {
        Timer();
    }

    void Timer()
    {
        if (_textCountDown != null && _timer > 0f)
        {
            _textCountDown.text = string.Format("Time: {0:00.00}", _timer);
            _timer -= Time.deltaTime;
        }
        else if (_timer <= 0)
        {
            playerHP.InflictDamage();
            _textCountDown.text = "END";
        }
    }

}