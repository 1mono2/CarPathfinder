using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using DG.Tweening;
using UniRx;

public class ShakeCamera : MonoBehaviour
{
    private Camera _camera;
    
    [SerializeField] float _duration = 0.5f;
    [SerializeField] float _strength = 3f;
    [SerializeField] int _vibrato = 15;
    [SerializeField] float _randomness = 90;

    private void Start()
    {
        _camera = GetComponent<Camera>();
        
        LevelPresenter.I.LevelProgressState
            .Where(state => state == StateType.Success)
            .Subscribe(_ => OnSuccess()).AddTo(this);
    }
    
    private async void OnSuccess()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(0.1f));
        ShakeRotateCamera();
    }
    
    [ContextMenu("ShakeRotateCamera")]
    public void ShakeRotateCamera()
    {
        _camera.DOShakeRotation(_duration, _strength, _vibrato, _randomness);
    }
}
