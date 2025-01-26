using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Globalization;
using UnityEngine.SceneManagement;
using frogjil;


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

    void PagarDeuda()
    {
        deudatotal = 0;
        Debug.Log("PagarDeuda called. Current money: " + money);
        foreach (float[] credito in credit.creditos)
        {
            
            Debug.Log("Processing credit: " + string.Join(", ", credito));
            if (credito[2] > 0)
            {
                UpdateMoney(-credito[1]) ;
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
                credit.creditos.Remove(credito);
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
}