using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dinero : MonoBehaviour
{
    public Rigidbody2D rbDinero;
    // Start is called before the first frame update
    void Start()
    {
        rbDinero = GetComponent<Rigidbody2D>();
        Vector2 force = new Vector2(Random.Range(-1f, 1f), -10f).normalized; // Dirección aleatoria hacia arriba y a los lados
        rbDinero.AddForce(force * -5, ForceMode2D.Impulse);

    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y <= -10)
        {
            Destroy(gameObject); // Destruir el objeto
        }

    }
}
