using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movimento")]
    public float velocidade = 5f;
    public float velocidadeRotacao = 10f;

    [Header("Pulo e Gravidade")]
    public float alturaPulo = 2f;
    public float gravidade = -9.81f;

    [Header("Referências")]
    public Animator animator;
    public Transform cameraTransform;

    private CharacterController controller;
    private Vector3 velocidadeVertical;
    private bool pulando = false;
    private bool noChao = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Checa chão
        noChao = controller.isGrounded;

        if (noChao && velocidadeVertical.y < 0)
        {
            velocidadeVertical.y = -2f;
            pulando = false;
        }

        // Pulo
        if (Input.GetButtonDown("Jump") && noChao)
        {
            velocidadeVertical.y = Mathf.Sqrt(alturaPulo * -2f * gravidade);
            pulando = true;
        }

        // Aplica gravidade
        velocidadeVertical.y += gravidade * Time.deltaTime;
        controller.Move(velocidadeVertical * Time.deltaTime);

        // Movimento WASD
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        bool estaMovendo = (h != 0 || v != 0);

        if (estaMovendo && cameraTransform != null)
        {
            Vector3 direcao = cameraTransform.forward * v
                            + cameraTransform.right * h;
            direcao.y = 0f;
            direcao.Normalize();

            controller.Move(direcao * velocidade * Time.deltaTime);

            Quaternion rotAlvo = Quaternion.LookRotation(direcao);
            transform.rotation = Quaternion.Slerp(
                transform.rotation, rotAlvo,
                velocidadeRotacao * Time.deltaTime);
        }

        // Animações
        if (animator != null)
        {
            animator.SetFloat("Speed", estaMovendo ? 1f : 0f);
            animator.SetBool("Pulando", pulando);
            animator.SetBool("NoChao", noChao);
        }
    }
}