using UnityEngine;
[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movimento")]
    public float velocidade = 5f;
    public float velocidadeCorrida = 10f;
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
    private bool correndo = false;
    private bool fazendoTaunt = false;
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }
    void Update()
    {
        noChao = controller.isGrounded;
        if (noChao && velocidadeVertical.y < 0)
        {
            velocidadeVertical.y = -2f;
            pulando = false;
        }
        if (Input.GetButtonDown("Jump") && noChao && !fazendoTaunt)
        {
            velocidadeVertical.y = Mathf.Sqrt(alturaPulo * -2f * gravidade);
            pulando = true;
        }
        velocidadeVertical.y += gravidade * Time.deltaTime;
        controller.Move(velocidadeVertical * Time.deltaTime);
        if (fazendoTaunt)
        {
            AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
            if (!state.IsName("Taunt"))
                fazendoTaunt = false;
        }
        if (!fazendoTaunt)
        {
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");
            bool estaMovendo = (h != 0 || v != 0);
            correndo = Input.GetKey(KeyCode.LeftShift) && estaMovendo;
            float velocidadeAtual = correndo ? velocidadeCorrida : velocidade;
            if (estaMovendo && cameraTransform != null)
            {
                Vector3 direcao = cameraTransform.forward * v
                                + cameraTransform.right * h;
                direcao.y = 0f;
                direcao.Normalize();
                controller.Move(direcao * velocidadeAtual * Time.deltaTime);
                Quaternion rotAlvo = Quaternion.LookRotation(direcao);
                transform.rotation = Quaternion.Slerp(
                    transform.rotation, rotAlvo,
                    velocidadeRotacao * Time.deltaTime);
            }
            if (animator != null)
            {
                animator.SetFloat("Speed", estaMovendo ? 1f : 0f);
                animator.SetBool("Pulando", pulando);
                animator.SetBool("NoChao", noChao);
                animator.SetBool("Correndo", correndo);
            }
        }
        if (Input.GetMouseButtonDown(0) && noChao && !fazendoTaunt)
            animator.SetTrigger("Atacando");
        if (Input.GetMouseButtonDown(1) && noChao && !fazendoTaunt)
            animator.SetTrigger("Atacando2");
        if (Input.GetKeyDown(KeyCode.R) && noChao && !fazendoTaunt)
        {
            animator.SetTrigger("Taunt");
            fazendoTaunt = true;
        }
    }
}