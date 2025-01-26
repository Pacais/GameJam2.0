using UnityEngine;

namespace frogjil
{
    public class PropiedadPorClick : MonoBehaviour
    {
        public string tipoPropiedad;
        private SimulacionBurbuja simulacionBurbuja;


        void Start()
        {
            simulacionBurbuja = FindObjectOfType<SimulacionBurbuja>();
            Debug.Log("SimulacionBurbuja encontrada: " + (simulacionBurbuja != null));
        }

        public void OnComprarButtonClick()
        {
            Debug.Log($"OnComprarButtonClick llamado en {gameObject.name} para {tipoPropiedad}");
            if (simulacionBurbuja != null)
            {
                simulacionBurbuja.ComprarPropiedad(tipoPropiedad);
                 
            }
            else
            {
                Debug.LogError("SimulacionBurbuja no encontrada!");
            }
        }

        public void OnVenderButtonClick()
        {
            Debug.Log($"OnVenderButtonClick llamado en {gameObject.name} para {tipoPropiedad}");
            if (simulacionBurbuja != null)
            {
                simulacionBurbuja.VenderPropiedad(tipoPropiedad);
            }
            else
            {
                Debug.LogError("SimulacionBurbuja no encontrada!");
            } 
        }
    }
}
