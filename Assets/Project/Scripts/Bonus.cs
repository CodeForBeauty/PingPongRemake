using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Bonus", menuName = "Pong Bonuses/Bonus Base")]
public class Bonus : ScriptableObject {
    public string BonusName = "Bonus";
    public string BonusDesc = "";
    public UnityEvent OnHit = new();
    public SpriteRenderer SkinPrefab;
    public AudioClip ActivateSound;
    public GameObject BallEffect;

    public Ball HitBall;

    public void BonusSpeed(float add) {
        HitBall.SpeedMultiplier += add;
    }

    public void BonusScaleBall(float multiply) {
        if (!HitBall.IsScaled) {
            HitBall.transform.localScale *= multiply;
            HitBall.IsScaled = true;
        }
    }

    public void BonusMagnet() {
        if (HitBall.Attraction != null) {
            return;
        }
        int index = GameManager.Instance.Sides.IndexOf(HitBall.HitRacket);
        index = index == 0 ? 1 : 0;
        HitBall.Attraction = GameManager.Instance.Sides[index];
    }

    public void BonusHideBall(float duration) {
        HitBall.StartCoroutine(HideBall(duration));
    }
    private IEnumerator HideBall(float time) {
        HitBall.SetVisibility(false);

        HitBall.HideTime += time;

        if (HitBall.IsHiden) {
            yield break;
        }
        Ball currBall = HitBall;
        while (currBall.HideTime > 0) {
            yield return new WaitForSeconds(1);
            currBall.HideTime -= 1;
        }
        currBall.HideTime = 0;

        currBall.SetVisibility(true);
    }

    public void BonusTeleportVert(float amount) {
        HitBall.transform.position += Vector3.up * amount;
    }

    public void BonusWave(float duration) {
        HitBall.StartCoroutine(StopWave(duration));
    }
    private IEnumerator StopWave(float time) {
        HitBall.WavingTime += time;
        if (HitBall.IsWaving) {
            yield break;
        }
        HitBall.IsWaving = true;

        Ball currBall = HitBall;
        while (currBall.WavingTime > 0) {
            yield return new WaitForSeconds(1);
            currBall.WavingTime -= 1;
        }
        currBall.IsWaving = false;
        currBall.WavingTime = 0;
    }

    public void BonusDeath() {
        HitBall.IsDeath = true;
    }

    public void BonusResetLevel() {
        GameManager.Instance.ResetLevel();
        GameManager.Instance.CleanLevel();
    }

    public void BonusRacketSize(float add) {
        if (add > 0) {
            HitBall.HitRacket.StartCoroutine(ResizeRacketBig(add, HitBall.HitRacket));
        }
        else {
            HitBall.HitRacket.StartCoroutine(ResizeRacketSmall(add, HitBall.HitRacket));
        }
    }
    private IEnumerator ResizeRacketBig(float add, ControlRacket racket) {
        racket.BigTimer += 10;
        if (racket.BigTimer > 10) {
            yield break;
        }
        racket.transform.localScale += Vector3.up * add;
        racket.ResetGunScale();

        while (racket.BigTimer > 0) {
            yield return new WaitForSeconds(1);
            racket.BigTimer -= 1;
        }

        racket.transform.localScale -= Vector3.up * add;
        racket.ResetGunScale();
    }
    private IEnumerator ResizeRacketSmall(float add, ControlRacket racket) {
        racket.SmallTimer += 10;
        if (racket.SmallTimer > 10) {
            yield break;
        }
        racket.transform.localScale += Vector3.up * add;
        racket.ResetGunScale();

        while (racket.SmallTimer > 0) {
            yield return new WaitForSeconds(1);
            racket.SmallTimer -= 1;
        }

        racket.transform.localScale -= Vector3.up * add;
        racket.ResetGunScale();
    }


    public void BonusRacketSpeed(float multiply) {
        float speed = HitBall.HitRacket.SpeedMultiplication;
        if ((speed <= 1.0f && multiply > 1.0f) || (speed >= 1.0f && multiply < 1.0f)) {
            HitBall.HitRacket.StartCoroutine(SpeedRacket(multiply, 10));
        }
    }
    private IEnumerator SpeedRacket(float multiply, float duration) {
        HitBall.HitRacket.SpeedMultiplication *= multiply;

        yield return new WaitForSeconds(duration);

        HitBall.HitRacket.SpeedMultiplication = 1;
    }

    public void BonusGun(int bullets) {
        HitBall.HitRacket.AddBullets(bullets);
    }

