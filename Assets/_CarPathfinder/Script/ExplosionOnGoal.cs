using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class ExplosionOnGoal : MonoBehaviour
{
    [SerializeField] private float _power = 1f; 
    [SerializeField] private float _radius = 1f;
    [SerializeField] private float _upwardsModifier = 1f;
    
    private void Start()
    {
        LevelPresenter.I.LevelProgressState
            .Where(state => state == StateType.Success)
            .Subscribe(_ =>
            {
                AddExplosion2D();
                
            }).AddTo(this);
    }

    private void AddExplosion2D()
    {
        var explosionPosition = this.transform.position;
        Collider2D[] results = new Collider2D[10];
        var colliders = Physics2D.OverlapCircleNonAlloc(explosionPosition, _radius, results);

        foreach (var collider2D1 in results)
        {
            if(collider2D1 == null) continue;
            if (collider2D1.TryGetComponent<Rigidbody2D>(out var rb))
            {
                Debug.Log(collider2D1.name);
                rb.AddExplosionForce2D(explosionPosition, _power, _radius, _upwardsModifier);
                
            }
            var rbInParent = collider2D1.GetComponentInParent<Rigidbody2D>();
            if (rbInParent == null) continue;
            
            Debug.Log(rbInParent.name);
            rbInParent.AddExplosionForce2D(explosionPosition, _power, _radius, _upwardsModifier);
            
            
        }
    }

    

}

public static class Rigidbody2DExtension
{
    public static void AddExplosionForce2D(this Rigidbody2D rb,Vector2 explosionPosition, float power, float radius, float upwardsModifier = 0.0F, ForceMode2D mode2D = ForceMode2D.Impulse)
    {
        Vector2 direction = ((Vector2)rb.transform.position - explosionPosition).normalized;
        rb.AddForce(direction * power, mode2D);
        rb.AddForce(Vector2.up * upwardsModifier, mode2D);
    }
}
