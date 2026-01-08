using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour {
    [SerializeField] private int _winScore = 9;
    public List<ControlRacket> Sides = new();
    public int[] CurrentScores = new int[2];
    public List<BouncyWall> Walls = new();
    private List<Vector3> _wallPos = new();

    public List<Bonus> Bonuses = new();
    public Bullet BulletPrefab;
    public GameBonus BonusPrefab;
    public BonusWall WallPrefab;
    public Ball BallPrefab;

    public GameObject BonusWall;

    public Transform FieldParent;

    public UnityEvent OnChange = new();

    private GameObject _background;
    [SerializeField] private Transform _backParent;

    [SerializeField] private GameObject _endScreen;
    [SerializeField] private TextMeshProUGUI _endScreenText;

    [SerializeField] private float _wallCloseSpeed = 0.2f; 


    public static GameManager Instance;

    public bool IsClosing = false;
    public List<Ball> PhantomBalls = new();


    private void Awake() {
        Instance = this;
        for (int i = 0; i < Walls.Count; i++) {
            //_wallPos.Add(Walls[i].transform.localPosition);
            //_wallPos.Add(Walls[i].transform.position);
            _wallPos.Add(Walls[i].rectTransform.anchoredPosition);
        }
    }

    private void Update() {
        if (!IsClosing) {
            return;
        }
        for (int i = 0; i < Walls.Count; i++) {
            Walls[i].transform.position += _wallCloseSpeed * Time.deltaTime * Walls[i].BounceDirection;
        }
    }

    public void StartGame() {
        if (_background != null) {
            Destroy(_background);
        }

        _background = Instantiate(SkinsManager.Instance.BackgroundSkins[SkinsManager.Instance.CurrentBack].Skin, _backParent);

        _winScore = GameSettings.Instance.WinScore;
        BonusSpawner.Instance.enabled = GameSettings.Instance.HasBonuses;

        for (int i = 0; i < CurrentScores.Length; i++) {
            CurrentScores[i] = 0;
            Sides[i].GunBullets = 0;
            Sides[i].HideGun();
        }
        BonusSpawner.Instance.enabled = true;

        CleanLevel();

        ResetLevel();
    }


    public void AddScoreNoReset(int index) {
        if (++CurrentScores[index] >= _winScore) {
            Ball.Instance.IsControllable = false;
            foreach (ControlRacket racket in Sides) {
                racket.IsControllable = false;
            }
            _endScreen.SetActive(true);
            _endScreenText.text = $"Player {index + 1} won!";
        }
        OnChange.Invoke();
    }

    public void AddScore(int index) {
        Ball.Instance.HitRacket = Sides[index];
        AddScoreNoReset(index);
        ResetLevel();
    }

    public void SubtractScore(int index) {
        if (CurrentScores[index] == 0) {
            return;
        }
        CurrentScores[index]--;
        OnChange.Invoke();
    }

    public void CleanLevel() {
        for (int i = 0; i < FieldParent.childCount; i++) {
            Destroy(FieldParent.GetChild(i).gameObject);
        }
    }

    public void ResetLevel() {
        Ball.Instance.ResetBall();
        foreach (ControlRacket wall in Sides) {
            wall.ResetRacket();
        }
        for (int i = 0; i < Walls.Count; i++) {
            //Walls[i].transform.localPosition = _wallPos[i];
            //Walls[i].transform.position = _wallPos[i];
            Walls[i].rectTransform.anchoredPosition = _wallPos[i];
        }
        foreach (Ball ball in PhantomBalls) {
            Destroy(ball.gameObject);
        }
        PhantomBalls.Clear();
        IsClosing = false;
        OnChange.Invoke();
    }
}
