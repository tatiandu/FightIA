using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scroll : MonoBehaviour
{
    public Vector2 uvAnimationRate;

    Vector2 uvOffset;
    Renderer renderer_;


    void Start()
    {
        uvOffset = Vector2.zero;
        renderer_ = GetComponent<Renderer>();
    }

    void LateUpdate()
    {
        uvOffset += (uvAnimationRate * Time.deltaTime);
        if (renderer_.enabled)
        {
            renderer_.materials[0].SetTextureOffset("_MainTex", uvOffset);
            //renderer_.materials[materialIndex].SetTextureOffset("_BumpMap", uvOffset);
        }
    }
}
