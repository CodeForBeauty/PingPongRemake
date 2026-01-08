using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShowCoins : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI _text;

    private void Update() {
        _text.text = SkinsManager.Instance.Coins.ToString();
    }
}
