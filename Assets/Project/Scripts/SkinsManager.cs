using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkinsManager : MonoBehaviour {
    public int Coins = 0;

    public SkinData[] RacketSkins;
    public SkinData[] BallSkins;
    public SkinData[] BackgroundSkins;

    public int CurrentRacketL = 0;
    public int CurrentRacketR = 0;
    public int CurrentBall = 0;
    public int CurrentBack = 0;

    public static SkinsManager Instance;
    

    private void Awake() {
        Instance = this;

        Coins = PlayerPrefs.GetInt("Coins");

        CurrentRacketL = PlayerPrefs.GetInt("RacketSkinL");
        CurrentRacketR = PlayerPrefs.GetInt("RacketSkinR");
        CurrentBall = PlayerPrefs.GetInt("BallSkin");
        CurrentBack = PlayerPrefs.GetInt("BackSkin");

        for (int i = 0; i < RacketSkins.Length; i++) {
            RacketSkins[i].Bought = PlayerPrefs.GetInt("RacketSkin" + i, RacketSkins[i].Bought ? 1 : 0) > 0;
        }
        for (int i = 0; i < BallSkins.Length; i++) {
            BallSkins[i].Bought = PlayerPrefs.GetInt("BallSkin" + i, BallSkins[i].Bought ? 1 : 0) > 0;
        }
        for (int i = 0; i < BackgroundSkins.Length; i++) {
            BackgroundSkins[i].Bought = PlayerPrefs.GetInt("BackSkin" + i, BackgroundSkins[i].Bought ? 1 : 0) > 0;
        }
    }

    private void OnDestroy() {
        PlayerPrefs.SetInt("Coins", Coins);
        PlayerPrefs.SetInt("RacketSkinL", CurrentRacketL);
        PlayerPrefs.SetInt("RacketSkinR", CurrentRacketR);
        PlayerPrefs.SetInt("BallSkin", CurrentBall);
        PlayerPrefs.SetInt("BackSkin", CurrentBack);

        for (int i = 0; i < RacketSkins.Length; i++) {
            PlayerPrefs.SetInt("RacketSkin" + i, RacketSkins[i].Bought ? 1 : 0);
        }
        for (int i = 0; i < BallSkins.Length; i++) {
            PlayerPrefs.SetInt("BallSkin" + i, BallSkins[i].Bought ? 1 : 0);
        }
        for (int i = 0; i < BackgroundSkins.Length; i++) {
            PlayerPrefs.SetInt("BackSkin" + i, BackgroundSkins[i].Bought ? 1 : 0);
        }

        PlayerPrefs.Save();
    }

    public void BuyRacketSkin(int index) {
        if (Coins < RacketSkins[index].Price) {
            return;
        }

        Coins -= RacketSkins[index].Price;
        RacketSkins[index].Bought = true;
    }

    public void BuyBallSkin(int index) {
        if (Coins < BallSkins[index].Price) {
            return;
        }

        Coins -= BallSkins[index].Price;
        BallSkins[index].Bought = true;
    }

    public void BuyBackSkin(int index) {
        if (Coins < BackgroundSkins[index].Price) {
            return;
        }

        Coins -= BackgroundSkins[index].Price;
        BackgroundSkins[index].Bought = true;
    }

    public void UseRacketSkin(int index, bool isRight) {
        if (!RacketSkins[index].Bought) {
            return;
        }

        if (isRight) {
            CurrentRacketR = index;
        }
        else {
            CurrentRacketL = index;
        }
    }

    public void UseBallSkin(int index) {
        if (!BallSkins[index].Bought) {
            return;
        }

        CurrentBall = index;
    }

    public void UseBackSkin(int index) {
        if (!BackgroundSkins[index].Bought) {
            return;
        }

        CurrentBack = index;
    }
}

public enum SkinType {
    Background,
    Racket,
    Ball
}

[System.Serializable]
public struct SkinData {
    public string Name;
    public GameObject Skin;
    public int Price;
    public bool Bought;
    public SkinType Type;
}