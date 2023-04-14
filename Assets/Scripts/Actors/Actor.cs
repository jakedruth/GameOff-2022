using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class Actor : MonoBehaviour
{
    public int maxHP;
    public int CurrentHP { get; private set; }
    public float NormalizedHP { get { return Mathf.Clamp01((float)CurrentHP / maxHP); } }
    public float invulnerabilityTime;
    public float InvulnerableTimer { get; private set; }
    public bool IsInvulnerable => InvulnerableTimer > 0;

    public bool GetIsDead()
    {
        return CurrentHP <= 0;
    }

    public float delayDestroyOnDeath;

    // Actor Events
    public UnityEngine.Events.UnityEvent OnTakeDamage = new();
    public UnityEngine.Events.UnityEvent onDeath = new();
    public UnityEngine.Events.UnityEvent onDestroyed = new();

    public virtual void Awake()
    {
        CurrentHP = maxHP;
    }

    public bool ApplyDamage(int damage, bool ignoreInvulnerability = false)
    {
        if (IsInvulnerable && !ignoreInvulnerability)
            return false;

        // Handle Damage
        CurrentHP -= damage;
        OnTakeDamage?.Invoke();

        // Invoke invulnerability timer
        if (invulnerabilityTime > 0)
            StartCoroutine(HandleInvulnerability());

        // Handle if health is <= 0
        if (CurrentHP <= 0)
        {
            OnDeath();
        }

        return true;
    }

    public IEnumerator HandleInvulnerability()
    {
        InvulnerableTimer = invulnerabilityTime;
        while (InvulnerableTimer > 0)
        {
            InvulnerableTimer -= Time.deltaTime;
            // TODO: Add flashing Iframes or something similar

            yield return null;
        }
    }

    /// <summary>
    /// Applies max damage, calling TakeDamage() 
    /// </summary>
    [ContextMenu("Instant Kill Actor")]
    public void InstantKill()
    {
        ApplyDamage(maxHP, ignoreInvulnerability: true);
    }

    /// <summary>
    /// Instantly removes the actor, with out calling OnDeath()
    /// </summary>
    public void Despawn()
    {
        Destroy(gameObject);
    }

    private void OnDeath()
    {
        // Call On Actor Death Event
        onDeath?.Invoke();

        // Destroy Actor
        Destroy(gameObject, delayDestroyOnDeath);
    }

    protected void OnDestroy()
    {
        // Invoke on destroyed
        onDestroyed?.Invoke();

        // remove all listeners to avoid null references
        OnTakeDamage.RemoveAllListeners();
        onDeath.RemoveAllListeners();
        onDestroyed.RemoveAllListeners();
    }
}
