using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameSettings : MonoBehaviour {
    public int WinScore { get; set; } = 9;
    public bool HasBonuses { get; set; } = true;
    public bool[] BonusEnabled = new bool[26];

    [SerializeField] private TMP_InputField _winScoreText;
    [SerializeField] private Toggle _hasBonusesToggle;
    [SerializeField] private List<Toggle> _bonusToggles;

    [SerializeField] private MenuBonusToggle _menuBonusPrefab;
    [SerializeField] private Transform _bonusesParent;

    public static GameSettings Instance;


    private void Awake() {
        Instance = this;
    }

    private void Start() {
        WinScore = PlayerPrefs.GetInt("WinScore", WinScore);
        HasBonuses = PlayerPrefs.GetInt("HasBonuses", HasBonuses ? 1 : 0) > 0;
        for (int i = 0; i < BonusEnabled.Length; i++) {
            bool value = BonusEnabled[i] = PlayerPrefs.GetInt("BonusEnabled" + i, 1) > 0;

            MenuBonusToggle toggle = Instantiate(_menuBonusPrefab, _bonusesParent);
            Sprite image = GameManager.Instance.Bonuses[i].SkinPrefab.sprite;
            toggle.Init(GameManager.Instance.Bonuses[i].BonusName + " - " + GameManager.Instance.Bonuses[i].BonusDesc, value, image);
            _bonusToggles.Add(toggle._toggle);

            _bonusToggles[i].isOn = value;
            int j = i;
            _bonusToggles[i].onValueChanged.AddListener((value) => SetBonus(j));
        }

        _winScoreText.text = WinScore.ToString();
        _hasBonusesToggle.isOn = HasBonuses;

        _winScoreText.onValueChanged.AddListener((value) => WinScore = int.Parse(value));
        _hasBonusesToggle.onValueChanged.AddListener((value) => HasBonuses = value);
    }

    private void OnDestroy() {
        PlayerPrefs.SetInt("WinScore", WinScore);
        PlayerPrefs.SetInt("HasBonuses", HasBonuses ? 1 : 0);
        for (int i = 0; i < BonusEnabled.Length; i++) {
            PlayerPrefs.SetInt("BonusEnabled" + i, BonusEnabled[i] ? 1 : 0);
        }
        PlayerPrefs.Save();
    }

    public void SetBonus(int index) {
        BonusEnabled[index] = _bonusToggles[index].isOn;
    }

    public void QuitGame() {
        Application.Quit();
    }
}
