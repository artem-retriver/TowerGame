using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TowerManager : MonoBehaviour
{
    public static TowerManager Instance;
    
    [SerializeField] private RectTransform towerArea;
    public RectTransform bottomPanel;

    private readonly List<RectTransform> _towerCubes = new List<RectTransform>();

    private void Awake()
    {
        Instance = this;
    }

    public bool TryPlaceCube(CubeItem cube)
    {
        if (_towerCubes.Count == 0 || IsAboveTopCube(cube.GetComponent<RectTransform>()))
        {
            return true;
        }
        return false;
    }

    public void PlaceCube(CubeItem cube)
    {
        RectTransform cubeTransform = cube.GetComponent<RectTransform>();
        float randomOffset = Random.Range(-cubeTransform.rect.width / 2, cubeTransform.rect.width / 2);

        if (_towerCubes.Count >= 1)
        {
            var newPositionX = _towerCubes[^1].localPosition.x + randomOffset;
            var newPositionY = _towerCubes[^1].localPosition.y + cubeTransform.rect.height;
            
            switch (newPositionX)
            {
                case <= -195:
                    newPositionX = -195;
                    break;
                case >= 195:
                    newPositionX = 195;
                    break;
            }
            
            cubeTransform.transform.DOLocalMove(new Vector3(newPositionX, newPositionY, 0), 0.5f);
        }
        else
        {
            var newPositionX = cubeTransform.localPosition.x;
            
            switch (newPositionX)
            {
                case <= 75:
                    newPositionX = -195;
                    cubeTransform.transform.DOLocalMoveX(newPositionX, 0.5f);
                    break;
                case >= 465:
                    newPositionX = 195;
                    cubeTransform.transform.DOLocalMoveX(newPositionX, 0.5f);
                    break;
            }
        }
        
        cubeTransform.SetParent(towerArea);
        _towerCubes.Add(cubeTransform);
    }

    bool IsAboveTopCube(RectTransform cube)
    {
        if (_towerCubes.Count == 0) return true;

        RectTransform topCube = _towerCubes[^1];
        return cube.anchoredPosition.y > topCube.anchoredPosition.y;
    }

    public int GetTowerCount() => _towerCubes.Count;

    public string GetCubeData(int index)
    {
        RectTransform cube = _towerCubes[index];
        Color color = cube.GetComponent<UnityEngine.UI.Image>().color;
        CubeData cubeData = new CubeData
        {
            r = color.r,
            g = color.g,
            b = color.b,
            position = cube.anchoredPosition
        };
        return JsonUtility.ToJson(cubeData);
    }

    public void RestoreCube(string data, GameConfig config)
    {
        CubeData cubeData = JsonUtility.FromJson<CubeData>(data);
        Color color = new Color(cubeData.r, cubeData.g, cubeData.b);
        Vector2 position = cubeData.position;

        CubeItem cube = Instantiate(config.cubePrefab, towerArea).GetComponent<CubeItem>();
        cube.ChangeIsPartOfTower(true);
        cube.Initialize(color);
        
        RectTransform cubeTransform = cube.GetComponent<RectTransform>();
        cubeTransform.anchoredPosition = position;
        _towerCubes.Add(cubeTransform);
    }
    
    public void AdjustTowerAfterRemoval(RectTransform removedCube, Vector3 removedCubePosition)
    {
        var index = _towerCubes.IndexOf(removedCube);
        
        if (index == -1)
        {
            return;
        }

        if (index == _towerCubes.Count - 1)
        {
            _towerCubes.RemoveAt(index);
            return;
        }
        
        for (int i = _towerCubes.Count - 1; i > index + 1; i--)
        {
            Vector2 newPosition = _towerCubes[i - 1].transform.position;
            _towerCubes[i].DOMove(newPosition, 0.5f);
        }
        
        _towerCubes[index + 1].DOMove(removedCubePosition, 0.3f).OnComplete(() =>
        {
            _towerCubes.RemoveAt(index);
        });
    }
}

[System.Serializable]
public class CubeData
{
    public float r;
    public float g;
    public float b;
    public Vector2 position;
}