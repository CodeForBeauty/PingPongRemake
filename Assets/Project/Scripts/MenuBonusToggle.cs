using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuBonusToggle : MonoBehaviour {
    public Toggle _toggle;
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private Image _image;

    public void Init(string text, bool value, Sprite image) {
        _toggle.isOn = value;
        _text.text = text;
        _image.sprite = image;
    }
}
