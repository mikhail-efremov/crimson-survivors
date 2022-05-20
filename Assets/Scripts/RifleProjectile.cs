using System;
using DefaultNamespace;
using UnityEngine;

public class RifleProjectile : MonoBehaviour
{
    private Vector3 targetDirection;

    private void OnTriggerEnter(Collider other)
    {
        var p = other.gameObject.GetComponent<Enemy>();
        if (p == null)
            return;

        p.TakeDamage(5);
        Destroy(gameObject);
    }
    
    public void Init(Vector3 direction)
    {
        targetDirection = direction;
    }

    private void Update()
    {
        transform.position += targetDirection * 10f * Time.deltaTime;
    }
}