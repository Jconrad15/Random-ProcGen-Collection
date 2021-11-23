using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LowPolyLiquidShaderExampleScript : MonoBehaviour
{
    [SerializeField]
    private Material objectMaterial;

    [SerializeField]
    private float speed = 0.8f;

    [SerializeField]
    private Color color1;

    [SerializeField]
    private Color color2;

    // Start is called before the first frame update
    void Update()
    {
        objectMaterial.SetFloat("_speed", speed);
        objectMaterial.SetColor("_color1", color1);
        objectMaterial.SetColor("_color2", color2);
    }


}
