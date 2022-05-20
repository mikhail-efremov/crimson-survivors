using System.Collections;
using System.Collections.Generic;
using Battle.BattleCreatures.Healths;
using PlayerAbilities;
using UnityEngine;

public class Player : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    [SerializeField] private float playerSpeed = 4.0f;
    private float jumpHeight = 1.0f;
    private float gravityValue = -9.81f;

    [SerializeField] private RifleProjectile rifleProjectile;
    [SerializeField] private GameObject projectileStartPosition;

    public GameObject ProjectileStartPosition => projectileStartPosition;
    
    private List<BasePlayerAbility> _abilities = new List<BasePlayerAbility>();

    private HealthBar _healthBar;
    private int _maxHealth;
    private int _health;

    private void Awake()
    {
        DefaultNamespace.Battle.Instance.Player = this;
    }

    private void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();

        _maxHealth = _health = 20;
        _healthBar = GetComponentInChildren<HealthBar>();
        _healthBar.Init(_maxHealth, _health, 0);
        
        _abilities.Add(new RiflePlayerAbility());

        StartCoroutine(AbilitiesUse());
    }

    private void Update()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        var move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        controller.Move(move * Time.deltaTime * playerSpeed);

        if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;
        }
        
        // Changes the height position of the player..
        if (Input.GetButtonDown("Jump") && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
        
        var t = transform;
        var position = t.position;
        position = new Vector3(position.x, 1, position.z);
        t.position = position;
    }
    
    private IEnumerator AbilitiesUse()
    {
        while (true)
        {
            foreach (var ability in _abilities)
            {
                ability.Update(this);
            }

            yield return new WaitForSeconds(1);
        }
    }

    public void TakeDamage(int damage)
    {
        _health -= damage;
        _healthBar.UpdateHealthBar(damage, _health, 0);
    }
}