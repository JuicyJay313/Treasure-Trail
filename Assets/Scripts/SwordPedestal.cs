using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordPedestal : MonoBehaviour
{
    [SerializeField] Sprite newSprite;

    public void ChangeSprite()
    {
        GetComponent<SpriteRenderer>().sprite = newSprite;
    }
}
