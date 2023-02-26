using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UniRx;
using DG.Tweening;
using UnityEngine.UI;

public class SuccessCanvasView : MonoBehaviour
{
    [SerializeField] private GameObject _successText;
    private Vector3 _defaultScale;
    [SerializeField] private ParticleSystem _confetti;
    [SerializeField] private List<GameObject> _objects;
    private void Start()
    {
        _defaultScale = _successText.transform.localScale;
        _successText.transform.localScale = Vector3.zero;
        
        LevelPresenter.I.LevelProgressState
            .Where(state => state == StateType.Success)
            .Subscribe(_ =>
            {
                OnSuccess();
            }).AddTo(this);
    }

    private async void OnSuccess()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(0.7f));
        foreach (var obj in _objects)
        {
            obj.gameObject.SetActive(true);
            obj.transform.DOScale(1, 0.5f).SetEase(Ease.OutBack);
        }
        _successText.SetActive(true);
        _successText.transform.DOScale(_defaultScale, 0.5f).SetEase(Ease.OutBack);
        _confetti.Play();
    }
}
