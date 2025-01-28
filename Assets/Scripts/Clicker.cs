using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Clicker : MonoBehaviour
{
    private GameManager gameManager;
    private AudioSource sonidodinero;
    private Coroutine stopAudioCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        // Obtener la referencia al GameManager
        gameManager = FindObjectOfType<GameManager>();
        sonidodinero = GetComponent<AudioSource>();
    }

    // Método que se llama cuando se hace clic en el objeto
    void OnMouseDown()
    {
        if (gameManager != null)
        {
            // Actualizar el dinero en el GameManager
            gameManager.UpdateMoney(1);

            // Reproducir el sonido si no está sonando
            if (sonidodinero != null)
            {
                if (!sonidodinero.isPlaying)
                {
                    sonidodinero.Play();
                }

                // Cancelar la corrutina de detener el sonido si se estaba ejecutando
                if (stopAudioCoroutine != null)
                {
                    StopCoroutine(stopAudioCoroutine);
                    stopAudioCoroutine = null;
                }
            }
        }
        
    }

    // Método que se llama cuando se deja de hacer clic en el objeto
    void OnMouseUp()
    {
        if (sonidodinero != null)
        {
            // Iniciar la corrutina para detener el audio después de 0.2 segundos
            stopAudioCoroutine = StartCoroutine(StopAudioAfterDelay(0.2f));
        }
    }

    // Corrutina que espera antes de detener el audio
    private IEnumerator StopAudioAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Detener el audio si sigue sonando
        if (sonidodinero.isPlaying)
        {
            sonidodinero.Stop();
        }

        stopAudioCoroutine = null;
    }
}
