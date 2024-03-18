using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Ball : MonoBehaviour
{
    public float INITIAL_SPEED = 8f;
    public float SPEED = 8f;
    public float MAX_SPEED = 30f;
    public float SPEED_INCREASE_AMOUNT = 0.5f;
    public float TIME_INCREASE_SPEED = 0.5f;
    private float elapsedTime = 0f;

    public float tiempoInicial = 5f;
    public float tiempoRestante;

    // Posición Inicial de la pelota en el centro, X e Y
    private Vector2 posInicial = new Vector2(0f, 0f);
    private Vector2 velocidadParada = new Vector2(0f, 0f);

    public int bluescore;
    public int redscore;
    public Text redTextScore;
    public Text blueTextScore;

    public Text reloj;

    public Text notificationText;

    private bool juegoTerminado = false;

    private Rigidbody2D rb;

    void Start()
    {

        // Obtener el RigidBody2d del componente actual que es la Bola.
        rb = GetComponent<Rigidbody2D>();

        // Asignar a la Bola un Vector 2 con dirección derecha y velocidad 'SPEED'.
        rb.velocity = Vector2.right * SPEED;

        // Encuentra los objetos Text para el marcador
        redTextScore = GameObject.Find("redTextScore").GetComponent<Text>();
        blueTextScore = GameObject.Find("blueTextScore").GetComponent<Text>();

        reloj = GameObject.Find("reloj").GetComponent<Text>();

        // Objeto text para mostrar la notificación final de quién ha ganado.
        notificationText = GameObject.Find("notificationText").GetComponent<Text>();

        // Hacer inactivo el objeto de notificación
        notificationText.gameObject.SetActive(false);

        tiempoRestante = tiempoInicial;


    }

    private void ActualizarHora()
    {
        // Si el tiempo restante es mayor que 0 y el juego no ha terminado
        if (tiempoRestante > 0f && !juegoTerminado)
        {
            // Actualizar el tiempo restante
            tiempoRestante -= Time.deltaTime;

            // Convertir el tiempo restante a minutos y segundos
            int minutos = Mathf.Max(0, Mathf.FloorToInt(tiempoRestante / 60)); // Asegurar que los minutos no sean negativos
            int segundos = Mathf.Max(0, Mathf.FloorToInt(tiempoRestante % 60)); // Asegurar que los segundos no sean negativos

            // Actualizar el texto del contador
            reloj.text = string.Format("{0:00}:{1:00}", minutos, segundos);

            // Verificar si el tiempo ha llegado a cero
            if (tiempoRestante <= 0f)
            {
                tiempoRestante = 0f;

                // Detener el juego
                juegoTerminado = true;

                // Parar la bola
                rb.velocity = Vector2.zero;

                // Comprobar el ganador
                ComprobarGanador();

                Debug.Log("¡Tiempo agotado!");
            }
        }
    }


    // Método para reiniciar la pelota a la posición central y establecer la velocidad a 0.
    void reiniciarPelota()
    {
        rb.position = posInicial;
        rb.velocity = velocidadParada;
        SPEED = INITIAL_SPEED;
    }



    // Método Update que se actualiza en cada Frame del renderizado.
    void Update()
    {
        if (juegoTerminado)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                reiniciarPartida();
            }
            return;
        }

        if (!juegoTerminado)
        {
            // Aumentar el tiempo en la variable elapsedTime
            elapsedTime += Time.deltaTime;

            // Si la pelota está en posición central y su velocidad es 0, y el juego no ha terminado,
            // comprobamos si se pulsa la tecla espacio para comenzar el movimiento de la pelota.
            if (rb.position == posInicial && rb.velocity == velocidadParada)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    // Comenzar el movimiento de la pelota
                    rb.velocity = Vector2.right * SPEED;
                }
            }
            // Si el tiempo restante es mayor que 0 y el juego no ha terminado, actualizamos el tiempo restante.
            if (tiempoRestante > 0f)
            {
                ActualizarHora();
            }
            else
            {
                // Si el tiempo ha llegado a cero, detenemos el juego y comprobamos el ganador.
                juegoTerminado = true;
                rb.velocity = velocidadParada;
                ComprobarGanador();
            }
        }

    }


    private void ComenzarMovimientoConEspacio()
    {
        // Si la pelota está en posición central y su velocidad es 0 comprobamos si se pulsa la tecla espacio para comenzar el movimiento de la pelota.
        if (rb.position == posInicial && rb.velocity == velocidadParada)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                rb.velocity = Vector2.right * SPEED;
            }
        }
    }

    private void IncrementarVelocidadBola()
    {
        // Si el tiempo transcurrido es superior o igual a un tiempo definido en TIME_INCREASE_SPEED, aumentamos la velocidad de la pelota.
        if (elapsedTime >= TIME_INCREASE_SPEED)
        {
            if (SPEED < MAX_SPEED)
            {
                SPEED += SPEED_INCREASE_AMOUNT;
            }
            elapsedTime = 0f;
        }
    }

    private void ComprobarGoles()
    {
        // Finalizar el juego si el marcador del jugador uno llega a 5.
        if (bluescore >= 5)
        {
            ShowNotification("Enhorabuena Jugador1, has ganado!\nPara jugar otra vez pulsar espacio.");
            // Reiniciar partida (Hay que pulsar espacio para que reinicie).
            reiniciarPartida();
        }

        // Finalizar el juego si el marcador del jugador dos llega a 5.
        if (redscore >= 5)
        {
            ShowNotification("Enhorabuena Jugador2, has ganado!\nPara jugar otra vez pulsar espacio.");
            // Reiniciar partida (Hay que pulsar espacio para que reinicie).
            reiniciarPartida();
        }
    }

    private void ComprobarGanador()
    {
        // Solo comprobar si el juego no ha terminado
        if (bluescore == 0 && redscore == 0)
        {
            ShowNotification("Tiempo Agotado!\nLos marcadores están a 0, nadie ha ganado.\nPara jugar otra vez pulsar espacio.");
        }
        else if (bluescore > redscore)
        {
            ShowNotification("Tiempo Agotado!\nEnhorabuena Jugador1, has ganado!\nPara jugar otra vez pulsar espacio.");
        }
        else if (redscore > bluescore)
        {
            ShowNotification("Tiempo Agotado!\nEnhorabuena Jugador2, has ganado!\nPara jugar otra vez pulsar espacio.");
        }
    }

    // Método para mostrar una notificación en el Objeto Text de notificación.
    private void ShowNotification(string message)
    {
        notificationText.text = message;
        notificationText.gameObject.SetActive(true);
    }

    private void HideNotification()
    {
        notificationText.gameObject.SetActive(false);
    }

    // Método para reiniciar la partida
    private void reiniciarPartida()
    {
        // Reiniciar los marcadores y el tiempo restante
        reiniciarMarcadores();
        ActualizarLabelsMarcadores();
        HideNotification();
        tiempoRestante = tiempoInicial;
        
        juegoTerminado = false;

        // Reiniciar la posición y el movimiento de la pelota
        reiniciarPelota();
    }

    private void ActualizarLabelsMarcadores()
    {
        redTextScore.text = "0";
        blueTextScore.text = "0";
    }

    // Método para reinciar los Text a 0.
    private void reiniciarMarcadores()
    {
        redscore = 0;
        bluescore = 0;
    }

    // Método para cambiar de color un GameObject con una duración determinada.
    IEnumerator ChangeColorAndRestore(GameObject obj, Color newColor, float duration)
    {
        SpriteRenderer spriteRenderer = obj.GetComponent<SpriteRenderer>();
        Color originalColor = spriteRenderer.color;
        spriteRenderer.color = newColor;
        yield return new WaitForSeconds(duration);
        spriteRenderer.color = originalColor;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        // Incrementa la velocidad en cada colisión
        SPEED += SPEED_INCREASE_AMOUNT;

        // Limita la velocidad máxima
        SPEED = Mathf.Min(SPEED, MAX_SPEED);

        // Aplica la nueva velocidad a la pelota
        rb.velocity = rb.velocity.normalized * SPEED;

        if (other.gameObject.name == "paddleBlue")
        {
            Vector2 dir = new Vector2(1, hitFactor(transform.position, other.transform.position, other.collider.bounds.size.y)).normalized;
            rb.velocity = dir * SPEED;
        }
        else if (other.gameObject.name == "paddleRed")
        {
            Vector2 dir = new Vector2(-1, hitFactor(transform.position, other.transform.position, other.collider.bounds.size.y)).normalized;
            rb.velocity = dir * SPEED;
        }
        else if (other.gameObject.name == "LeftWall")
        {
            redscore++;
            redTextScore.text = redscore.ToString();
            StartCoroutine(ChangeColorAndRestore(other.gameObject, Color.magenta, 0.5f));
            reiniciarPelota();
        }
        else if (other.gameObject.name == "RightWall")
        {
            bluescore++;
            blueTextScore.text = bluescore.ToString();
            StartCoroutine(ChangeColorAndRestore(other.gameObject, Color.magenta, 0.5f));
            reiniciarPelota();
        }
    }

    float hitFactor(Vector2 ballPos, Vector2 racketPos, float racketHeight)
    {
        return (ballPos.y - racketPos.y) / racketHeight;
    }
}
