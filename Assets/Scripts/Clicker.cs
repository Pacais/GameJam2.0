using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Clicker : MonoBehaviour
{
    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        // Obtener la referencia al GameManager
        gameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Método que se llama cuando se hace clic en el objeto
    void OnMouseDown()
    {
        if (gameManager != null)
        {
           
            Debug.Log("Dinero: " + gameManager.money);
            gameManager.UpdateMoney(1); // Actualizar el texto del dinero
        }
        else
        {
            Debug.LogError("GameManager no encontrado!");
        }
    }
}
