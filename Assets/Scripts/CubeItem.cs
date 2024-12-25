using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class CubeItem : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Color cubeColor;
    private Vector3 currentCubePosition;

    private int _cubeIndex;
    private bool _isPartOfTower;
    
    public void Initialize(Color color)
    {
        cubeColor = color;
        GetComponent<UnityEngine.UI.Image>().color = cubeColor;
    }

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void ChangeIsPartOfTower(bool partOfTower)
    {
        _isPartOfTower = partOfTower;
    }

    public void ChangeIndexCube(int newIndex)
    {
        _cubeIndex = newIndex;
    }

    public int SetIndexCube()
    {
        return _cubeIndex;
    }

    public void ActivateCanvasGroupBlocksRaycast()
    {
        canvasGroup.blocksRaycasts = true;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = false;
        currentCubePosition = transform.position;
        transform.SetParent(transform.root);

        if (_isPartOfTower == false)
        {
            GameManager.Instance.ReplaceCube(this);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        
        if (VoidManager.Instance.IsInVoid(rectTransform.position))
        {
            VoidManager.Instance.AbsorbCube(this, currentCubePosition);
            GameManager.Instance.NotifyAction("Cube removed through the void.");
        }
        else if (_isPartOfTower)
        {
            AnimateDisappearance();
            Destroy(gameObject, 0.5f);
            TowerManager.Instance.AdjustTowerAfterRemoval(rectTransform, currentCubePosition);
            GameManager.Instance.NotifyAction("Cube removed through the failed.");
        }
        else if (TowerManager.Instance.TryPlaceCube(this) && _isPartOfTower == false)
        {
            _isPartOfTower = true;
            
            TowerManager.Instance.PlaceCube(this);
            GameManager.Instance.NotifyAction("Cube placed on the tower.");
        }
        else
        {
            AnimateDisappearance();
            GameManager.Instance.NotifyAction("Cube missed and disappeared.");
        }
    }

    private void AnimateDisappearance()
    {
        transform.DOScale(new Vector3(0,0,0), 0.5f);
    }
}