using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonBase<GameManager>
{
    public static readonly bool DEBUG_MODE = false;

    private ArduinoBase _arduino;
    public ArduinoBase Arduino { get { return _arduino; } }

    private float _screenHeight = 0;
    private float _screenWidth = 0;

    public enum SCREEN_AXIS { X, Y };


    private void Start ()
    {
        //_arduino = gameObject.AddComponent<ArduinoMock>();
        _arduino = gameObject.AddComponent<ArduinoConnector>();

        // Get Screen values
        Vector2 topRightCorner = new Vector2(1, 1);
        Vector2 edgeVector = Camera.main.ViewportToWorldPoint(topRightCorner);  // center is at (0,0)
        _screenHeight = edgeVector.y * 2;
        _screenWidth = edgeVector.x * 2;

        // Init ui
        UIManager.Instance.DebugMode = DEBUG_MODE;
        UIManager.Instance.PlayGameStartsAnimation(StartGame);
    }

    /// <summary>
    /// This callback function is called when all start animations have been finished.
    /// </summary>
    public void StartGame() 
    {
        SceneManager.Instance.StartObstracleSpawner();
        ScoringManager.Instance.StartScoringSystem();
    }

    /// <summary>
    /// Finishes the current game.
    /// </summary>
    public void EndGame()
    {
        _arduino.Digits = ScoringManager.Instance.Score;
        ScoringManager.Instance.StopScoringSystem();
        SceneManager.Instance.StopObstracleSpawner();
        UIManager.Instance.ShowGameOverText();
        _arduino.GameOver();
    }

    /// <summary>
    /// Returns the screen coordinate of the given axis 
    /// </summary>
    /// <param name="percentage">Value between 0 and 1</param>
    /// <returns>Percentage of screen axis</returns>
    public float CalcPercentageOfScreen(SCREEN_AXIS axis, float percentage)
    {
        float res;

        switch (axis)
        {
            case SCREEN_AXIS.X: res = _screenWidth * percentage - _screenWidth / 2; break;
            case SCREEN_AXIS.Y: res = _screenHeight * percentage - _screenHeight / 2; break;
            default:    res = 0; break;
        }

        return res;
    }
}
