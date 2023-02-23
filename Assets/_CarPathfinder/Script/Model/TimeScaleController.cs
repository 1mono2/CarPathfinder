using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class TimeScaleController: MonoBehaviour
{
    [SerializeField] private float _timeRange = 1.0f;
    [SerializeField] private float _minTime = 0.1f;
    [SerializeField]private float _timeScaleOnSuccess = 0.7f;
    


    private void Start()
    {
        LevelPresenter.I.LevelProgressState
            .Where(state => state == StateType.Success)
            .Subscribe(_ =>
            {
                Time.timeScale = _timeScaleOnSuccess;
                Observable.Timer(TimeSpan.Zero,TimeSpan.FromSeconds(_minTime))
                    .Select(time => time * _minTime)
                    .TakeWhile(time => time < _timeRange)
                    .Subscribe(time =>
                    {
                        Time.timeScale = Mathf.Lerp(_timeScaleOnSuccess, 1f, time / _timeRange);
                    }).AddTo(this);
                
            }).AddTo(this);
    }

    private void OnDestroy()
    {
        Time.timeScale = 1f;
    }
}
