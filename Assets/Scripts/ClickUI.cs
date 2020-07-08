using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickUI : MonoBehaviour
{
    private Animator _animator;
    private bool _isAnimationActive = false;

    void Awake()
    {
        _animator = GetComponent<Animator>();
        _animator.enabled = false;
    }

    public void StartAnimation()
    {
        _isAnimationActive = _animator.enabled = true;
    }

    public void StopAnimation()
    {
        _isAnimationActive = _animator.enabled = false;
    }

    public bool AnimationState {
        get { return _isAnimationActive; }
    }
}
