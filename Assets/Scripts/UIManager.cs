using System;
using UnityEditor.Localization.Plugins.XLIFF.V20;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    
    [SerializeField] private LocalizedTextUpdater localizedTextUpdater;
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private Text messageText;
    
    private Vector3 _currentPosition;

    private const float MessageDuration = 2f;
    private float _messageTimer;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (_messageTimer > 0)
        {
            _messageTimer -= Time.deltaTime;
            
            if (_messageTimer <= 0)
            {
                ClearMessage();
            }
        }
    }

    public void ChangeEnableScrollRect(bool enable)
    {
        scrollRect.enabled = enable;
        
        if (enable == false)
        {
            _currentPosition.x = scrollRect.horizontalNormalizedPosition;
        }
        else
        {
            scrollRect.horizontalNormalizedPosition = _currentPosition.x;
        }
    }

    public void DisplayMessage(string key)
    {
        localizedTextUpdater.UpdateLocalizedText(key);
        _messageTimer = MessageDuration;
    }

    private void ClearMessage()
    {
        messageText.text = "";
    }
}