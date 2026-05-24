using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Referências")]
    public Transform alvo;

    [Header("Configurações")]
    public float distancia = 5f;
    public float altura = 2.5f;
    public float suavidade = 10f;
    public float sensibilidadeMouse = 3f;

    private float anguloX = 20f;
    private float anguloY = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void LateUpdate()
    {
        if (alvo == null) return;

        // Rotação com o mouse
        anguloY += Input.GetAxis("Mouse X") * sensibilidadeMouse;
        anguloX -= Input.GetAxis("Mouse Y") * sensibilidadeMouse;
        anguloX = Mathf.Clamp(anguloX, -10f, 60f);

        // Posição da câmera atrás do personagem
        Quaternion rotacao = Quaternion.Euler(anguloX, anguloY, 0f);
        Vector3 direcao = rotacao * Vector3.back;
        Vector3 posicaoAlvo = alvo.position + Vector3.up * altura;
        Vector3 posicaoCamera = posicaoAlvo + direcao * distancia;

        // Evita câmera atravessar paredes
        if (Physics.Raycast(posicaoAlvo, direcao, out RaycastHit hit, distancia))
            posicaoCamera = hit.point + hit.normal * 0.2f;

        // Suaviza movimento
        transform.position = Vector3.Lerp(transform.position, posicaoCamera, suavidade * Time.deltaTime);
        transform.LookAt(posicaoAlvo);
    }
}