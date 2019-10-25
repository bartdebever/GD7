using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;
using UnityEngine.UI;

public class UISingleton : MonoBehaviour
{
    public Image BottomPanel;

    private Text BottomPanelText;

    // Start is called before the first frame update
    void Start()
    {
        Game.UI = this;

        BottomPanelText = this.BottomPanel.GetComponentInChildren<Text>();
    }

    public void SetBottomText(string text)
    {
        BottomPanelText.text = text;
        BottomPanel.gameObject.SetActive(true);
    }

    public void HideBottom()
    {
        BottomPanel.gameObject.SetActive(false);
    }
}
