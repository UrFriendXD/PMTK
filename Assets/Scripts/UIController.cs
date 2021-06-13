using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private Canvas menuCanvas;
    [SerializeField] private Canvas gameCanvas;
    [SerializeField] private Button playButton;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private Color selectionColor;

    public enum UIType
    {
        Menu,
        Game
    }

    private void Update()
    {
        if (!playButton.enabled || !playButton.gameObject.activeInHierarchy)
            return;
        
        if (Keyboard.current.enterKey.wasPressedThisFrame || Keyboard.current.spaceKey.wasPressedThisFrame)
            OnButtonPressed();
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
        playButton.enabled = false;
    }

    public void OnButtonPressed()
    {
        GameManager.Instance.OnBeginPlay();
    }

    public void OnButtonHover()
    {
        if (!playButton.enabled || !playButton.gameObject.activeInHierarchy)
            return;
        
        TextMeshProUGUI text = playButton.GetComponentInChildren<TextMeshProUGUI>();
        text.fontStyle = FontStyles.Underline;
        text.color = selectionColor;
    }

    public void OnButtonExit()
    {
        TextMeshProUGUI text = playButton.GetComponentInChildren<TextMeshProUGUI>();
        text.fontStyle = FontStyles.Normal;
        text.color = Color.white;
    }
}