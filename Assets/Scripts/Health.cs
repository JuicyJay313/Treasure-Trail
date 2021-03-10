using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [SerializeField] private int maxHealth = 6;
    [SerializeField] private Sprite fullHeart;
    [SerializeField] private Sprite halfHeart;
    [SerializeField] private Sprite emptyHeart;
    [SerializeField] private Image[] hearts;

    // TODO Serialized only for testing
    [SerializeField] private int currentHealth;
   
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        DisplayHearts();
    }

    private void DisplayHearts()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i == Mathf.Ceil(currentHealth / 2))
            {
                if (currentHealth % 2 != 0)
                {
                    hearts[i].sprite = halfHeart;
                }
                else if (currentHealth % 2 == 0)
                {
                    hearts[i].sprite = emptyHeart;
                }
            }


            if (i < Mathf.Ceil(maxHealth / 2))
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }

            if (i > Mathf.Ceil(currentHealth / 2))
            {
                hearts[i].sprite = emptyHeart;
            }
            else if (i < Mathf.Ceil(currentHealth / 2))
            {
                hearts[i].sprite = fullHeart;
            }
        }
    }

    public void LoseHealth(int damageReceived)
    {
        currentHealth -= damageReceived;
        DisplayHearts();
    }

    public Image[] GetHeartImages() { return hearts; }

    public void SetHearts(Image[] receivedHearts)
    {
        hearts = receivedHearts;
    }

    public int GetCurrentHealth() { return currentHealth; }

    
}
