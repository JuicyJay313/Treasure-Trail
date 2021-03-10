using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallaxing : MonoBehaviour
{
    [SerializeField] Transform[] backgrounds;
    private float[] parallaxScales;
    [SerializeField] float smoothing = 1f;

    private Transform cam;
    private Vector3 previousCamPos;

    private void Awake()
    {
        cam = Camera.main.transform;
    }
    
    void Start()
    {
        previousCamPos = cam.position;
        parallaxScales = new float[backgrounds.Length];

        for(int i = 0; i < backgrounds.Length; i++)
        {
            parallaxScales[i] = backgrounds[i].position.z * -1;
        }
    }

    void Update()
    {
        ParallaxOnX();
        //ParallaxOnY();
    }

    private void ParallaxOnX()
    {
        for (int i = 0; i < backgrounds.Length; i++)
        {
            float parallaxX = (previousCamPos.x - cam.position.x) * parallaxScales[i];
            float backgroundTargetPosX = backgrounds[i].position.x + parallaxX;

            Vector3 backgroundTargetPos = new Vector3(backgroundTargetPosX, backgrounds[i].position.y,
                backgrounds[i].position.z);

            backgrounds[i].position = Vector3.Lerp(backgrounds[i].position, backgroundTargetPos, smoothing * Time.deltaTime);

            previousCamPos = cam.position;
        }
    }

    private void ParallaxOnY()
    {
        for (int i = 0; i < backgrounds.Length; i++)
        {
            float parallaxY = (previousCamPos.y - cam.position.y) * parallaxScales[i];
            float backgroundTargetPosY = backgrounds[i].position.y + parallaxY;

            Vector3 backgroundTargetPos = new Vector3(backgrounds[i].position.x, backgroundTargetPosY,
                backgrounds[i].position.z);

            backgrounds[i].position = Vector3.Lerp(backgrounds[i].position, backgroundTargetPos, smoothing * Time.deltaTime);

            previousCamPos = cam.position;
        }
    }
}
