using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MonsterBehaviour : MonoBehaviour
{
    public enum States {
        Appears, Suffer, GoesAway
    }
    [Header("Settings")]
    public Transform monsterParent;

    [Header("Lamp")]
    public GameObject lamp;

    [Header("Effects")]
    public GameObject vanishSoundEffect;

    [SerializeField]
    private States currentState = States.Appears;

    private Transform _transform;

    private string _name;
    private bool _isActive = false;

    private Animator _anim;

    private Vector3 defaultScale;

    void Awake()
    {
        _transform = transform;
        _anim = GetComponent<Animator>();
        _name = name;

        vanishSoundEffect.SetActive(false);

        defaultScale = _transform.localScale;
    }

    public void Suffer ()
    {
        if (currentState == States.Suffer) return;
        _isActive = false;
        lamp.GetComponent<LampBehaviour>().Flicking = false;
        currentState = States.Suffer;
        //_anim.SetBool("Suffer", true);
        StartCoroutine("Hide");
    }

    private IEnumerator Hide()
    {
        currentState = States.GoesAway;
        yield return new WaitForSeconds(0.3f);

        vanishSoundEffect.SetActive(true);

        _transform.DOScale(Vector3.zero, 1f).OnComplete(Reset);
    }

    private void Reset()
    {
        currentState = States.Appears;
        vanishSoundEffect.SetActive(false);
        if (!GameManager.Instance.GameIsOver)
            StartCoroutine("SpawnRandomEnemy");
    }

    private IEnumerator SpawnRandomEnemy()
    {
        yield return new WaitForSeconds(Random.Range(1.0f, 2.0f));
        _transform.localScale = defaultScale;
        GameManager.Instance.SetRandomEnemy();
    }

    public bool IsActive {
        get { return _isActive; }
        set {
            _isActive = value;
            lamp.GetComponent<LampBehaviour>().Flicking = (_isActive) ? true : false;
        }
    }

    public States State {
        get { return currentState; }
    }
}
