 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteScrolling : MonoBehaviour
{
    [SerializeField] float backgroundScrollSpeed = 0.5f;
    Material myMaterial;
    Vector2 offset;

    private void Start()
    {
        myMaterial = GetComponent<Renderer>().material;
        offset = new Vector2(backgroundScrollSpeed, 0f);
    }

    private void Update()
    {
        myMaterial.mainTextureOffset += offset * Time.deltaTime;
    }
}
