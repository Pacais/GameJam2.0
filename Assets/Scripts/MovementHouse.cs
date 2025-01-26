using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementHouse : MonoBehaviour
{
     float speed = 1.5f; // Velocidad de movimiento
     float diametro = 4.5f; // Diámetro máximo permitido antes de rebotar
    private Vector2 direction; // Dirección de movimiento
     Transform centerTransform; // Punto central para calcular rebotes

    // Start is called before the first frame update
    void Start()
    {
        // Si no se asigna un Transform en el Inspector, buscar uno por nombre o tag
        if (centerTransform == null)
        {
            centerTransform = GameObject.Find("Burbuja_0")?.transform; // Busca por nombre
            if (centerTransform == null)
            {
                Debug.LogError("No se asignó un 'centerTransform' y no se encontró un objeto llamado 'CenterPoint' en la jerarquía.");
                return;
            }
        }

        // Generar una dirección aleatoria inicial
        SetRandomDirection();
    }

    // Update is called once per frame
    void Update()
    {
        if (centerTransform == null) return;

        // Mover el objeto en la dirección actual
        transform.Translate(direction * speed * Time.deltaTime);

        // Calcular la distancia desde el punto central
        float distanceFromCenter = Vector2.Distance(centerTransform.position, transform.position);

        // Cambiar la dirección si el objeto supera el diámetro especificado
        if (distanceFromCenter > diametro / 2f)
        {
            // Ajustar la posición al borde del círculo
            Vector2 directionFromCenter = (transform.position - centerTransform.position).normalized;
            transform.position = (Vector2)centerTransform.position + directionFromCenter * (diametro / 2f);

            // Asignar una nueva dirección aleatoria
            SetRandomDirection();
        }
    }

    void SetRandomDirection()
    {
        // Generar una nueva dirección aleatoria
        float angle = Random.Range(0f, 360f); // Ángulo aleatorio en grados
        direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)).normalized;
    }
}