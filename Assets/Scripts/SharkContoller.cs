using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class SharkContoller : MonoBehaviour
{
    [SerializeField] private Animator animatorController;
    [SerializeField] private Animation sickAnimation;
    [SerializeField] private UIController uiController;
    [SerializeField] private InputController inputController;
    [SerializeField] private float defaultOffsetX = 0.4f;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float attackTime = 2;
    [SerializeField] private int defaultScore = 50;

    private Vector3 _startPos;
    private Vector3 _targetPos;
    private Quaternion _targetRotation;
    private SharkState _currentState;

    private Obstacle _currentObstacle;

    private float _attackTimer = 2;
    private static readonly int AttackState = Animator.StringToHash("attack");
    private static readonly int MoveState = Animator.StringToHash("move");

    public Action<Obstacle> OnDestroyObstacle;


    public void Setup(float defaultPosX)
    {
        _startPos = new Vector3(defaultPosX + defaultOffsetX, 0, 0);

        _attackTimer = attackTime;

        transform.position = _startPos;
        _currentState = SharkState.Idle;
        inputController.OnPointerClick += SetTarget;
        uiController.SetScore(defaultScore);
    }

    private void Update()
    {
        switch (_currentState)
        {
            case SharkState.Idle:
                transform.position = Vector3.Lerp(transform.position, _startPos, moveSpeed * Time.deltaTime);
                transform.rotation =
                    Quaternion.Lerp(transform.rotation, Quaternion.identity, moveSpeed * Time.deltaTime);
                break;
            case SharkState.Move:
                if (Vector3.Distance(transform.position, _targetPos) <= 0.5f)
                {
                    _currentState = SharkState.Idle;
                    animatorController.SetBool(MoveState, false);
                }
                else
                {
                    transform.position = Vector3.Lerp(transform.position, _targetPos, moveSpeed * Time.deltaTime);
                    print(_targetRotation);
                    transform.rotation =
                        Quaternion.Lerp(transform.rotation, _targetRotation, moveSpeed * Time.deltaTime);
                }

                break;
            case SharkState.Attack:
                _attackTimer -= Time.deltaTime;
                if (_attackTimer <= 0)
                {
                    _currentState = SharkState.Idle;
                    _attackTimer = attackTime;
                    ChangeScore();
                    
                    animatorController.SetBool(AttackState, false);
                    OnDestroyObstacle.Invoke(_currentObstacle);
                }

                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void ChangeScore()
    {
        if (_currentObstacle.type == ObstacleType.BadFish)
        {
            defaultScore -= 8;
            sickAnimation.Play();
        }
        else
        {
            defaultScore += 10;
        }

        if (defaultScore < 0)
        {
            defaultScore = 0;
            uiController.ShowLoose();
            Time.timeScale = 0;
        }

        if (defaultScore > 100)
        {
            defaultScore = 100;
            uiController.ShowWin();
            Time.timeScale = 0;
        }

        uiController.SetScore(defaultScore);
    }

    private void SetTarget(Vector3 pos)
    {
        if (_currentState == SharkState.Attack) return;

        _currentState = SharkState.Move;
        animatorController.SetBool(MoveState, true);
        _targetPos = pos;

        var diff = _targetPos - transform.position;
        diff.Normalize();
        _targetRotation = transform.rotation;
        var zAngle = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        _targetRotation.eulerAngles = new Vector3(0, 0, zAngle);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var otherObstacle = other.GetComponent<Obstacle>();
        if (!otherObstacle) return;
        _currentObstacle = otherObstacle;
        animatorController.SetBool(AttackState, true);
        _currentState = SharkState.Attack;
    }

    enum SharkState
    {
        Idle,
        Move,
        Attack,
    }
}