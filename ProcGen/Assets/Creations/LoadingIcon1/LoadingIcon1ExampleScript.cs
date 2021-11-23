using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingIcon1ExampleScript : MonoBehaviour
{
    [SerializeField]
    private Material objectMaterial;

    [SerializeField]
    private Color color;

    // Start is called before the first frame update
    void Update()
    {
        objectMaterial.SetColor("_color", color);
    }
}