    public void BonusBullet() {
        Bullet bullet = Instantiate(GameManager.Instance.BulletPrefab, HitBall.transform.position, Quaternion.identity, GameManager.Instance.FieldParent);
        bullet.MoveDirection.x = HitBall.Velocity.x;
        bullet.MoveDirection.Normalize();
    }

    public void BonusCreateWall(float posx) {
        Vector3 pos = HitBall.transform.position;
        pos.y = 0;
        pos.x += posx;
        if (GameManager.Instance.BonusWall != null) {
            Destroy(GameManager.Instance.BonusWall);
        }
        GameManager.Instance.BonusWall = Instantiate(GameManager.Instance.WallPrefab, pos, Quaternion.identity, GameManager.Instance.FieldParent).gameObject;
    }

    public void BonusBounceH() {
        Vector3 bounce = HitBall.Velocity;
        bounce.x = 0;
        bounce.Normalize();
        HitBall.AddVelocity(-bounce);

        AudioSource.PlayClipAtPoint(HitBall.BounceClip, Camera.main.transform.position);
    }

    public void BonusBounceV() {
        Vector3 bounce = HitBall.Velocity;
        bounce.y = 0;
        bounce.Normalize();
        HitBall.AddVelocity(-bounce);

        if (HitBall.Attraction != null) {
            HitBall.Attraction = GameManager.Instance.Sides.Find(side => side != HitBall.Attraction);
        }

        AudioSource.PlayClipAtPoint(HitBall.BounceClip, Camera.main.transform.position);
    }

    public void BonusChangeScore(int amount) {
        int index = GameManager.Instance.Sides.IndexOf(HitBall.HitRacket);
        if (amount > 0) {
            GameManager.Instance.AddScoreNoReset(index);
        }
        else {
            GameManager.Instance.SubtractScore(index);
        }
    }

    public void BonusRandom() {
        if (BonusSpawner.Instance.OpenBonuses.Count <= 1) {
            return;
        }
        int index;
        while (true) {
            index = Random.Range(0, BonusSpawner.Instance.OpenBonuses.Count);
            if (BonusSpawner.Instance.OpenBonuses[index] != this) {
                break;
            }
        }

        BonusSpawner.Instance.OpenBonuses[index].HitBall = HitBall;
        BonusSpawner.Instance.OpenBonuses[index].OnHit.Invoke();
    }

    public void BonusClosing(float maxTime) {
        if (!GameManager.Instance.IsClosing) {
            GameManager.Instance.StartCoroutine(ClosingWalls(maxTime));
        }
    }
    private IEnumerator ClosingWalls(float time) {
        GameManager.Instance.IsClosing = true;

        yield return new WaitForSeconds(time);

        GameManager.Instance.ResetLevel();
    }

    public void BonusPhantoms(int amount) {
        Vector3 dir = HitBall.Velocity;
        dir.y = 0;
        dir.Normalize();
        for (int i = 0; i < amount; i++) {
            Ball ball = Instantiate(GameManager.Instance.BallPrefab, HitBall.transform.position, Quaternion.identity, GameManager.Instance.FieldParent);
            ball.AddVelocity(dir);
        }
    }

    public void BonusCoin(int amount) {
        SkinsManager.Instance.Coins += amount;
    }

    public void BonusSwapScore() {
        int first = GameManager.Instance.CurrentScores[0];
        GameManager.Instance.CurrentScores[0] = GameManager.Instance.CurrentScores[1];
        GameManager.Instance.CurrentScores[1] = first;
        GameManager.Instance.OnChange.Invoke();
    }

    public void BonusStickyBall() {
        HitBall.IsSticky = true;
    }

    public void ChangeBallForRound() {
        Instantiate(BallEffect, HitBall.transform);
        HitBall.SkinVisibility(false);
    }

    public void ChangeBallForSeconds(float duration) {
        HitBall.StartCoroutine(BallAddEffect(duration));
    }
    private IEnumerator BallAddEffect(float duration) {
        GameObject obj = Instantiate(BallEffect, HitBall.transform);
        HitBall.SkinVisibility(false);

        yield return new WaitForSeconds(duration);

        Destroy(obj);
        HitBall.SkinVisibility(true);
    }

    public void AddBallEffect() {
        HitBall.AddEffect(BonusName, BallEffect);
    }

    public void AddBallEffectForTime(float time) {
        HitBall.StartCoroutine(AddBallEffectTimed(time));
    }

    private IEnumerator AddBallEffectTimed(float time) {
        HitBall.AddEffect(BonusName, BallEffect);

        yield return new WaitForSeconds(time);

        HitBall.RemoveEffect(BonusName);
    }

    public void AddEffect() {
        Instantiate(BallEffect, HitBall.transform.position, Quaternion.identity, HitBall.transform.parent);
    }
}
