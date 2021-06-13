using Player;
using UnityEngine;

public class OffscreenIndicator : MonoBehaviour
{
    private enum OffscreenType {OnScreen, OffscreenLeft, OffscreenRight }
    
    [SerializeField] private new Camera camera;
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Transform target;
    [SerializeField] private float padding;
    [SerializeField] private PlayerController playerController;
    
    private OffscreenType offscreenType = OffscreenType.OnScreen;
    private static readonly int IsLeft = Animator.StringToHash("IsLeft");

    private float centrePositionX;
    private float leftPositionX;
    private float rightPositionX;

    private void Start()
    {
        centrePositionX = camera.ScreenToWorldPoint(new Vector2(camera.pixelWidth / 2, camera.pixelHeight / 2)).x;
        leftPositionX = camera.ScreenToWorldPoint(new Vector2(0, 0)).x + padding;
        rightPositionX = camera.ScreenToWorldPoint(new Vector2(camera.pixelWidth, camera.pixelHeight)).x - padding;
    }

    private void Update()
    {
        CalculateOffscreen();
        
        ToggleIndicator();

        float xPosition = offscreenType switch
        {
            OffscreenType.OffscreenLeft => leftPositionX,
            OffscreenType.OffscreenRight => rightPositionX,
            _ => 0
        };

        transform.position = new Vector2(
            xPosition,
            target.position.y
        );
    }

    private void CalculateOffscreen()
    {
        Vector2 screenSpacePosition = camera.WorldToScreenPoint(target.position);
        
        if (screenSpacePosition.x > 0 &&
            screenSpacePosition.x < camera.pixelWidth &&
            screenSpacePosition.y > 0 &&
            screenSpacePosition.y < camera.pixelHeight)
        {
            offscreenType = OffscreenType.OnScreen;
        }
        else
        {
            offscreenType = camera.WorldToScreenPoint(target.position).x < centrePositionX ?
                OffscreenType.OffscreenLeft :
                OffscreenType.OffscreenRight;
        }
    }

    private void ToggleIndicator()
    {
        if (offscreenType == OffscreenType.OnScreen || !playerController.IsReleased)
        {
            spriteRenderer.enabled = false;
        }
        else
        {
            spriteRenderer.enabled = true;
            
            animator.SetBool(IsLeft, offscreenType == OffscreenType.OffscreenLeft);
        }
    }
}
