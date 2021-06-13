using System;
using Player;
using UnityEngine;

public class OffscreenIndicator : MonoBehaviour
{
    private enum VerticalPositionType
    {
        Up,
        Centre,
        Down
    }
    
    private enum HorizontalPositionType
    {
        Left,
        Centre,
        Right
    }
    
    [SerializeField] private new Camera camera;
    [SerializeField] private SpriteRenderer headSpriteRenderer;
    [SerializeField] private SpriteRenderer arrowSpriteRenderer;
    [SerializeField] private Transform target;
    [SerializeField] private float padding;
    [SerializeField] private PlayerController playerController;
    
    private VerticalPositionType verticalPosition = VerticalPositionType.Centre;
    private HorizontalPositionType horizontalPosition = HorizontalPositionType.Centre;
    private static readonly int IsLeft = Animator.StringToHash("IsLeft");

    private Vector2 centrePosition;
    private float leftPositionX;
    private float rightPositionX;
    private float upPositionY;
    private float downPositionY;

    private void Start()
    {
        centrePosition = camera.ScreenToWorldPoint(new Vector2(camera.pixelWidth / 2, camera.pixelHeight / 2));
        
        Vector2 bottomLeftPosition = camera.ScreenToWorldPoint(new Vector2(0, 0));
        Vector2 topRightPosition = camera.ScreenToWorldPoint(new Vector2(camera.pixelWidth, camera.pixelHeight));

        leftPositionX = bottomLeftPosition.x + padding;
        rightPositionX = topRightPosition.x - padding;
        upPositionY = topRightPosition.y - padding;
        downPositionY = bottomLeftPosition.y + padding;
    }

    private void Update()
    {
        CalculateOffscreen();
        
        ToggleIndicator();

        float xPosition = horizontalPosition switch
        {
            HorizontalPositionType.Left => leftPositionX,
            HorizontalPositionType.Right => rightPositionX,
            _ => target.position.x
        };
        
        float yPosition = verticalPosition switch
        {
            VerticalPositionType.Down => downPositionY,
            VerticalPositionType.Up => upPositionY,
            _ => target.position.y
        };

        transform.position = new Vector2(
            xPosition,
            yPosition
        );
    }

    private void CalculateOffscreen()
    {
        Vector2 screenSpacePosition = camera.WorldToScreenPoint(target.position);
        
        if (screenSpacePosition.x > 0 && screenSpacePosition.x < camera.pixelWidth)
            horizontalPosition = HorizontalPositionType.Centre;
        else
            horizontalPosition = screenSpacePosition.x < centrePosition.x ?
                HorizontalPositionType.Left :
                HorizontalPositionType.Right;

        if (screenSpacePosition.y > 0 && screenSpacePosition.y < camera.pixelHeight)
            verticalPosition = VerticalPositionType.Centre;
        else
            verticalPosition = screenSpacePosition.y < centrePosition.y ?
                VerticalPositionType.Down :
                VerticalPositionType.Up;
    }

    private void ToggleIndicator()
    {
        if (horizontalPosition == HorizontalPositionType.Centre &&
            verticalPosition == VerticalPositionType.Centre ||
            !playerController.IsReleased)
        {
            headSpriteRenderer.enabled = false;
            arrowSpriteRenderer.enabled = false;
        }
        else
        {
            headSpriteRenderer.enabled = true;
            arrowSpriteRenderer.enabled = true;

            arrowSpriteRenderer.transform.rotation = Quaternion.LookRotation(Vector3.forward, target.position - transform.position) * Quaternion.Euler(0, 0, 90);
        }
        
    }
}
