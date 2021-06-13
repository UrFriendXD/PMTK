using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private Canvas menuCanvas;
    [SerializeField] private Canvas gameCanvas;
    [SerializeField] private Button[] buttons;
    [SerializeField] private TextMeshProUGUI scoreText;

    public enum UIType
    {
        Menu,
        Game
    }

    public void Hide()
    {
        Hide(menuCanvas);
        Hide(gameCanvas);
    }
    
    private static void Hide(Canvas canvas)
    {
        foreach (Transform child in canvas.transform)
            child.gameObject.SetActive(false);
    }

    public void Show(UIType uiType)
    {
        switch (uiType)
        {
            case UIType.Menu:
                Show(menuCanvas);
                break;
            
            case UIType.Game:
                Show(gameCanvas);
                GameManager.Instance.scoreText = scoreText;
                break;
        }
    }

    private void Show(Canvas canvas)
    {
        foreach (Transform child in canvas.transform)
            child.gameObject.SetActive(true);
    }

    public void Disable()
    {
        foreach (Button button in buttons)
            button.enabled = false;
    }

    public void OnButtonPressed()
    {
        GameManager.Instance.OnBeginPlay();
    }
}