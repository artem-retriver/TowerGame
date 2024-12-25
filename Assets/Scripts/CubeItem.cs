using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class CubeItem : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private RectTransform _rectTransform;
    private CanvasGroup _canvasGroup;
    private Color _cubeColor;
    private Vector3 _currentCubePosition;

    private int _cubeIndex;
    private bool _isPartOfTower;
    
    public void Initialize(Color color)
    {
        _cubeColor = color;
        GetComponent<UnityEngine.UI.Image>().color = _cubeColor;
    }

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _canvasGroup = GetComponent<CanvasGroup>();
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
        _canvasGroup.blocksRaycasts = true;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _canvasGroup.blocksRaycasts = false;
        _currentCubePosition = transform.position;
        transform.SetParent(transform.root);

        if (_isPartOfTower == false)
        {
            GameManager.Instance.ReplaceCube(this);
        }
        
        UIManager.Instance.ChangeEnableScrollRect(false);
    }

    public void OnDrag(PointerEventData eventData)
    {
        _rectTransform.transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _canvasGroup.blocksRaycasts = true;
        
        if (VoidManager.Instance.IsInVoid(_rectTransform.position))
        {
            UIManager.Instance.ChangeEnableScrollRect(true);
            VoidManager.Instance.AbsorbCube(this, _currentCubePosition);
            GameManager.Instance.NotifyAction("PlaceVoid");
        }
        else if (_isPartOfTower)
        {
            AnimateDisappearance();
            UIManager.Instance.ChangeEnableScrollRect(true);
            TowerManager.Instance.AdjustTowerAfterRemoval(_rectTransform, _currentCubePosition);
            GameManager.Instance.NotifyAction("FailedCube");
        }
        else if (TowerManager.Instance.TryPlaceCube(this) && _isPartOfTower == false)
        {
            _isPartOfTower = true;
            
            UIManager.Instance.ChangeEnableScrollRect(true);
            TowerManager.Instance.PlaceCube(this);
            GameManager.Instance.NotifyAction("PlaceCube");
        }
        else
        {
            AnimateDisappearance();
            UIManager.Instance.ChangeEnableScrollRect(true);
            GameManager.Instance.NotifyAction("MissedCube");
        }
    }

    private void AnimateDisappearance()
    {
        transform.DOScale(new Vector3(0,0,0), 0.5f);
        Destroy(gameObject, 0.51f);
    }
}