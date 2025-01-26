using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickScript : MonoBehaviour
{
   private Animator animator;
   public GameObject billete;
   public GameObject moneda;
   public Rigidbody2D rbBillete;

    void Start()
    {
        // Obtiene el componente Animator
        animator = GetComponent<Animator>();
        
    }
        void OnMouseDown()
    {
        // Activa el trigger para reproducir la animación
        animator.SetTrigger("Click");
        ThrowMoney();
    }

    void ThrowMoney(){
        int nbilletes = Random.Range(2, 4);
        int nmonedas = Random.Range(3, 6);

        for(int i=0; i<nbilletes; i++){
            Instantiate(billete, new Vector2(Random.Range(-8,-4),-2), Quaternion.identity);
            
            
        }
        for(int i=0; i<nmonedas; i++){
            Instantiate(moneda, new Vector2(Random.Range(-8,-4),-2), Quaternion.identity);
            
            
        }


        
    }

}
