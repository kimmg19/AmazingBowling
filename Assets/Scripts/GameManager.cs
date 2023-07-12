using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    public static GameManager Instance;
    public GameObject readyPanel;
    public Text scoreText;
    public Text bestScoreText;
    public Text messageText;
    public bool isRoundActive = false;
    private int score = 0;
    public ShooterRotator shooterRotator;
    public CamFollow cam;
    public UnityEvent onReset;

    private void Awake() {
        Instance = this;
        UpdateUi();
    }
    private void Start() {
        StartCoroutine("RoundRoutine");
    }
    public void AddScore(int newScore) {
        score += newScore;
        UpdateBestScore();
        UpdateUi();
    }
    void UpdateBestScore() {
        if (GetBestScore() < score)
            PlayerPrefs.SetInt("BestScore", score);
    }
    int GetBestScore() {
        int bestScore = PlayerPrefs.GetInt("BestScore");
        return bestScore;
    }
    void UpdateUi() {
        scoreText.text = "Score: " + score;
        bestScoreText.text = "Best Score: " + GetBestScore();
    }
    public void OnBallDestroy() {
        UpdateUi();
        isRoundActive = false;
    }
    public void Reset() {
        score = 0;
        UpdateUi();
        StartCoroutine("RoundRoutine");
    }
    IEnumerator RoundRoutine() {
        //Ready
        onReset.Invoke();
        readyPanel.SetActive(true);
        cam.SetTarget(shooterRotator.transform, CamFollow.State.Idle);
        shooterRotator.enabled = false;
        isRoundActive = false;
        messageText.text = "Ready...";
        yield return new WaitForSeconds(3f);

        //Play
        readyPanel.SetActive(false);
        isRoundActive = true;
        shooterRotator.enabled = true;
        cam.SetTarget(shooterRotator.transform, CamFollow.State.Ready);
        while (isRoundActive == true) {
            yield return null;
        }
        yield return new WaitForSeconds(1f);

        //End
        readyPanel.SetActive(true);
        shooterRotator.enabled = false;
        messageText.text = "Ready For Next Round...";

        yield return new WaitForSeconds(3f);
        Reset();
    }
}
