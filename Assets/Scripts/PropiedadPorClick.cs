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
        }

        public void OnComprarButtonClick()
        {
            if (simulacionBurbuja != null)
            {
                simulacionBurbuja.ComprarPropiedad(tipoPropiedad);
            }
        }

        public void OnVenderButtonClick()
        {
            if (simulacionBurbuja != null)
            {
                simulacionBurbuja.VenderPropiedad(tipoPropiedad);
            }
        }
    }
}
