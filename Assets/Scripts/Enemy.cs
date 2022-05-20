using System.Collections;
using UnityEngine;

namespace DefaultNamespace
{
    public class Enemy : MonoBehaviour
    {
        public float speed = 2.0f;
        private Player _target;
        [SerializeField] private GameObject damageView;

        private int _maxHealth;
        private int _health;
        
        private void Awake()
        {
            _target = FindObjectOfType<Player>();
            _maxHealth = _health = 10;
        }

        private IEnumerator Start()
        {
            Battle.Instance.Enemies.Add(this);
            
            while (true)
            {
                if (Vector3.Distance(transform.position, _target.transform.position) < 1f)
                {
                    _target.TakeDamage(1);

                    TakeDamage(1);
                }

                yield return new WaitForSeconds(1);
            }
        }

        private void Update()
        {
            var step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, _target.transform.position, step);
        }

        public void TakeDamage(int damage)
        {
            _health -= damage;

            if (_health <= 0)
            {
                Battle.Instance.Enemies.Remove(this);
                Destroy(gameObject);
            }
            else
            {
                StartCoroutine(DamageRoutine());
            }
        }

        private IEnumerator DamageRoutine()
        {
            damageView.SetActive(true);
            yield return new WaitForSeconds(.1f);
            damageView.SetActive(false);
        }
    }
}