using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] int damage = 1;
    [SerializeField] int maxHealth = 2;
    [SerializeField] int currentHealth;
    //[SerializeField] GameObject deathVFX;
    //[SerializeField] float durationOfDeathVFX = 1f;
    [SerializeField] float durationOfDeathAnim = 1f;
    [SerializeField] float invincibilityPeriod = 1f;


    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
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
        GetComponent<EnemyMovement>().DieAnimation();
        FindObjectOfType<LevelManagement>().enemyDestroy();
        Destroy(gameObject, durationOfDeathAnim);
        //GameObject explosion = Instantiate(deathVFX, transform.position, transform.rotation);
        //Destroy(explosion, durationOfDeathVFX);
        //Play Particle effect
        //Destroy GameObject
        //Play Die Animation
        //Play Die sound FX
    }


}
