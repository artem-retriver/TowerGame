using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class LocalizedTextUpdater : MonoBehaviour
{
    private Text _messageText;

    private void Start()
    {
        _messageText = GetComponent<Text>();
    }

    public void UpdateLocalizedText(string key)
    {
        var localizedString = LocalizationSettings.StringDatabase.GetLocalizedStringAsync("GameTexts", key);

        localizedString.Completed += result =>
        {
            if (result.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded)
            {
                
                if (_messageText != null)
                {
                    _messageText.text = result.Result;
                }
            }
            else
            {
                Debug.LogError($"Failed to resolve localized key '{key}'");
            }
        };
    }
}