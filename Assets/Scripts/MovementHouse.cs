using UnityEngine;

public class MovementHouse : MonoBehaviour
{
    public float speed = 5f; // Velocidad de movimiento
     float maxDiameter = 4.4f; // Diámetro máximo permitido antes de rebotar
    private Transform Center; // Centro del círculo
    private Vector2 direction; // Dirección actual del movimiento

    void Start()
    {
        // Genera una dirección inicial aleatoria
        direction = Random.insideUnitCircle.normalized;
        Center = GameObject.Find("Burbuja_0").transform;
    }

    void Update()
    {
        // Mueve el objeto en la dirección actual
        transform.Translate(direction * speed * Time.deltaTime);

        // Calcula la distancia desde el centro
        float distanceFromCenter = Vector2.Distance(Center.position, transform.position);

        // Cambia la dirección si el objeto supera el radio máximo permitido
        if (distanceFromCenter > maxDiameter / 2f)
        {
            // Genera una nueva dirección aleatoria
            direction = Random.insideUnitCircle.normalized;

            // Ajusta la posición al borde del círculo
            Vector2 directionFromCenter = ((Vector2)transform.position - (Vector2)Center.position).normalized;
            transform.position = (Vector2)Center.position + directionFromCenter * (maxDiameter / 2f);
        }
    }
}