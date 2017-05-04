using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : SingletonBase<SceneManager> {
    
    public static readonly float PLAYER_POS_X = .1f; // Percentage from left screen edge
    public static readonly float OBSTRACLE_POS_X = 1f; // Percentage from left screen edge
    public static readonly float OBSTRACLE_SPAWN_RATE = 1.5f;    // in seconds
    public static readonly float OBSTRACLE_BRIGHT_PROBABILITY = 0.6f; // percentage
    public static readonly string OBSTRACLE_TAG = "Obstracle";

    public GameObject playerPrefab;
    public GameObject obstracleBrightPrefab;
    public GameObject obstracleDarkPrefab;

    private GameObject _root;
    private GameObject _player;
    private List<GameObject> _obstracles = new List<GameObject>();
    private int _obstracleCounter = 0;

    private IEnumerator coroutine;  // Copy of coroutine

    public enum OBSTRACLE_TYPE { BRIGHT, DARK };


    private void Start () {
        // Create root-folder for obstracles
        _root = new GameObject();
        _root.transform.position = Vector3.zero;
        _root.transform.rotation = Quaternion.identity;
        _root.name = "SceneGraph";
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    public void SpawnPlayer()
    {
        if (playerPrefab != null && _player == null)
            _player = Instantiate(
                playerPrefab,
                new Vector3(GameManager.Instance.CalcPercentageOfScreen(GameManager.SCREEN_AXIS.X, PLAYER_POS_X), 0),
                Quaternion.identity);
        else
            Debug.LogError("Player-Prefab is not set or player is already instantiated.");
    }

    public void SpawnObstracle(OBSTRACLE_TYPE type)
    {
        if (obstracleBrightPrefab != null)
        {
            GameObject prefab;
            string name = "Obstracle";
            switch (type) {
                case OBSTRACLE_TYPE.BRIGHT:
                    prefab = obstracleBrightPrefab;
                    name += "Bright_";
                    break;
                case OBSTRACLE_TYPE.DARK:
                    prefab = obstracleDarkPrefab;
                    name += "Dark_";
                    break;
                default:
                    prefab = null;
                    name += "_";
                    break;
            }

            var o = Instantiate(prefab,
                new Vector3(GameManager.Instance.CalcPercentageOfScreen(GameManager.SCREEN_AXIS.X, OBSTRACLE_POS_X), 0),
                Quaternion.identity);

            o.tag = OBSTRACLE_TAG;
            o.name = "" + _obstracleCounter++;
            o.transform.SetParent(_root.transform);
            
            _obstracles.Add(o);
        } else
            Debug.LogError("Obstracle-Prefab is not set.");
    }

    public void DestroyObstracle(GameObject obstracle)
    {
        _obstracles.Remove(obstracle);
        Destroy(obstracle);
    }

    public void StartObstracleSpawner()
    {
        coroutine = RandomObstracleSpawner();
        StartCoroutine(coroutine);
    }

    public void StopObstracleSpawner() {
        if (coroutine != null)
            StopCoroutine(coroutine);
    }

    private bool InRandomRange(float value)
    {
        float random = Random.Range(0.0f, 1.0f);

        if (random <= value)
            return true;
        else
            return false;
    }

    #region Coroutines

    public IEnumerator RandomObstracleSpawner()
    {
        while (true)
        {
            // Creates randomly bright and dark obstracles
            if (InRandomRange(OBSTRACLE_BRIGHT_PROBABILITY))
                SpawnObstracle(OBSTRACLE_TYPE.BRIGHT);
            else
                SpawnObstracle(OBSTRACLE_TYPE.DARK);

            yield return new WaitForSeconds(OBSTRACLE_SPAWN_RATE);
        }
    }

    #endregion
}
