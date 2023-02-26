
using System;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using UnityEngine.VFX;
using DG.Tweening;
using Cysharp.Threading.Tasks;

[RequireComponent(typeof(Rigidbody2D))]
public class CarBehavior : MonoBehaviour
{
    [SerializeField] private float _speed = 1f;
    
    [SerializeField] private GameObject _body;
    [SerializeField] private GameObject _wheel;
    [SerializeField] private Collider2D _bodyCollider;
    [SerializeField] VisualEffect _gasEffect;
    [SerializeField] ParticleSystem _explosionPrefab;
    
    
    private Rigidbody2D _rigidbody;
    private IDisposable _move;
    private bool _isGoal = false;
    public bool IsGoal => _isGoal;
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        LevelPresenter.I.LevelProgressState
            .Where(state => state != StateType.StartView)
            .Subscribe(_ => Move()).AddTo(this);
        
        _bodyCollider.OnTriggerEnter2DAsObservable()
            .Where(collision => collision.gameObject.CompareTag("GoalFlag") && !_isGoal)
            .Subscribe(collision =>
            {
                LevelPresenter.I.GoalCounter.AddCount(1);
                _isGoal = true;
            }).AddTo(this);
        
        _bodyCollider.OnCollisionEnter2DAsObservable()
            .Subscribe(collision =>
            {
                if(collision.gameObject.TryGetComponent<IObstacle>(out var obstacle))
                {
                    Crash();
                }
            }).AddTo(this);
        
        this.transform.ObserveEveryValueChanged(transform => transform.position)
            .Where(position => position.y < -20f)
            .Subscribe(_ => Destroy(this.gameObject)).AddTo(this);
        
        _gasEffect.Play();
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
        _move?.Dispose();
        _gasEffect.Stop();
    }
    
    [ContextMenu("Crash")]
    private async void Crash()
    {
        var explosion = Instantiate(_explosionPrefab, this.transform.position, Quaternion.identity);
        explosion.Play();
        _move?.Dispose();
        _gasEffect.Stop();
        await UniTask.Delay(TimeSpan.FromSeconds(0.5f), cancellationToken:this.GetCancellationTokenOnDestroy());
        await this.transform.DOScale(Vector3.zero, 0.5f).ToUniTask(cancellationToken:this.GetCancellationTokenOnDestroy());
        if(this != null)
            Destroy(this.gameObject);
    }

    private void OnDestroy()
    {
        _move?.Dispose();
    }
}
