using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoringManager : SingletonBase<ScoringManager> {

    private int _score = 0;
    public int Score { get { return _score; } }

    private IEnumerator coroutine;  // Copy of coroutine


    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    public void StartScoringSystem()
    {
        coroutine = IncreaseScore();
        StartCoroutine(coroutine);
    }

    public void StopScoringSystem()
    {
        if (coroutine != null)
            StopCoroutine(coroutine);
    }

    #region Coroutines

    private IEnumerator IncreaseScore()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            _score++;
        }
    }

    #endregion
}
