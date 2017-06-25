using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : SingletonBase<UIManager>
{
    public static readonly int GAME_STARTS_IN_TIME = 4;

    public Text gameStartsInText;
    public Text startText;
    public Text endText;
    public Image brightnessImg;
    public Text scoreText;

    private bool _debugMode = false;
    public bool DebugMode { get { return _debugMode; } set { _debugMode = value; } }


    private void Start ()
    {
        if (gameStartsInText != null)
            gameStartsInText.enabled = false;   // Just to make sure its hidden
        else
            Debug.LogError("GameStartsInText is not set.");

        if (startText != null)
            startText.enabled = false;   // Just to make sure its hidden
        else
            Debug.LogError("StartText is not set.");

        if (endText != null)
            endText.enabled = false;   // Just to make sure its hidden
        else
            Debug.LogError("EndText is not set.");

        if (brightnessImg != null)
            brightnessImg.enabled = false;   // Just to make sure its hidden
        else
            Debug.LogError("BrightnessImg is not set.");

        if (scoreText != null)
            scoreText.enabled = false;   // Just to make sure its hidden
        else
            Debug.LogError("ScoreText is not set.");
    }

    /// <summary>
    /// Only used for debuging
    /// </summary>
    private void Update()
    {
        if (!_debugMode)
            return;

        brightnessImg.enabled = true;
        scoreText.enabled = true;

        if (GameManager.Instance.Arduino.Brightness < GameManager.Instance.Arduino.EnvironmentLight)
            brightnessImg.color = Color.white;
        else
            brightnessImg.color = Color.black;

        scoreText.text = ScoringManager.Instance.Score.ToString();
    }

    public void PlayGameStartsAnimation(System.Action callback)
    {
        StartCoroutine(TextAnimationGameStartsIn(callback));
    }

    public void ShowGameOverText()
    {
        SetEndText();
    }

    #region Text Settings

    private void SetGameStartsInText(int value)
    {
        gameStartsInText.text = "Game starts in " + value;
        gameStartsInText.enabled = true;
    }

    private void SetStartText()
    {
        startText.text = "Go!";
        startText.enabled = true;
    }

    private void SetEndText()
    {
        endText.text = "Game Over!";
        endText.enabled = true;
    }

    #endregion

    #region Coroutines

    private IEnumerator TextAnimationGameStartsIn(System.Action callback)
    {
        for (int i = GAME_STARTS_IN_TIME; i > 0; i--)
        {
            SetGameStartsInText(i);
            yield return new WaitForSeconds(1);
        }

        // Switch texts
        gameStartsInText.enabled = false;
        SetStartText();

        yield return new WaitForSeconds(1);
        startText.enabled = false;

        if (callback != null)
            callback();
    }

    #endregion
}
