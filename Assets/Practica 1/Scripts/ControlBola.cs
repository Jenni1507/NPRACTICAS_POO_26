using UnityEngine;

public class ControlBola : MonoBehaviour
{
    public Rigidbody rb;
    public float velocidadDeApuntado = 5f;
    public float fuerzaDeLanzamiento = 1000f;
    public float lmiteIzquierdo = -2f;
    public float lmiteDerecho = 2f;
    private bool haSidoLanzada = false;

    public Transform CamaraPrincipal;
    public Vector3 offset = new Vector3(0f, 2f, -6f);
    public float suavidadCamara = 5f;

    void Update()
    {
        if (!haSidoLanzada)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Lanzar();
            }

            Apuntar();
        }
    }

    void Apuntar()
    {
        float inputHorizontal = Input.GetAxis("Horizontal");
        transform.Translate(Vector3.right * inputHorizontal * velocidadDeApuntado * Time.deltaTime);

        Vector3 posicionActual = transform.position;
        posicionActual.x = Mathf.Clamp(posicionActual.x, lmiteIzquierdo, lmiteDerecho);
        transform.position = posicionActual;
    }

    void Lanzar()
    {
        haSidoLanzada = true;
        rb.AddForce(Vector3.forward * fuerzaDeLanzamiento);

        if (CamaraPrincipal != null)
        {
            // No la hacemos hija para evitar que rote con la bola
            // Solo usamos el seguimiento por posición
        }
    }

    void LateUpdate()
    {
        if (haSidoLanzada && CamaraPrincipal != null)
        {
            // Calcula la posición deseada detrás de la bola
            Vector3 posicionDeseada = transform.position + offset;
            CamaraPrincipal.position = Vector3.Lerp(
                CamaraPrincipal.position,
                posicionDeseada,
                suavidadCamara * Time.deltaTime
            );

            // Mira hacia adelante, sin seguir la rotación de la bola
            Vector3 lookDirection = new Vector3(transform.position.x, CamaraPrincipal.position.y, transform.position.z + 5f);
            CamaraPrincipal.LookAt(lookDirection);
        }
    }
}
