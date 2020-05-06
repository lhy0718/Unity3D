using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//https://learn.unity.com/tutorial/writting-the-gamemanager
public class GameManager : Singleton<GameManager>
{
	private string _currentLevelName = string.Empty;

	List<GameObject> _instancedSystemPrefabs;
	List<AsyncOperation> _loadOperations;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
	{
		_instancedSystemPrefabs = new List<GameObject>();
		_loadOperations = new List<AsyncOperation>();

		InstantiateSystemPrefabs();

		if (SceneManager.GetActiveScene().name != "Main")
		{
			LoadLevel("Main");
		}
	}

	void OnLoadOperationComplete(AsyncOperation ao)
	{
		if (_loadOperations.Contains(ao))
		{
			_loadOperations.Remove(ao);

			// dispatch message
			// transition between scenes
		}

		SceneManager.SetActiveScene(SceneManager.GetSceneByName(_currentLevelName));

		if (_currentLevelName == "Main")
		{
			InitializeGame();
		}

		Debug.Log("Load Complete.");
	}

	void OnUnloadOperationComplete(AsyncOperation ao)
	{
		Debug.Log("Unload Complete.");
	}

	void InstantiateSystemPrefabs()
	{
		GameObject[] systemPrefabs = Resources.LoadAll<GameObject>("System");
		GameObject prefabInstance;
		for (int i = 0; i < systemPrefabs.Length; i++)
		{
			prefabInstance = Instantiate(systemPrefabs[i]);
			_instancedSystemPrefabs.Add(prefabInstance);
		}
	}

	public void LoadLevel(string levelName)
	{
		AsyncOperation ao = SceneManager.LoadSceneAsync(levelName, LoadSceneMode.Additive);
		if (ao == null)
		{
			Debug.Log("[GameManager] Unable to load level." + levelName);
			return;
		}

		ao.completed += OnLoadOperationComplete;
		_loadOperations.Add(ao);

		_currentLevelName = levelName;
	}

	public void UnloadLevel(string levelName)
	{
		AsyncOperation ao = SceneManager.UnloadSceneAsync(levelName);
		if (ao == null)
		{
			Debug.Log("[GameManager] Unable to unload level." + levelName);
			return;
		}

		ao.completed += OnLoadOperationComplete;
		ao.completed += OnUnloadOperationComplete;
	}

	private void InitializeGame()
	{

	}

	protected override void OnDestroy()
	{
		base.OnDestroy();

		for (int i = 0; i < _instancedSystemPrefabs.Count; i++)
		{
			Destroy(_instancedSystemPrefabs[i]);
		}
		_instancedSystemPrefabs.Clear();
	}
}
