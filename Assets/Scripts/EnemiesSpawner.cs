using System.Collections;
using DefaultNamespace;
using UnityEngine;

public class EnemiesSpawner : MonoBehaviour
{
    [SerializeField] private Enemy template;

    private IEnumerator Start()
    {
        while (true)
        {
            var player = DefaultNamespace.Battle.Instance.Player;

            var point = RandomPointInAnnulus(player.transform.position, 20f, 25f);
            Instantiate(template, point, Quaternion.identity, transform);
            yield return new WaitForSeconds(1f);
        }
    }
    
    public Vector3 RandomPointInAnnulus(Vector3 origin, float minRadius, float maxRadius)
    {
        var p = Random.insideUnitCircle.normalized * Random.Range(minRadius, maxRadius);
        return origin + new Vector3(p.x, 0, p.y);
    }
}