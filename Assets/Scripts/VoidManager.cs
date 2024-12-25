using DG.Tweening;
using UnityEngine;

public class VoidManager : MonoBehaviour
{
    public static VoidManager Instance;
    
    [SerializeField] private RectTransform voidArea;
    [SerializeField] private RectTransform voidCubePositionStart;
    [SerializeField] private RectTransform voidCubePositionEnd;

    private void Awake()
    {
        Instance = this;
    }

    public bool IsInVoid(Vector3 position)
    {
        return RectTransformUtility.RectangleContainsScreenPoint(voidArea, position);
    }

    public void AbsorbCube(CubeItem cube, Vector3 removedCubePosition)
    {
        cube.transform.DOJump(voidCubePositionStart.transform.position, 100,0,0.5f).OnComplete(() =>
        {
            cube.transform.SetParent(voidCubePositionStart);
            cube.transform.DOMove(voidCubePositionEnd.transform.position,0.5f);
            cube.transform.DOScale(new Vector3(0,0,0), 0.5f);
            Destroy(cube.gameObject, 0.5f);
        });
        
        TowerManager.Instance.AdjustTowerAfterRemoval(cube.GetComponent<RectTransform>(), removedCubePosition);
    }
}