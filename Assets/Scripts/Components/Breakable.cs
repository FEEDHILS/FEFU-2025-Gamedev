using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class Breakable : MonoBehaviour
{
    public MaterialTypes Material;
    public float Health = 1;
    public float WrongTypeMultiplier = 0.2f;

    public Item DropItem;
    public int Amount;
    [SerializeField] GameObject Parent;
    public bool OnBreakOverride = false; // Перезаписывает стандартное поведение при ломании.
    public bool Regenerate = false; // Полезно для построек
    public UnityEvent OnBreak;
    public UnityEvent OnHit;

    [SerializeField] GameObject HitParticles;
    [SerializeField] string sfx = "";

    float maxHealth;
    bool tookDamage = false;
    void Awake()
    {
        maxHealth = Health;
        if (Regenerate)
            StartCoroutine("Regenerating");
        
        if (!OnBreakOverride)
            OnBreak.AddListener(Break);
    }

    IEnumerator Regenerating()
    {
        yield return new WaitForSeconds(2.5f);
        if (!tookDamage)
            Health = maxHealth;
        
        tookDamage = false;
        StartCoroutine("Regenerating");
    }

    public void Break()
    {
        PlayerInventory.Instance.AddItem(DropItem, Amount);
        if (!Parent)
            Destroy(gameObject);
        else
            Destroy(Parent);
    }

    public void TakeDamage(float Damage, ViewmodelAttack.WeaponTypes Weapon)
    {
        float FinalDamage = Damage;
        OnHit?.Invoke();
        if (!IsSuitableWeapon(Weapon))
            FinalDamage *= WrongTypeMultiplier;

        Health -= FinalDamage;
        tookDamage = true;
        if (Health <= 0)
            OnBreak?.Invoke();
    }

    public void TakeDamage(float Damage, ViewmodelAttack.WeaponTypes Weapon, RaycastHit hit)
    {
        float FinalDamage = Damage;
        OnHit?.Invoke();
        if (!IsSuitableWeapon(Weapon))
            FinalDamage *= WrongTypeMultiplier;

        Health -= FinalDamage;
        VisualEffects(hit);
        tookDamage = true;
        if (Health <= 0)
            OnBreak?.Invoke();
    }

    void VisualEffects(RaycastHit hit)
    {
        if (HitParticles)
        {
            GameObject particles = Instantiate(HitParticles, hit.point, Quaternion.LookRotation(hit.normal));
            float liveTime = particles.GetComponent<ParticleSystem>().main.duration;
            Destroy(particles, liveTime);
        }

        if (sfx != "")
            AudioManager.instance.Play(sfx);
    }

    bool IsSuitableWeapon(ViewmodelAttack.WeaponTypes Weapon)
    {
        switch(Material)
        {
            case MaterialTypes.Wood:
                if (Weapon == ViewmodelAttack.WeaponTypes.Axe)
                    return true;
                else
                    return false;


            case MaterialTypes.Stone:
                if (Weapon == ViewmodelAttack.WeaponTypes.Pickaxe)
                    return true;
                else
                    return false;
        }

        Debug.LogWarning("Unkown MaterialType", this);
        return false;
    }
}