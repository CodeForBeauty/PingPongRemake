using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UISkin : MonoBehaviour {
    [SerializeField] private Button _selectBtn;
    [SerializeField] private Button _buyBtn;
    [SerializeField] private TextMeshProUGUI _buyPriceTag;
    [SerializeField] private Transform _skinPlace;

    private int _skinIndex = -1;

    private SkinType _type;

    private bool _isRight = false;

    private UISkinsManager _manager;

    private void Start() {
        _buyBtn.onClick.AddListener(BuySkin);
        _selectBtn.onClick.AddListener(SelectSkin);
    }

    private void OnEnable() {
        UpdateUI();
    }

    public void UpdateUI() {
        if (_skinIndex == -1) {
            return;
        }

        bool isBought = false;
        switch (_type) {
            case SkinType.Ball:
                isBought = SkinsManager.Instance.BallSkins[_skinIndex].Bought;
                break;
            case SkinType.Background:
                isBought = SkinsManager.Instance.BackgroundSkins[_skinIndex].Bought;
                break;
            case SkinType.Racket:
                isBought = SkinsManager.Instance.RacketSkins[_skinIndex].Bought;
                break;
        }

        if (isBought) {
            _selectBtn.gameObject.SetActive(true);
            _buyBtn.gameObject.SetActive(false);
        }
        else {
            _selectBtn.gameObject.SetActive(false);
            _buyBtn.gameObject.SetActive(true);
        }

        switch (_type) {
            case SkinType.Ball:
                _selectBtn.interactable = SkinsManager.Instance.CurrentBall != _skinIndex;
                break;
            case SkinType.Background:
                _selectBtn.interactable = SkinsManager.Instance.CurrentBack != _skinIndex;
                break;
            case SkinType.Racket:
                if (_isRight) {
                    _selectBtn.interactable = SkinsManager.Instance.CurrentRacketR != _skinIndex;
                }
                else {
                    _selectBtn.interactable = SkinsManager.Instance.CurrentRacketL != _skinIndex;
                }
                break;
        }
    }

    public void Init(SkinData skin, int index, bool isRight, UISkinsManager manager) {
        _buyPriceTag.text = skin.Price.ToString();

        if (skin.Bought) {
            _selectBtn.gameObject.SetActive(true);
            _buyBtn.gameObject.SetActive(false);
        }
        else {
            _selectBtn.gameObject.SetActive(false);
            _buyBtn.gameObject.SetActive(true);
        }

        _skinIndex = index;

        _type = skin.Type;

        _isRight = isRight;

        _manager = manager;

        RectTransform skinUI = Instantiate(skin.Skin, _skinPlace).GetComponent<RectTransform>();
        skinUI.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 1.5f);
        skinUI.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 1.5f);

        UpdateUI();
    }

    public void BuySkin() {
        switch (_type) {
            case SkinType.Ball:
                SkinsManager.Instance.BuyBallSkin(_skinIndex);
                break;
            case SkinType.Background:
                SkinsManager.Instance.BuyBackSkin(_skinIndex);
                break;
            case SkinType.Racket:
                SkinsManager.Instance.BuyRacketSkin(_skinIndex);
                break;
        }

        _manager.UpdateUI();
    }

    public void SelectSkin() {
        switch (_type) {
            case SkinType.Ball:
                SkinsManager.Instance.UseBallSkin(_skinIndex);
                break;
            case SkinType.Background:
                SkinsManager.Instance.UseBackSkin(_skinIndex);
                break;
            case SkinType.Racket:
                SkinsManager.Instance.UseRacketSkin(_skinIndex, _isRight);
                break;
        }

        _manager.UpdateUI();
    }
}
