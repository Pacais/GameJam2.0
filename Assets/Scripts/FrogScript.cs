using System.Collections;
using UnityEngine;

public class FrogScript : MonoBehaviour
{
    private Animator animator;
    public AudioSource sonidoRana;
    private Coroutine stopAudioCoroutine;

    void Start()
    {
        // Obtiene el componente Animator
        animator = GetComponent<Animator>();
        StartCoroutine(PlayIdleAnimationLoop()); // Inicia la corrutina para la animación en bucle
    }

    void OnMouseDown()
    {
        // Activa el trigger para reproducir la animación
        animator.SetTrigger("Click");

        // Reproduce el sonido si no está ya sonando
        if (sonidoRana != null)
        {
            if (!sonidoRana.isPlaying)
            {
                sonidoRana.Play();
            }

            // Cancela cualquier corrutina activa que intente detener el audio
            if (stopAudioCoroutine != null)
            {
                StopCoroutine(stopAudioCoroutine);
                stopAudioCoroutine = null;
            }
        }
    }

    void OnMouseUp()
    {
        // Inicia la corrutina para detener el sonido después de 0.2 segundos
        if (sonidoRana != null && sonidoRana.isPlaying)
        {
            stopAudioCoroutine = StartCoroutine(StopAudioAfterDelay(0.2f));
        }
    }

    private IEnumerator PlayIdleAnimationLoop()
    {
        while (true) // Bucle infinito para repetir la animación
        {
            yield return new WaitForSeconds(2f); // Espera 2 segundos
            animator.SetTrigger("Idle"); // Activa la animación de Idle
        }
    }

    private IEnumerator StopAudioAfterDelay(float delay)
    {
        // Espera el tiempo especificado
        yield return new WaitForSeconds(delay);

        // Detiene el audio si sigue sonando
        if (sonidoRana != null && sonidoRana.isPlaying)
        {
            sonidoRana.Stop();
        }

        stopAudioCoroutine = null;
    }
}
