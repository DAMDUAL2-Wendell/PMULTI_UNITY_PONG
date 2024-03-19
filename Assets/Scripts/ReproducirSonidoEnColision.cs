using UnityEngine;

public class ReproducirSonidoEnColision : MonoBehaviour
{
    public AudioClip sonido; // AudioClip a reproducir
    private AudioSource audioSource; // Referencia al AudioSource

    void Start()
    {
        // Obtener el AudioSource o agregar uno si no existe
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.clip = sonido;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Reproducir el sonido si hay una colisión
        if (collision.gameObject.CompareTag("Pelota") && audioSource != null && audioSource.clip != null)
        {
            audioSource.PlayOneShot(audioSource.clip);
        }
    }
}
