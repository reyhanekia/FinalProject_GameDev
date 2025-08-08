using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public enum BoostType
{
    DoublePoints,
    FastBall,
    InvertedControl
}

public class LevelProgression : MonoBehaviour
{
    public static LevelProgression current;

    [Header("UI Elements")]
    public TextMeshProUGUI scoreDisplay;
    public Image background;
    public TextMeshProUGUI boostTimer;

    [Header("Visual Themes")]
    public Sprite stage1Background;
    public Sprite stage2Background;
    public Sprite stage3Background;

    [Header("Audio Themes")]
    public AudioSource bgmSource;
    public AudioClip track1;
    public AudioClip track2;
    public AudioClip track3;

    [Header("Game Elements")]
    public HealthManager lifeSystem;
    public BallSkinLoader skinChanger;

    [Header("Pause System")]
    public GameObject pausePanel;
    public GameObject[] gameplayHandlers;

    [Header("Ball Control")]
    public BallController controlledBall;

    private int currentScore = 0;
    private bool stage2Reached, stage3Reached;

    private float baseJumpForce;

    private Dictionary<BoostType, Coroutine> activeBoosts = new Dictionary<BoostType, Coroutine>();

    void Awake()
    {
        current = this;
    }

    void Start()
    {
        background.sprite = stage1Background;
        bgmSource.clip = track1;
        bgmSource.Play();

        Application.targetFrameRate = 60;
        if (pausePanel != null) pausePanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void IncreaseScore(int amount)
    {
        if (activeBoosts.ContainsKey(BoostType.DoublePoints)) amount *= 2;

        currentScore += amount;
        scoreDisplay.text = "امتیاز: " + currentScore;

        if (currentScore >= 20 && !stage2Reached) AdvanceToStage2();
        if (currentScore >= 40 && !stage3Reached) AdvanceToStage3();
    }

    public int GetCurrentScore() => currentScore;

    void AdvanceToStage2()
    {
        stage2Reached = true;
        background.sprite = stage2Background;
        bgmSource.clip = track2;
        bgmSource.Play();
        skinChanger?.ApplySkin(1);
    }

    void AdvanceToStage3()
    {
        stage3Reached = true;
        background.sprite = stage3Background;
        bgmSource.clip = track3;
        bgmSource.Play();
        skinChanger?.ApplySkin(2);
    }

    public void PauseGameplay()
    {
        Time.timeScale = 0f;
        if (pausePanel != null) pausePanel.SetActive(true);
        foreach (var obj in gameplayHandlers)
            obj.SetActive(false);
    }

    public void ResumeGameplay()
    {
        Time.timeScale = 1f;
        if (pausePanel != null) pausePanel.SetActive(false);
        foreach (var obj in gameplayHandlers)
            obj.SetActive(true);
    }

    public void ActivateBoost(BoostType boost, float time)
    {
        if (activeBoosts.ContainsKey(boost))
        {
            StopCoroutine(activeBoosts[boost]);
            DeactivateBoost(boost);
        }

        Coroutine newBoost = StartCoroutine(HandleBoost(boost, time));
        activeBoosts[boost] = newBoost;
    }

    private IEnumerator HandleBoost(BoostType boost, float time)
    {
        string boostName = GetBoostName(boost);
        float remainingTime = time;

        switch (boost)
        {
            case BoostType.DoublePoints:
                break;

            case BoostType.FastBall:
                if (controlledBall != null)
                {
                    baseJumpForce = controlledBall.ForceMultiplier;
                    controlledBall.ForceMultiplier *= 4f;
                }
                break;

            case BoostType.InvertedControl:
                if (controlledBall != null)
                    controlledBall.SetInvertedControl(true);
                break;
        }

        while (remainingTime > 0)
        {
            if (boostTimer != null && activeBoosts.Count == 1)
                boostTimer.text = $"{boostName}: {Mathf.CeilToInt(remainingTime)} ثانیه";

            yield return new WaitForSeconds(1f);
            remainingTime -= 1f;
        }

        DeactivateBoost(boost);
        activeBoosts.Remove(boost);

        if (activeBoosts.Count == 0 && boostTimer != null)
            boostTimer.text = "";

    }

    private void DeactivateBoost(BoostType boost)
    {
        switch (boost)
        {
            case BoostType.DoublePoints:
                break;

            case BoostType.FastBall:
                if (controlledBall != null)
                    controlledBall.ForceMultiplier = baseJumpForce;
                break;

            case BoostType.InvertedControl:
                if (controlledBall != null)
                    controlledBall.SetInvertedControl(false);
                break;
        }

        if (activeBoosts.Count == 0 && boostTimer != null)
            boostTimer.text = "";
    }

    private string GetBoostName(BoostType boost)
    {
        switch (boost)
        {
            case BoostType.DoublePoints:
                return "دو امتیازی";
            case BoostType.FastBall:
                return "توپ سریع";
            case BoostType.InvertedControl:
                return "کنترل برعکس";
            default:
                return "قدرت ناشناخته";
        }
    }
}
