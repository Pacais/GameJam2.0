using System;
using System.Collections.Generic;
using UnityEngine;

namespace frogjil
{
    public enum TipoPropiedad
    {
        Piso = 0,
        Casa = 1,
        Luxury_Casa = 2,
        Edificio = 3,
        Club_nocturno = 4,
        Rascacielos = 5,
        Centro_Comercial = 6,
        Aeropuerto_Privado = 7,
        Base_Espacial = 8
    }

    public class SimulacionBurbuja : MonoBehaviour
    {
        private GameManager gameManager;

        // Parámetros iniciales
        
        private int anio = 2008;
        private int mes = 1;
        private int clicsMensuales = 3000;
        private float gClicBase = 10f;
        private float mejoraClic = 0.5f;

        // Propiedades por Tier (precios base, rentas y riesgo asociados)
        public Dictionary<string, (float precio, float renta, float riesgo)> propiedades = new Dictionary<string, (float, float, float)>
        {
            { "Piso", (50f, 2f, 0.03f) },
            { "Casa", (100f, 5f, 0.05f) },
            { "Luxury_Casa", (500f, 25f, 0.1f) },
            { "Edificio", (1000f, 50f, 0.15f) },
            { "Club_nocturno", (2000f, 100f, 0.2f) },
            { "Rascacielos", (5000f, 250f, 0.3f) },
            { "Centro_Comercial", (10000f, 500f, 0.4f) },
            { "Aeropuerto_Privado", (20000f, 1000f, 0.5f) },
            { "Base_Espacial", (50000f, 2500f, 0.7f) }
        };
        public GameObject Bubble;

        private Dictionary<string, int> propiedadesCompradas = new Dictionary<string, int>
        {
            { "Piso", 0 },
            { "Casa", 0 },
            { "Luxury_Casa", 0 },
            { "Edificio", 0 },
            { "Club_nocturno", 0 },
            { "Rascacielos", 0 },
            { "Centro_Comercial", 0 },
            { "Aeropuerto_Privado", 0 },
            { "Base_Espacial", 0 }
        };

        private float inflacion = 0.1f;
        public float rentaTotal = 0f;
        private float gPClic;
        private bool isAngry = false;
        private float burbuja = 0f;
        private float explosionBurbuja = 15f;

        // Parámetros de la explosión
        private int frecuenciaExplosion = 24;      // Explosión cada 24 meses si no se alcanza el umbral
        private float ajustePrecio = 0.3f;         // Reducción del 30% en los precios tras la explosión
        private float ajusteRenta = 0.5f;          // Reducción del 50% en las rentas tras la explosión
        int enfado;
        private int mesesDesdeExplosion = 0;
        private List<GameObject> edificiosInstanciados = new List<GameObject>();

        public GameObject[] Edificio;
        public GameObject Rana;

        // Inicialización
        void Start()
        {
            gameManager = FindObjectOfType<GameManager>();
            gPClic = gClicBase;
            foreach (var tipo in propiedades.Keys)
            {
                propiedadesCompradas[tipo] = 0;
            }
        }

        public void ComprarPropiedad(string tipo)
        {
            if (gameManager.money >= propiedades[tipo].precio)
            {
                gameManager.UpdateMoney(-propiedades[tipo].precio);
                propiedadesCompradas[tipo]++;
                rentaTotal += propiedades[tipo].renta;
                propiedades[tipo] = (propiedades[tipo].precio * (1 + inflacion), propiedades[tipo].renta, propiedades[tipo].riesgo);
                burbuja += propiedades[tipo].riesgo;
                int index = (int)Enum.Parse(typeof(TipoPropiedad), tipo);
                GameObject nuevoEdificio = Instantiate(Edificio[index], new Vector3(0, 0, 0), Quaternion.identity);
                edificiosInstanciados.Add(nuevoEdificio);

                if (burbuja > explosionBurbuja)
                {
                    if (UnityEngine.Random.Range(0f, 2f) > 1)
                    {
                        ExplosiónBurbuja();
                        foreach (var edificio in edificiosInstanciados)
                        {
                            Destroy(edificio);
                        }
                        Bubble.GetComponent<Animator>().Play("Explotion");
                    }
                }
                gameManager.ActualizarPreciosUI(propiedades); // Actualizar los precios en la UI
            }
        }

        public void VenderPropiedad(string tipo)
        {
            if (propiedadesCompradas.ContainsKey(tipo) && propiedadesCompradas[tipo] > 0)
            {
                propiedadesCompradas[tipo]--;
                rentaTotal -= propiedades[tipo].renta;
                gameManager.AddMoney(propiedades[tipo].precio * 0.8f); // Recuperar el 80% del precio de compra
            }
        }

        public void CobrarLasRentas()
        {
            float ingresos = 0f;
            foreach (var propiedad in propiedadesCompradas)
            {
                ingresos += propiedad.Value * propiedades[propiedad.Key].renta;
                Debug.Log(ingresos.ToString());
            }
            if(isAngry){
                enfado++;
                if(enfado>2){
                    Rana.GetComponent<Animator>().SetBool("Angry",false);
                    isAngry=false;
                    enfado=0;
                }
            }
            gameManager.AddMoney(ingresos);
            
        }

        void ExplosiónBurbuja()
        {
          
            // Crear una lista con las claves del diccionario
            var tiposPropiedades = new List<string>(propiedades.Keys);

            // Ajustar precios y rentas
            foreach (var tipo in tiposPropiedades)
            {
                var propiedad = propiedades[tipo];
                propiedades[tipo] = (propiedad.precio * ajustePrecio, propiedad.renta * ajusteRenta, propiedad.riesgo);
            }

            rentaTotal = 0f;
            foreach (var propiedad in propiedadesCompradas)
            {
                rentaTotal += propiedades[propiedad.Key].renta * propiedad.Value;
            }
            Rana.GetComponent<Animator>().SetBool("Angry",true);
            isAngry=true;
            burbuja = 0f; // Reiniciar el riesgo
            mesesDesdeExplosion = 0; // Reiniciar el contador
            explosionBurbuja = 15;
           
        }

    }
}