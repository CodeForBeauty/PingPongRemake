using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
public class Ball : MonoBehaviour {
    [SerializeField] private float _speed = 5.0f;
    [SerializeField] private float _startingAngle = 65;
    [SerializeField] private float _bounciness = 2;

    private Vector3 _baseVelocity;
    private Vector3 _velocity;
    public Vector3 Velocity { get { return _velocity; } }

    public ControlRacket Attraction = null;

    public bool IsWaving = false;
    [SerializeField] private float _wavingStrength = 1;
    [SerializeField] private float _waveLength = 0.5f;

    public AudioClip BounceClip;

    public bool IsDeath = false;

    public bool IsSticky = false;

    public float SpeedMultiplier = 1.0f;

    public bool IsControllable = true;

    public bool IsHiden = false;

    public ControlRacket HitRacket;


    private GameObject _skin;
    private Vector3 _startScale;
    private Vector3 _startPos;
    private Transform _startParent;

    public static Ball Instance;

    public bool IsScaled = false;
    public float WavingTime = 0;
    public float HideTime = 0;

    private Dictionary<string, GameObject> _effects = new();


    private void Awake() {
        _startScale = transform.localScale;
        _startPos = transform.position;
        _startParent = transform.parent;
        if (Instance == null) {
            Instance = this;
        }
    }

    private void Update() {
        if (!IsControllable) {
            return;
        }
        _velocity = _baseVelocity;
        if (Attraction != null) {
            _velocity = Attraction.transform.position - transform.position;
            _velocity.Normalize();
        }
        if (IsWaving) {
            _velocity += Vector3.Cross(_velocity, Vector3.forward * Mathf.Sin(Time.time * _waveLength)) * _wavingStrength;
            //_velocity.y += Mathf.Sin(Time.time * _waveLength) * _wavingStrength;
            _velocity.Normalize();
        }
        transform.position += _speed * SpeedMultiplier * Time.deltaTime * _velocity;
    }

    private void OnEnable() {
        transform.position = _startPos;

        if (_skin != null) {
            Destroy(_skin);
        }

        _skin = Instantiate(SkinsManager.Instance.BallSkins[SkinsManager.Instance.CurrentBall].Skin, transform);

        if (Instance != this) {
            _skin.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
            GameManager.Instance.PhantomBalls.Add(this);
        }

        float radians = Mathf.Deg2Rad * Random.Range(-_startingAngle, _startingAngle);
        int side = Random.Range(0, 2);
        HitRacket = GameManager.Instance.Sides[side];
        side = side == 0 ? -1 : 1;

        _baseVelocity = new Vector3(Mathf.Sin(radians) * side, Mathf.Cos(radians));
    }

    public void AddVelocity(Vector3 direction) {
        _baseVelocity += direction * _bounciness;
        _baseVelocity.Normalize();
    }

    public void ResetBall() {
        SpeedMultiplier = 1.0f;
        transform.localScale = _startScale;
        SetVisibility(true);
        Attraction = null;
        IsWaving = false;
        IsDeath = false;
        IsSticky = false;
        IsControllable = true;
        IsScaled = false;

        WavingTime = 0;
        HideTime = 0;

        for (int i = 0; i < transform.childCount; i++) {
            GameObject child = transform.GetChild(i).gameObject;
            if (child != _skin) {
                Destroy(child);
            }
        }

        if (HitRacket == null) {
            return;
        }
        Vector3 pos = transform.localPosition;
        //pos.x = GameManager.Instance.Sides[GameManager.Instance.Sides.IndexOf(HitRacket)].transform.localPosition.x;
        pos.x = HitRacket.transform.localPosition.x;
        transform.localPosition = pos;

        _effects.Clear();
    }

    public void SetVisibility(bool visible) {
        if (_skin != null) {
            _skin.SetActive(visible);
            IsHiden = !visible;
        }
    }

    public void SkinVisibility(bool visible) {
        if (_skin != null) {
            _skin.SetActive(visible);
        }
    }

    public void ResetParent() {
        transform.parent = _startParent;

        foreach (GameObject obj in _effects.Values) {
            Destroy(obj);
        }
        _effects.Clear();
    }

    public void AddEffect(string effect, GameObject prefab) {
        if (!_effects.ContainsKey(effect)) {
            _effects.Add(effect, Instantiate(prefab, transform));
        }
    }

    public void RemoveEffect(string effect) {
        if (_effects.ContainsKey(effect)) {
            _effects.Remove(effect);
        }
    }
}
