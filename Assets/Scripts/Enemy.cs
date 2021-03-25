using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header ("Enemy Stats")]
    [SerializeField] int damage = 1;
    [SerializeField] int maxHealth = 2;
    [SerializeField] int currentHealth;

    [Header ("Death Properties")]
    [SerializeField] GameObject deathAnimation;
    [SerializeField] float durationOfDeathAnim = 0.5f;
    [SerializeField] float invincibilityPeriod = 1f;


    void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        
    }

    public int GetDamage() { return damage; }

    public void TakeDamage(int damage)
    {
        StartCoroutine(HandleHit(damage));
        // Play Hurt sound FX
    }

    IEnumerator HandleHit(int damage)
    {
        gameObject.layer = 14;
        damage = damage > 1 ? 1 : 1;
        currentHealth -= damage;
        GetComponent<EnemyMovement>().HurtAnimation();
        yield return new WaitForSeconds(invincibilityPeriod);
        if(currentHealth <= 0) { Die(); }
        gameObject.layer = 12;
    }

    private void Die()
    {
        Debug.Log(gameObject.name + " died!");
        FindObjectOfType<LevelManagement>().enemyDestroy();
        Destroy(gameObject);
        GameObject death = Instantiate(deathAnimation, transform.position, transform.rotation);
        death.GetComponent<Animator>().SetTrigger(gameObject.name);
        Destroy(death, durationOfDeathAnim);
    }


}
