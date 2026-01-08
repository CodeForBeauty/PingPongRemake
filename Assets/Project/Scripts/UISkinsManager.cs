using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISkinsManager : MonoBehaviour {
    [SerializeField] private UISkin _prefab;

    [SerializeField] private Transform _content;

    [SerializeField] private SkinType _type;

    private List<UISkin> _skins = new();

    [SerializeField] private bool _isRight;

    private void Start() {
        SkinData[] datas = null;
        switch (_type) {
            case SkinType.Ball:
                datas = SkinsManager.Instance.BallSkins;
                break;
            case SkinType.Background:
                datas = SkinsManager.Instance.BackgroundSkins;
                break;
            case SkinType.Racket:
                datas = SkinsManager.Instance.RacketSkins;
                break;
        }

        for (int i = 0; i < datas.Length; i++) {
            SkinData data = datas[i];
            UISkin skin = Instantiate(_prefab, _content);
            skin.Init(data, i, _isRight, this);
            _skins.Add(skin);
        }
    }

    public void UpdateUI() {
        for (int i = 0; i < _skins.Count; i++) {
            _skins[i].UpdateUI();
        }
    }
}
