using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class Actor : MonoBehaviour
{
    public float maxHP;
    public float CurrentHP { get; private set; }
    public float NormalizedHP { get { return Mathf.Clamp01(CurrentHP / maxHP); } }
    public float invulnerabilityTime;
    public float InvulnerableTimer { get; private set; }
    public bool IsInvulnerable => InvulnerableTimer > 0;

    public bool GetIsDead()
    {
        return CurrentHP <= 0;
    }

    public float delayDestroyOnDeath;

    // Actor Events
    public UnityEngine.Events.UnityEvent onHit = new UnityEngine.Events.UnityEvent();
    public UnityEngine.Events.UnityEvent onDeath = new UnityEngine.Events.UnityEvent();
    public UnityEngine.Events.UnityEvent onDestroyed = new UnityEngine.Events.UnityEvent();

    public virtual void Awake()
    {
        CurrentHP = maxHP;
    }

    public bool TakeDamage(float amount, Actor source = null, bool ignoreInvulnerability = false)
    {
        if (IsInvulnerable && !ignoreInvulnerability)
            return false;

        // Handle Damage
        CurrentHP -= amount;
        onHit?.Invoke();

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
        TakeDamage(maxHP, ignoreInvulnerability: true);
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

    private void OnDestroy()
    {
        // Invoke on destroyed
        onDestroyed?.Invoke();

        // remove all listeners to avoid null references
        onHit.RemoveAllListeners();
        onDeath.RemoveAllListeners();
        onDestroyed.RemoveAllListeners();
    }
}
