using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Ball : MonoBehaviour
{
    public float speed = 10f;
    public float maxSpeed = 40f;
    public float speedIncreaseAmount = 2f;
    public float tiempoAumentarVelocidad = 1f;
    private float tiempoTranscurrido = 0f;

    // Posición Inicial de la pelota en el centro, X e Y
    private Vector2 posInicial = new Vector2(10f,6f);

    public int bluescore;
    public int redscore;
    public Text redTextScore;
    public Text blueTextScore;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.right * speed;

        // Encuentra los objetos Text para el marcador
        redTextScore = GameObject.Find("redTextScore").GetComponent<Text>();
        blueTextScore = GameObject.Find("blueTextScore").GetComponent<Text>();
    }

    void reiniciarPelota()
    {
        rb.position = posInicial;
        rb.velocity = Vector2.right * speed;
    }

    private void Update()
    {
        tiempoTranscurrido += Time.deltaTime;
        if (tiempoTranscurrido >= tiempoAumentarVelocidad)
        {
            if (speed < maxSpeed)
            {
                speed += speedIncreaseAmount;
            }
            tiempoTranscurrido = 0f;
        }
        
    }

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
        speed += speedIncreaseAmount;

        // Limita la velocidad máxima
        speed = Mathf.Min(speed, maxSpeed);

        // Aplica la nueva velocidad a la pelota
        rb.velocity = rb.velocity.normalized * speed;

        if (other.gameObject.name == "paddleBlue")
        {
            Vector2 dir = new Vector2(1, hitFactor(transform.position, other.transform.position, other.collider.bounds.size.y)).normalized;
            rb.velocity = dir * speed;
        }
        else if (other.gameObject.name == "paddleRed")
        {
            Vector2 dir = new Vector2(-1, hitFactor(transform.position, other.transform.position, other.collider.bounds.size.y)).normalized;
            rb.velocity = dir * speed;
        }
        else if (other.gameObject.name == "LeftWall")
        {
            redscore++;
            redTextScore.text = redscore.ToString();
            StartCoroutine(ChangeColorAndRestore(other.gameObject, Color.magenta, 0.5f));
            //reiniciarPelota();
        }
        else if (other.gameObject.name == "RightWall")
        {
            bluescore++;
            blueTextScore.text = bluescore.ToString();
            StartCoroutine(ChangeColorAndRestore(other.gameObject, Color.magenta, 0.5f));
            //reiniciarPelota();
        }
    }

    float hitFactor(Vector2 ballPos, Vector2 racketPos, float racketHeight)
    {
        return (ballPos.y - racketPos.y) / racketHeight;
    }
}
