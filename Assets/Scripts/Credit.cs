using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Credit : MonoBehaviour
{
    private GameManager gameManager;
    public bool cooldown;
    public float multiplier = 1;
    public List<float[]> creditos;
    Renderer renderer;


    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        creditos = new List<float[]>();
        renderer = GetComponent<Renderer>();

    }

    void Update()
    {
        
    }

    void OnMouseDown()
    {
        if (!cooldown)
        {
            bajarTranparencia();
            gameManager.ActivateCooldown();
            // Que se quite la opacidad del botón
            AddCredit();
            
        }
    }

    void AddCredit(){
        float[] credito = {50f * multiplier, 50f*multiplier /30f, 30f};
        
        creditos.Add(credito);
        multiplier *= 2;
         gameManager.UpdateMoney(credito[0]); // Actualizar el texto del dinero
    }
    public void bajarTranparencia(){
        Color color = renderer.material.color;
        color.a = 0.5f;
        renderer.material.color = color;
    }
      public void RestaurarTransparencia()
    {
        // Lógica para restaurar la transparencia del botón
        Color color = renderer.material.color;
        color.a = 1f; // Ajusta el valor de alfa para hacer el botón completamente opaco
        renderer.material.color = color; // Actualiza el color del botón
    }
}
