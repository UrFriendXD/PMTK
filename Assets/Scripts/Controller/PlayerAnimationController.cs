using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private Animator _animator;
    private static readonly int MoveUp = Animator.StringToHash("MoveUp");
    private static readonly int MoveDown = Animator.StringToHash("MoveDown");
    private static readonly int Fling1 = Animator.StringToHash("Fling");

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void UpdateMoveAnimation(float moveY)
    {
        //_animator.SetFloat("Y", moveY);
        if (moveY > 0)
        {
            _animator.SetBool(MoveUp, true);
            _animator.SetBool(MoveDown, false);
        }
        else if (moveY < 0)
        {
            _animator.SetBool(MoveDown, true);
            _animator.SetBool(MoveUp, false);
        }
        else
        {
            _animator.SetBool(MoveDown, false);
            _animator.SetBool(MoveUp, false);
        }
    }

    public void Fling()
    {
        _animator.SetTrigger(Fling1);
    }
}
