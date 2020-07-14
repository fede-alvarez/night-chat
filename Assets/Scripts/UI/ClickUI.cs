using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickUI : MonoBehaviour
{
    private Animator _animator;

    [SerializeField]
    private bool _isAnimationActive = false;

    void Awake()
    {
        _animator = GetComponent<Animator>();
        _animator.enabled = false;
    }

    void Start()
    {
        if (_isAnimationActive)
            StartAnimation();
    }

    public void StartAnimation()
    {
        _isAnimationActive = _animator.enabled = true;
        _animator.SetBool("Animating", true);
    }

    public void StopAnimation()
    {
        _animator.SetBool("Animating", false);
        _isAnimationActive = _animator.enabled = false;
    }

    public bool AnimationState {
        get { return _isAnimationActive; }
    }
}
