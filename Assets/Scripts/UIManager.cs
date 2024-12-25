using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Text messageText;
    [SerializeField] private float messageDuration = 2f;

    private float _messageTimer;

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

    public void DisplayMessage(string message)
    {
        messageText.text = message;
        _messageTimer = messageDuration;
    }

    private void ClearMessage()
    {
        messageText.text = "";
    }
}