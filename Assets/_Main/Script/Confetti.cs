using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class Confetti : MonoBehaviour
{
    [SerializeField] ParticleSystem _confetti;

    private void Start()
    {
        this.OnTriggerEnter2DAsObservable()
            .Subscribe(collider2D =>
            {
                if (collider2D.gameObject.CompareTag("Car"))
                    _confetti.Play();
            }).AddTo(this);
    }
}
