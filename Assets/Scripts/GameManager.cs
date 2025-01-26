using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Globalization;
using UnityEngine.SceneManagement;
using frogjil;
using Unity.VisualScripting;


public class GameManager : MonoBehaviour
{
    public float money=0;
    public TextMeshProUGUI dateText; // Referencia al componente Text para mostrar la fecha
    public TextMeshProUGUI moneyText; // Referencia al componente Text para mostrar el dinero
    public TextMeshProUGUI rentaText; // Referencia al componente Text para mostrar la renta
    public TextMeshProUGUI creditText; // Referencia al componente Text para mostrar el crédito;
    public TextMeshProUGUI informeText; // Referencia al componente Text para mostrar el informe mensual

    private DateTime currentDate;
    private DateTime targetDate;
    private float timer;
    private const float timeInterval = 5f; // 5 segundos
    public Credit credit;
    public GameObject gameOver;
    private SimulacionBurbuja simulacionBurbuja;
    private string InformeMensual;
    float deudatotal = 0;
    public Dictionary<string, TextMeshProUGUI> precioTextos; // Diccionario para asociar cada tipo de propiedad con su TextMeshProUGUI

    public TextMeshProUGUI pisoText;
    public TextMeshProUGUI casaText;
    public TextMeshProUGUI luxuryCasaText;
    public TextMeshProUGUI edificioText;
    public TextMeshProUGUI clubNocturnoText;
    public TextMeshProUGUI rascacielosText;
    public TextMeshProUGUI centroComercialText;
    public TextMeshProUGUI aeropuertoPrivadoText;
    public TextMeshProUGUI baseEspacialText;

    void Awake()
    {
        precioTextos = new Dictionary<string, TextMeshProUGUI>
        {
            { "Piso", pisoText },
            { "Casa", casaText },
            { "Luxury_Casa", luxuryCasaText },
            { "Edificio", edificioText },
            { "Club_nocturno", clubNocturnoText },
            { "Rascacielos", rascacielosText },
            { "Centro_Comercial", centroComercialText },
            { "Aeropuerto_Privado", aeropuertoPrivadoText },
            { "Base_Espacial", baseEspacialText }
        };
    }

    void Start()
    {
        Screen.SetResolution(1920, 1080, true);
        Camera.main.aspect = 1920f / 1080f;
        credit = FindObjectOfType<Credit>();
        simulacionBurbuja = FindObjectOfType<SimulacionBurbuja>();
        currentDate = new DateTime(2008, 1, 1); // Fecha inicial
        targetDate = currentDate; // Inicialmente, la fecha objetivo es la misma que la fecha actual
        UpdateDateText();
        UpdateMoney(money);
        ActualizarPreciosUI(simulacionBurbuja.propiedades);
        Debug.Log("Game started. Initial date: " + currentDate.ToString("MMMM yyyy", new CultureInfo("es-ES")));
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= timeInterval)
        {
            InformeMensual="";
            timer = 0f;
            IncrementMonth();
            UpdateDateText();
            PagarDeuda();
            UpdateDateText();
            InformeMensual = "Fecha: " + currentDate.ToString("MMMM yyyy", new CultureInfo("es-ES")) + "\n"
                + "Renta total: " + simulacionBurbuja.rentaTotal.ToString("C", new CultureInfo("es-ES")) + "\n"
                + "Deuda total: " + deudatotal.ToString("C", new CultureInfo("es-ES")) + "\n"
                + "Créditos restantes: " + credit.creditos.Count + "\n";
            informeText.text = InformeMensual;
        }

        // Verificar si han pasado tres meses desde que se activó el cooldown
        if (credit.cooldown && currentDate >= targetDate)
        {
            credit.RestaurarTransparencia();
            credit.cooldown = false;
            Debug.Log("Cooldown set to false");
        }
    }

    void IncrementMonth()
    {
        currentDate = currentDate.AddMonths(1);
        Debug.Log("Month incremented. New date: " + currentDate.ToString("MMMM yyyy", new CultureInfo("es-ES")));
        simulacionBurbuja.CobrarLasRentas();
        rentaText.text = simulacionBurbuja.rentaTotal.ToString("C", new CultureInfo("es-ES"));
        //simulacionBurbuja.VerificarExplosionBurbuja();
    }

    void UpdateDateText()
    {
        dateText.text = currentDate.ToString("MMMM yyyy", new CultureInfo("es-ES"));
        Debug.Log("Date text updated: " + dateText.text);
    }

    public void UpdateMoney(float moneySum)
    {
        money += moneySum;
        moneyText.text = money.ToString("C", new CultureInfo("es-ES"));
        Debug.Log("Money text updated: " + money);
    }

    public void ActivateCooldown()
    {
        credit.cooldown = true;
        targetDate = currentDate.AddMonths(3); // Establecer la fecha objetivo a tres meses en el futuro
        Debug.Log("Cooldown activated. Target date: " + targetDate.ToString("MMMM yyyy", new CultureInfo("es-ES")));
    }

    public void PagarDeuda()
    {
        for (int i = credit.creditos.Count - 1; i >= 0; i--)
        {
            var credito = credit.creditos[i];
            if (credito[2] > 0)
            {
                UpdateMoney(-credito[1]);
                creditText.text = credito[1].ToString("F2");
                credito[2]--; // Decrementar el número de pagos restantes
                deudatotal += credito[1];
                if (money < 0)
                {
                    GameOver();
                }
                Debug.Log("Payment made. Remaining payments: " + credito[2] + ". New money: " + money);
            }
            else
            {
                credit.creditos.RemoveAt(i);
                Debug.Log("Credit removed: " + string.Join(", ", credito));
            }
        }
    }

    void GameOver()
    {
        Time.timeScale = 0;
        gameOver.SetActive(true);
        Debug.Log("Game Over");
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ReiniciarJuego();
        }
    }

    private void ReiniciarJuego()
    {
        Time.timeScale = 1;
        Debug.Log("Reiniciando juego");
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name); 
    }

    public void AddMoney(float amount)
    {
        UpdateMoney(amount);
    }

    public void SubtractMoney(float amount)
    {
        UpdateMoney(-amount);
    }

    public void ActualizarPreciosUI(Dictionary<string, (float precio, float renta, float riesgo)> propiedades)
    {
        foreach (var tipo in propiedades.Keys)
        {
            if (precioTextos.ContainsKey(tipo))
            {
                precioTextos[tipo].text = $"Precio : {propiedades[tipo].precio}";
            }
        }
    }

    // Actualizar precios UI


    
}