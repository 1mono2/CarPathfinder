
using System;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

[RequireComponent(typeof(Rigidbody2D))]
public class CarBehavior : MonoBehaviour
{
    [SerializeField] private float _speed = 1f;
    private Rigidbody2D _rigidbody;
    private IDisposable _move;
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        LevelPresenter.I.LevelProgressState
            .Where(state => state != StateType.StartView)
            .Subscribe(_ => Move()).AddTo(this);
        
        this.OnTriggerEnter2DAsObservable()
            .Where(collision => collision.gameObject.CompareTag("GoalFlag"))
            .Subscribe(collision =>
            {
                LevelPresenter.I.GoalCounter.AddCount(1);
            }).AddTo(this);

    }
    
    private void Move()
    {
        _rigidbody.isKinematic = false;
        _move?.Dispose();
        _move = this.FixedUpdateAsObservable()
            .Subscribe(_ => _rigidbody.AddForce(_speed * transform.right));
    }
    
    private void Stop()
    {
        _move.Dispose();
    }
    
}
