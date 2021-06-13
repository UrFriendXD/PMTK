using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PayloadAnimationController : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    private Rigidbody2D _rb;
    private static readonly int MoveUp = Animator.StringToHash("MoveUp");
    private static readonly int MoveDown = Animator.StringToHash("MoveDown");
    private static readonly int Tethered = Animator.StringToHash("Tethered");

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    public void Update()
    {
        //_animator.SetFloat("Y", moveY);
        if (_rb.velocity.y > 1f)
        {
            _animator.SetBool(MoveUp, true);
            _animator.SetBool(MoveDown, false);
        }
        else if (_rb.velocity.y < -1f)
        {
            _animator.SetBool(MoveDown, true);
            _animator.SetBool(MoveUp, false);
        }
        else
        {
            _animator.SetBool(MoveDown, false);
            _animator.SetBool(MoveUp, false);
        }
        _animator.SetFloat("Speed", _rb.velocity.x);
    }

    public void Connect()
    {
        _animator.SetBool(Tethered, true);
        Debug.Log("CONNECT");
    }

    public void Disconnect()
    {
        _animator.SetBool(Tethered, false);
        _animator.SetTrigger("Fling");
    }
}
