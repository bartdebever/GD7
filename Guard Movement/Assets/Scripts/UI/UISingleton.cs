using Assets.Scripts;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A singleton that manages the UI.
/// </summary>
public class UISingleton : MonoBehaviour
{
    /// <summary>
    /// The bottom panel of the UI.
    /// </summary>
    public Image BottomPanel;

    public Text MoneyText;

    public Text AmmoText;

    private Text _bottomPanelText;

    private int _money;

    private void Start()
    {
        Game.UI = this;

        _bottomPanelText = this.BottomPanel.GetComponentInChildren<Text>();
    }

    /// <summary>
    /// Sets the text on the bottom panel to be the provided <paramref name="text"/>.
    /// Also enables the bottom panel.
    /// </summary>
    /// <param name="text">The text to be set.</param>
    public void SetBottomText(string text)
    {
        // Unity editor tried to set this text when closing.
        // Unsure where the call came from but error checking is not terrible.
        if (_bottomPanelText == null)
        {
            Debug.LogError("Tried to set bottom text when no text was found.");
            return;
        }

        _bottomPanelText.text = text;
        BottomPanel.gameObject.SetActive(true);
    }

    /// <summary>
    /// Hides the bottom panel.
    /// </summary>
    public void HideBottom()
    {
        BottomPanel.gameObject.SetActive(false);
    }

    public void IncreaseMoney(int amount)
    {
        _money += amount;

        MoneyText.text = $"$ {_money}";
    }

    public void SetAmmo(int ammo)
    {
        AmmoText.text = $"{ammo}x O";
    }
}
