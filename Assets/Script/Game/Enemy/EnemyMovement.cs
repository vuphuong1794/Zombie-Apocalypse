using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public bool canMove=true;

    [SerializeField]
    private float _speed;

    [SerializeField]
    private float _rotationSpeed;

    private Rigidbody2D _rigidbody;
    private PlayerAwarenessController _playerAwarenessController;
    private Vector2 _targetDirection;
    private float _changeDirectionCooldown;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _playerAwarenessController = GetComponent<PlayerAwarenessController>();
        _targetDirection = transform.up;

    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            UpdateTargetDirection();
            RotateTowardsTarget();
            SetVelocity();
        }
    }

    //update hướng đi của enemy
    private void UpdateTargetDirection()
    {
        HandleRandomDirectionChange();
        HandlePlayerTargeting();
    }

    // Hành vi chuyển động loạn xạ của zombie khi không tìm thấy người chơi
    private void HandleRandomDirectionChange()
    {
        _changeDirectionCooldown -= Time.deltaTime;

        if (_changeDirectionCooldown <= 0)
        {
            float angleChange = Random.Range(-90f, 90f);
            Quaternion rotation = Quaternion.AngleAxis(angleChange, transform.forward);
            _targetDirection = rotation * _targetDirection;

            _changeDirectionCooldown = Random.Range(1f, 5f);
        }
    }

    // Tìm vị trị player
    private void HandlePlayerTargeting()
    {
        if (_playerAwarenessController.AwareOfPlayer)
        {
            _targetDirection = _playerAwarenessController.DirectionToPlayer;
        }
    }

    //quay theo hướng player
    private void RotateTowardsTarget()
    {
        Quaternion targetRotation = Quaternion.LookRotation(transform.forward, _targetDirection);
        Quaternion rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);

        _rigidbody.SetRotation(rotation);
    }

    //cho enemy tiến lại player
    private void SetVelocity()
    {
        _rigidbody.velocity = _targetDirection * _speed;

        // Di chuyển theo hướng player mà không cần quay
        //_rigidbody.velocity = _targetDirection * _speed;
    }
}
