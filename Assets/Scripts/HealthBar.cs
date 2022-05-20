using System.Collections;
using System.Globalization;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Battle.BattleCreatures.Healths
{
  public class HealthBar : MonoBehaviour
  {
    [SerializeField] private Image _healthImage;
    [SerializeField] private Image _shieldImage;

    [SerializeField] private TextMeshProUGUI _healthText;

    [SerializeField] private float _damagesDecreaseRate = 3;
    [SerializeField] private TextMeshProUGUI _battleEventText;

    private float _currentHealthPoints;
    private float _damages;
    private float _maxHealthPoints = 100;
    private float _shieldPoints;

    private static readonly int Percent = Shader.PropertyToID("_Percent");
    private static readonly int DamagesPercent = Shader.PropertyToID("_DamagesPercent");
    private static readonly int Steps = Shader.PropertyToID("_Steps");
    
    private float Health
    {
      set
      {
        _currentHealthPoints = Mathf.Clamp(value, 0, MaxHealthPoints);
        _healthImage.material.SetFloat(Percent, _currentHealthPoints / MaxHealthPoints);
        _healthText.text = _currentHealthPoints.ToString(CultureInfo.InvariantCulture);
      }
    }

    private float Damages
    {
      get => _damages;
      set
      {
        _damages = Mathf.Clamp(value, 0, MaxHealthPoints);
        _healthImage.material.SetFloat(DamagesPercent, _damages / MaxHealthPoints);
      }
    }

    private float MaxHealthPoints
    {
      get => _maxHealthPoints;
      set
      {
        _maxHealthPoints = value;
        var res = MaxHealthPoints / 2;

        _healthImage.material.SetFloat(Steps, res);
      }
    }

    private float ShieldPoints
    {
      set
      {
        _shieldPoints = value;
        _shieldImage.fillAmount = _shieldPoints / ShieldMaxPoints;
      }
    }

    private float ShieldMaxPoints { get; set; }

    public void Init(int maxHealthPoints, int currentHealthPoints, int maxShieldPoint)
    {
      LookAtCamera();
      MaxHealthPoints = maxHealthPoints;
      Health = currentHealthPoints;
      ShieldMaxPoints = maxShieldPoint;
    }

    public void UpdateHealthBar(float damage, float health, float shield)
    {
      StartCoroutine(ShowBattleEventText(damage));
      Damages += damage;
      Health = health;
      ShieldPoints = shield;
    }

    private IEnumerator ShowBattleEventText(float damage)
    {
      _battleEventText.gameObject.SetActive(true);
      _battleEventText.text = damage.ToString(CultureInfo.InvariantCulture);
      yield return new WaitForSeconds(2);
      _battleEventText.gameObject.SetActive(false);
    }

    private void Awake()
    {
      _healthImage.material = Instantiate(_healthImage.material);
      _battleEventText.gameObject.SetActive(false);
    }

    private void Update()
    {
      LookAtCamera();
      if (Damages > 0)
      {
        Damages -= _damagesDecreaseRate * Time.deltaTime;
      }
    }

    private void LookAtCamera()
    {
      var c = FindObjectsOfType<GameObject>().FirstOrDefault(x => x.name == "HealthbarAnchor");
      if (c == null)
        return;

      var dir = c.transform.position - transform.position;

      var rotation = Quaternion.LookRotation(dir * -1, Vector3.up);
      transform.rotation = rotation;
    }
  }
}