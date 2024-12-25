using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "ScriptableObjects/GameConfig")]
public class GameConfig : ScriptableObject
{
    public CubeItem cubePrefab;
    public Color[] cubeColors;
}