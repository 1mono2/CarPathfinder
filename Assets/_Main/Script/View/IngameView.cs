using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UniRx;

public class IngameView : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _goalCountText;
 
    private void Start()
    {
        LevelPresenter.I.GoalCounter.Count
            .Subscribe(count =>
            {
                var countText = $"{count} / {LevelPresenter.I.GoalCount} ".ToString(); 
                _goalCountText.text = countText;
            }).AddTo(this);
    }
}
