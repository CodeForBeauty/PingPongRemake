using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
public class ControlRacket : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler {
    [SerializeField] private float _speed = 5.0f;
    [SerializeField] private Vector2 _moveDirection = Vector3.up;
    [SerializeField] private Vector2 _bounceDirection = Vector2.right;
    [SerializeField] private float _width = 0.8f;

    [SerializeField] private string _ballTag = "Ball";
    [SerializeField] private string _bulletTag = "Bullet";
    [SerializeField] private string _phantomTag = "Phantom";

    [SerializeField] private float _freezeDuration = 2.0f;

    [SerializeField] private GameObject _gun;

    public int GunBullets = 0;

    public bool IsBallStuck = false;

    [SerializeField] private AudioClip _fire;
    [SerializeField] private AudioClip _bounceClip;
    [SerializeField] private AudioClip _deathClip;
    [SerializeField] private AudioClip _bulletHitClip;


    private GameObject _skin;

    public float SpeedMultiplication = 1.0f;

    public int Direction { get; set; } = 0;

    private Vector3 _startPos;
    private Vector3 _startScale;

    public bool IsControllable = true;
    public float BigTimer = 0;
    public float SmallTimer = 0;

    private Rigidbody2D _rb;
    private Ball _stuckBall;

    public RectTransform rectTransform;

    public bool isPlayerControl = true;


    private void Awake() {
        rectTransform = GetComponent<RectTransform>();

        //_startPos = transform.localPosition;
        _startPos = rectTransform.anchoredPosition;
        _startScale = transform.localScale;
        _rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable() {
        if (_skin != null) {
            Destroy(_skin);
        }

        if (_bounceDirection.x > 0) {
            _skin = Instantiate(SkinsManager.Instance.RacketSkins[SkinsManager.Instance.CurrentRacketL].Skin, transform);
        }
        else {
            _skin = Instantiate(SkinsManager.Instance.RacketSkins[SkinsManager.Instance.CurrentRacketR].Skin, transform);
        }
    }

    private void Update() {
        if (!IsControllable) {
            return;
        }
        
        Vector2 addPos = Direction * _speed * SpeedMultiplication * Time.deltaTime * _moveDirection;
        _rb.position += addPos;
        if (_stuckBall != null) {
            _stuckBall.transform.position += (Vector3)addPos;
        }

        Direction = 0;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (!collision.gameObject.CompareTag(_ballTag) && !collision.gameObject.CompareTag(_phantomTag)) {
            if (collision.gameObject.CompareTag(_bulletTag)) {
                IsControllable = false;
                Invoke(nameof(Unfreeze), _freezeDuration);
                Destroy(collision.gameObject);

                AudioSource.PlayClipAtPoint(_bulletHitClip, Camera.main.transform.position);
            }
            return;
        }
        /*if (GunBullets > 0) {
            return;
        }*/
        Ball ball = collision.gameObject.GetComponent<Ball>();
        if (ball.IsDeath) {
            gameObject.SetActive(false);

            Invoke(nameof(ResetRacket), 2.0f);

            GameManager.Instance.AddScore(GameManager.Instance.Sides.IndexOf(this) == 0 ? 1 : 0);

            AudioSource.PlayClipAtPoint(_deathClip, Camera.main.transform.position);

            return;
        }
        if (Vector2.Dot(ball.Velocity, _bounceDirection) > 0) {// || (ball.transform.position.x - transform.position.x) * _bounceDirection.x < _width) {
            return;
        }

        ball.HitRacket = this;

        Vector3 vel = _bounceDirection;
        vel.y += Direction;
        ball.AddVelocity(vel);
        ball.Attraction = null;

        if (ball.IsSticky && !IsBallStuck) {
            ball.IsControllable = false;
            ball.transform.parent = transform;
            _stuckBall = ball;
            IsBallStuck = true;
            Invoke(nameof(UnstickBall), 5);
        }

        AudioSource.PlayClipAtPoint(_bounceClip, Camera.main.transform.position);
    }

    private void UnstickBallTimer() {
        if (!IsBallStuck) {
            return;
        }
        UnstickBall();
    }

    private void Unfreeze() {
        IsControllable = true;
    }

    public void ResetRacket() {
        //transform.localPosition = _startPos;
        rectTransform.anchoredPosition = _startPos;
        transform.localScale = _startScale;
        gameObject.SetActive(true);
        enabled = true;
        SpeedMultiplication = 1.0f;
        IsControllable = true;
    }

    public void AddBullets(int amount) {
        GunBullets += amount;

        _gun.SetActive(true);
        ResetGunScale();
        //_skin.SetActive(false);
    }

    public void ResetGunScale() {
        _gun.transform.localScale = Vector3.one;
        _gun.transform.localScale = new Vector3(1.3f / _gun.transform.lossyScale.x, 1.3f / _gun.transform.lossyScale.y, 1.3f / _gun.transform.lossyScale.z);
    }

    public void FireBullet() {
        if (GunBullets <= 0) {
            return;
        }

        Bullet bullet = Instantiate(GameManager.Instance.BulletPrefab, transform.position + (Vector3)(_bounceDirection * 0.9f), Quaternion.identity, GameManager.Instance.FieldParent);
        bullet.MoveDirection = _bounceDirection;

        GunBullets--;

        if (GunBullets == 0) {
            HideGun();
        }

        AudioSource.PlayClipAtPoint(_fire, Camera.main.transform.position);
    }

    public void HideGun() {
        _gun.SetActive(false);
        //_skin.SetActive(true);
    }

    public void UnstickBall() {
        if (!IsBallStuck) {
            return;
        }
        Ball.Instance.IsSticky = false;
        Ball.Instance.IsControllable = true;
        Ball.Instance.ResetParent();
        IsBallStuck = false;
        _stuckBall = null;

        AudioSource.PlayClipAtPoint(_fire, Camera.main.transform.position);
    }

    public void OnDrag(PointerEventData eventData) {
        if (isPlayerControl) {
            Direction = eventData.delta.y > 0 ? 1 : -1;
            //print(Direction);
        }
    }

    public void OnBeginDrag(PointerEventData eventData) {
        _skin.GetComponent<Image>().color = new Color(0.7f, 0.7f, 0.7f);
    }
    public void OnEndDrag(PointerEventData eventData) {
        _skin.GetComponent<Image>().color = new Color(1f, 1f, 1f);
    }
}
