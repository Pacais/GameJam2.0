using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebGlResolution : MonoBehaviour
{
    void Start()
    {
        // Solo aplicamos la configuración de resolución si el juego está corriendo en WebGL
        #if UNITY_WEBGL
        SetWebGLResolution();
        #endif
    }

    private void SetWebGLResolution()
    {
        // Establece la resolución en 1920x1080
        Screen.SetResolution(1920, 1080, false);

        Debug.Log("Resolución ajustada a 1920x1080 para WebGL");
    }

}
