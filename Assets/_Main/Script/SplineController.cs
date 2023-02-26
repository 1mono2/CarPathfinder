using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.U2D;

public class SplineController : MonoBehaviour
{
    [SerializeField] private SpriteShapeController _spriteShapeController;
    private Spline _spline;
    [SerializeField] private float _splineHeight;
    
    [SerializeField] private List<GameObject> _joints;
    
    private void Start()
    {
        _spline = _spriteShapeController.spline;
        SetSplineHeight();
        SetJoint();
        
    }

    private void SetSplineHeight()
    {
        if(_spline == null) return;
        for (int i = 0; i < _spline.GetPointCount(); i++)
        {
            _spline.SetHeight(i, _splineHeight);
        }
    }

    private void SetJoint()
    {
        for (int i = 0; i < _joints.Count; i++)
        {
            if(_joints[i] == null) continue;
            var point = i; // iだとSubscribeの中で値が変わってしまうので、pointに代入しておく
            _joints[point].ObserveEveryValueChanged(obj => obj.transform.localPosition)
                .Subscribe(position => _spline.SetPosition(point, position)).AddTo(this);
        }
    }
}
