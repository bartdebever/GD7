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

    private Text _bottomPanelText;

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
}
