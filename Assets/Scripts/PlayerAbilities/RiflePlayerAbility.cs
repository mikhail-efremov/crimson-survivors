using DefaultNamespace;
using UnityEngine;

namespace PlayerAbilities
{
    public class RiflePlayerAbility : BasePlayerAbility
    {
        private Player _player;
        
        public override void Update(Player player)
        {
            _player = player;
            
            var enemies = DefaultNamespace.Battle.Instance.Enemies;
            Enemy closest = null;
            var minDist = Mathf.Infinity;
            var currentPos = _player.transform.position;
            foreach (var e in enemies)
            {
                if (e == null)
                    continue;
                var dist = Vector3.Distance(e.transform.position, currentPos);
                if (dist < minDist)
                {
                    closest = e;
                    minDist = dist;
                }
            }

            if (closest != null)
            {
                var difference = closest.transform.position - _player.transform.position;

                var distance = difference.sqrMagnitude;
                var direction = difference / distance;
                direction.Normalize();
                FireBullet(direction);
            }
        }
        
        private void FireBullet(Vector3 direction)
        {
            var template = Resources.Load<RifleProjectile>("Prefabs/RifleProjectile");
            var b = GameObject.Instantiate(template);
            b.transform.position = _player.ProjectileStartPosition.transform.position;
            b.Init(direction);
        }
    }
}