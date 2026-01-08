using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class PlayVideo : MonoBehaviour {
    [SerializeField] private VideoPlayer _player;
    [SerializeField] private GameObject _placeholder;


    private void OnEnable() {
        _player.Prepare();
        _placeholder.SetActive(true);
        _player.prepareCompleted += (v) => { 
            _player.Play();
            _placeholder.SetActive(false);
        };
    }
}
