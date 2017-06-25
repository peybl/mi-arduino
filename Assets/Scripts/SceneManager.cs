using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : SingletonBase<SceneManager>
{
    public static readonly float PLAYER_POS_X = .25f; // Percentage from left screen edge
    public static readonly float OBSTRACLE_POS_X = 1f; // Percentage from left screen edge
    public static readonly float OBSTRACLE_SPAWN_RATE = 2f;    // in seconds
    public static readonly float OBSTRACLE_BRIGHT_PROBABILITY = 0.6f; // percentage
    public static readonly int MIN_NUM_BRIGHT_OBSTRACLES = 2;
    public static readonly int MAX_NUM_BRIGHT_OBSTRACLES = 4;
    public static readonly string ROOT_TAG = "SceneGraph";
    public static readonly string OBSTRACLE_TAG = "Obstracle";

    public GameObject playerPrefab;
    public GameObject obstracleBrightPrefab;
    public GameObject obstracleDarkPrefab;
    public GameObject explosionPrefab;

    private GameObject _rootFolder;
    private GameObject _obstracleFolder;
    private GameObject _player;
    private List<GameObject> _obstracles = new List<GameObject>();
    private int _obstracleCounter = 0;

    private IEnumerator coroutine;  // Copy of coroutine

    public enum OBSTRACLE_TYPE { BRIGHT, DARK };


    private void Start ()
    {
        // Find or create root-folder
        _rootFolder = GameObject.Find(ROOT_TAG);
        if (_rootFolder == null)
        {
            Debug.Log("Could not find root element with name '" + ROOT_TAG + "' creating new one!");
            _rootFolder = new GameObject();
            _rootFolder.transform.position = Vector3.zero;
            _rootFolder.transform.rotation = Quaternion.identity;
            _rootFolder.name = ROOT_TAG;
        }

        // Create folder for obstracles
        _obstracleFolder = new GameObject();
        _obstracleFolder.name = "Obstracles";
        _obstracleFolder.transform.SetParent(_rootFolder.transform);

        //Set properties
        GameManager.Instance.SetScreenProperties();

        // Create player object
        SceneManager.Instance.SpawnPlayer();
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    /// <summary>
    /// Creates the player GameObject.
    /// </summary>
    public void SpawnPlayer()
    {
        if (playerPrefab != null && _player == null)
        {
            _player = Instantiate(
                playerPrefab,
                new Vector3(GameManager.Instance.CalcPercentageOfScreen(GameManager.SCREEN_AXIS.X, PLAYER_POS_X), 0),
                Quaternion.identity);

            _player.tag = "Player";
            _player.name = "Player";
            _player.transform.SetParent(_rootFolder.transform);
        }
        else
            Debug.LogError("Player-Prefab is not set or player is already instantiated.");
    }

    /// <summary>
    /// Creates an 'obstracle' GameObject with the given typen.
    /// </summary>
    /// <param name="type">The obstracle's type</param>
    public void SpawnObstracle(OBSTRACLE_TYPE type)
    {
        if (obstracleBrightPrefab != null && obstracleDarkPrefab != null)
        {
            List<GameObject> obstracles;
            string name = "Obstracle";
            switch (type) {
                case OBSTRACLE_TYPE.BRIGHT:
                    obstracles = createBrightObstraclesRandomly();
                    name += "Bright_";
                    break;
                case OBSTRACLE_TYPE.DARK:
                    obstracles = createDarkObstracle();
                    name += "Dark_";
                    break;
                default:
                    obstracles = new List<GameObject>();    // empty list
                    break;
            }
  
            foreach (GameObject o in obstracles)
            {
                o.tag = OBSTRACLE_TAG;
                o.name = "" + _obstracleCounter++;
                o.transform.SetParent(_obstracleFolder.transform);

                _obstracles.Add(o);
            }
        } else
            Debug.LogError("Obstracle-Prefabs are not set.");
    }

    private List<GameObject> createDarkObstracle()
    {
        /** Since dark obstracles fill the height of the screen, instantiate only a single GameObject at the center of X. */
        List<GameObject> obstracles = new List<GameObject>();

        var o = Instantiate(obstracleDarkPrefab,
            new Vector3(GameManager.Instance.CalcPercentageOfScreen(GameManager.SCREEN_AXIS.X, OBSTRACLE_POS_X), 0),
            Quaternion.identity);

        obstracles.Add(o);
        return obstracles;
    }

    private List<GameObject> createBrightObstraclesRandomly()
    {
        /** Creates a random number of obstracles and groups and positions them randomly on the y-axis. */
        List<GameObject> obstracles = new List<GameObject>();

        int amount = Random.Range(MIN_NUM_BRIGHT_OBSTRACLES, MAX_NUM_BRIGHT_OBSTRACLES + 1);    // Number of obstracles to create
        //int amount = 1;

        // Get a random position between the lower and upper bound of the camera view.
        // Add an offset to the random position to avoid positioning obstracles outside of the camera view.
        float rand = Random.Range(0.0f, 1.0f);
        float centerPos = GameManager.Instance.CalcPercentageOfScreen(GameManager.SCREEN_AXIS.Y, rand);
        float multiplier = (centerPos <= 0.5f) ? 0.5f : -0.5f;
        float offset = obstracleBrightPrefab.GetComponent<Renderer>().bounds.size.y * multiplier;
        centerPos += amount * offset;


        for (int i = 0; i < amount; i++)
        {
            // Since collision is always enabled on bright obstracles, a small vertical
            // deviation between them is enough to force them to realign themselves.
            var o = Instantiate(obstracleBrightPrefab,
                new Vector3(
                    GameManager.Instance.CalcPercentageOfScreen(GameManager.SCREEN_AXIS.X, OBSTRACLE_POS_X),
                    centerPos + i * 0.01f ),
                Quaternion.identity);

            obstracles.Add(o);
        }
        return obstracles;
    }

    /// <summary>
    /// Destroys the given obstracle.
    /// </summary>
    /// <param name="obstracle">The GameObject to destroy</param>
    public void DestroyObstracle(GameObject obstracle)
    {
        _obstracles.Remove(obstracle);
        Destroy(obstracle);
    }

    /// <summary>
    /// Destroys the player object and plays the explosion animation.
    /// </summary>
    public void DestroyPlayer()
    {
        if (explosionPrefab != null)
        {
            var explosion = Instantiate(explosionPrefab, _player.transform.position, Quaternion.identity);
            explosion.transform.SetParent(_rootFolder.transform);
            
            SelfDestruction sd = explosion.AddComponent<SelfDestruction>();
            sd.SetDelay(1.0f);  // Cartoon explosion is kinda weird - use hardcoded length!
            //sd.SetDelay(explosionPrefab.GetComponent<ParticleSystem>().main.duration - 0.1f);
        }
        else
            Debug.Log("No explosion Prefab found!");

        Destroy(_player);
    }

    /// <summary>
    /// Starts the obstracle spawner for an unlimited time span.
    /// </summary>
    public void StartObstracleSpawner()
    {
        coroutine = RandomObstracleSpawner();
        StartCoroutine(coroutine);
    }

    /// <summary>
    /// Stops the currently running obstracle spawner.
    /// </summary>
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

    private IEnumerator RandomObstracleSpawner()
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
