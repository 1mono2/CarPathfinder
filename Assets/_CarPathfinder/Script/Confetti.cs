using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;


public class Confetti : MonoBehaviour
{
    [SerializeField] ParticleSystem _confetti;

    private void Start()
    {
        LevelPresenter.I.LevelProgressState
            .Where(state => state == StateType.Success)
            .Subscribe(_ =>
            {
                _confetti.Play();
            }).AddTo(this);
    }
}
