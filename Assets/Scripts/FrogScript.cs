using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogScript : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        // Obtiene el componente Animator
        animator = GetComponent<Animator>();
    
    }
        void OnMouseDown()
    {
        // Activa el trigger para reproducir la animación
        animator.SetTrigger("Click");
    }
}
