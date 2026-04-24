using UnityEngine;
using UnityEngine.Events;

public class SubmarineHealth : MonoBehaviour
{
    public static SubmarineHealth Instance { get; private set; }
    [Header("Здоровье подлодки")]
    public int maxHP = 3;
    public int currentHP;
    public UnityEvent onDamaged;
    public UnityEvent onDeath;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        currentHP = maxHP;
    }
    public void TakeDamage(int amount)
    {
        currentHP -= amount;
        Debug.Log($"Подлодка получила урон! HP: {currentHP}/{maxHP}");
        onDamaged?.Invoke();
        if (currentHP <= 0)
        {
            Die();
        }
    }
    private void Die()
    {
        Debug.Log("ПОДЛОДКА УНИЧТОЖЕНА!");
        if (DeathScreen.Instance) DeathScreen.Instance.Show();
        onDeath?.Invoke();
    }
    public void ResetHealth()
    {
        currentHP = maxHP;
    }
}