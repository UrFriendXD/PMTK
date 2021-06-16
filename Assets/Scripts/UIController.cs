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

    [SerializeField] private bool pulse;
    [SerializeField] private float pulseSpeed;
    [SerializeField] private float pulseMin;

    private TextMeshProUGUI _buttonText;

    public enum UIType
    {
        Menu,
        Game
    }

    public bool Enabled => playButton.enabled && playButton.gameObject.activeInHierarchy;

    private void Awake()
    {
        _buttonText = playButton.GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Update()
    {
        if (!Enabled)
            return;

        _buttonText.alpha = Mathf.Lerp(pulseMin, 1.0f, (Mathf.Sin(Time.time * pulseSpeed) + 1.0f) / 2.0f);

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
        OnButtonExit();
        GameManager.Instance.OnBeginPlay();
    }

    public void OnButtonHover()
    {
        if (!Enabled)
            return;
        
        TextMeshProUGUI text = playButton.GetComponentInChildren<TextMeshProUGUI>();
        text.textStyle = text.styleSheet.GetStyle("Hover");
    }

    public void OnButtonExit()
    {
        TextMeshProUGUI text = playButton.GetComponentInChildren<TextMeshProUGUI>();
        text.textStyle = text.styleSheet.GetStyle("Normal");
    }
}