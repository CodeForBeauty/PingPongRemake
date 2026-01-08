using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusSpawner : MonoBehaviour {
    public List<Bonus> OpenBonuses;

    [SerializeField] private Vector2 _spawnArea;

    [SerializeField] private float _spawnTime = 5;
    [SerializeField] private float _aliveTime = 10;

    public static BonusSpawner Instance;

    private void Awake() {
        Instance = this;
    }

    private void OnEnable() {
        OpenBonuses.Clear();

        for (int i = 0; i < GameSettings.Instance.BonusEnabled.Length; i++) {
            if (GameSettings.Instance.BonusEnabled[i]) {
                OpenBonuses.Add(GameManager.Instance.Bonuses[i]);
            }
        }

        StopAllCoroutines();
        StartCoroutine(StartSpawn());
    }

    private IEnumerator StartSpawn() {
        while (true) {
            if (!enabled) {
                continue;
            }
            yield return new WaitForSeconds(_spawnTime);

            int index = Random.Range(0, OpenBonuses.Count);
            Vector3 pos = new Vector3(Random.Range(-_spawnArea.x, _spawnArea.x), Random.Range(-_spawnArea.y, _spawnArea.y));
            GameBonus bonus = Instantiate(GameManager.Instance.BonusPrefab, pos, Quaternion.identity, GameManager.Instance.FieldParent);
            bonus.Init(OpenBonuses[index], _aliveTime);
        }
    }

    private void OnDestroy() {
        for (int i = 0; i < GameManager.Instance.Bonuses.Count; i++) {
            if (OpenBonuses.Contains(GameManager.Instance.Bonuses[i])) {
                PlayerPrefs.SetInt("BonusEnabled" + i, 1);
            }
        }
    }

    public void SetBonus(bool value, int index) {
        if (value) {
            OpenBonuses.Add(GameManager.Instance.Bonuses[index]);
        }
        else {
            OpenBonuses.Remove(GameManager.Instance.Bonuses[index]);
        }
    }
}
