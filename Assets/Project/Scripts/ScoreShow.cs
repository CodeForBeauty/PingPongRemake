using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class ScoreShow : MonoBehaviour {
    [SerializeField] private int _index;

    private TextMeshProUGUI _text;

    private void Awake() {
        _text = GetComponent<TextMeshProUGUI>();
    }

    private void Start() {
        GameManager.Instance.OnChange.AddListener(UpdateScore);
    }

    private void UpdateScore() {
        _text.text = GameManager.Instance.CurrentScores[_index].ToString();
    }
}
