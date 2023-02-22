using System;
using System.Collections;
using System.Collections.Generic;
using Lean.Touch;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class CarGenerator : MonoBehaviour
{
    [SerializeField] private CarBehavior _carPrefab;
    [SerializeField] private GameObject _startPoint;

    [SerializeField] private float _distanceThreshold = 2f;
    private CarBehavior _currentCar;
    
    
    void Start()
    {
        GenerateCar();

        this.UpdateAsObservable()
            .Subscribe(_ =>
            {
                if(_currentCar == null) return;
                if (CheckDistanceOver(_distanceThreshold, _currentCar.transform.position, _startPoint.transform.position))
                {
                    GenerateCar();
                }
            });
    }
    
    private void GenerateCar()
    {
        _currentCar = Instantiate(_carPrefab, _startPoint.transform.position, Quaternion.identity);
    }

    private static bool CheckDistanceOver(float threshold, Vector3 position1, Vector3 position2)
    {
        if (Vector3.Distance(position1, position2) > threshold)
        {
            return true;
        }
        return false;
    }
    

}
