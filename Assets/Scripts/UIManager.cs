using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    
    [SerializeField] private LocalizedTextUpdater localizedTextUpdater;
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private Text messageText;

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