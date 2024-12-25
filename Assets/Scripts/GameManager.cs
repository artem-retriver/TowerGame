using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    [SerializeField] private GameConfig config;
    [SerializeField] private TowerManager towerManager;
    [SerializeField] private UIManager uiManager;

    private readonly List<CubeItem> _bottomCubes = new List<CubeItem>();

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        LoadProgress();
        InitializeCubes();
    }
    
    private void OnApplicationQuit()
    {
        SaveProgress();
    }

    private void InitializeCubes()
    {
        foreach (var color in config.cubeColors)
        {
            CreateCube(color);
        }
    }

    private void CreateCube(Color color)
    {
        CubeItem cubeInstance = Instantiate(config.cubePrefab, towerManager.bottomPanel.transform);
        cubeInstance.Initialize(color);
        cubeInstance.ChangeIndexCube(_bottomCubes.Count);
        _bottomCubes.Add(cubeInstance);
    }
    
    public void ReplaceCube(CubeItem cube)
    {
        int indexCube = cube.SetIndexCube();
        Color originalColor = cube.GetComponent<UnityEngine.UI.Image>().color;
        
        CubeItem cubeInstance = Instantiate(cube, towerManager.bottomPanel.transform);
        cubeInstance.Initialize(originalColor);
        cubeInstance.ActivateCanvasGroupBlocksRaycast();
        cubeInstance.ChangeIndexCube(indexCube);
        cubeInstance.transform.SetSiblingIndex(indexCube);
    }


    public void NotifyAction(string message)
    {
        uiManager.DisplayMessage(message);
    }

    private void SaveProgress()
    {
        PlayerPrefs.SetInt("TowerCount", towerManager.GetTowerCount());
        for (int i = 0; i < towerManager.GetTowerCount(); i++)
        {
            PlayerPrefs.SetString($"TowerCube_{i}", towerManager.GetCubeData(i));
        }
        PlayerPrefs.Save();
    }

    private void LoadProgress()
    {
        int count = PlayerPrefs.GetInt("TowerCount", 0);
        for (int i = 0; i < count; i++)
        {
            string data = PlayerPrefs.GetString($"TowerCube_{i}", "");
            if (!string.IsNullOrEmpty(data))
            {
                towerManager.RestoreCube(data, config);
            }
        }
    }
}