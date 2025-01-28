using System.Collections;
using UnityEngine;

public class FrogScript : MonoBehaviour
{
    private Animator animator;
    public AudioSource sonidoRana;
    private Coroutine stopAudioCoroutine;
    private bool isIdle = false; // Estado inicial del personaje (idle)


    void Start()
    {
        // Obtiene el componente Animator
        animator = GetComponent<Animator>();
        StartCoroutine(PlayIdleAnimation());

    }
    private void Update() {
        // Activa o desactiva la animación de idle
        if (isIdle)
        {
            animator.SetTrigger("isIdle");
        }
        else
        {
            animator.ResetTrigger("isIdle");
        }
        
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
    private System.Collections.IEnumerator PlayIdleAnimation()
    {
        while (true) // Bucle infinito
        {
            // Espera 2 segundos
            yield return new WaitForSeconds(2f);

            // Activa la animación cambiando el estado de isIdle
            isIdle = !isIdle;

            // Opcional: Puedes agregar un mensaje de depuración para verificar
            Debug.Log("Idle state changed to: " + isIdle);
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
