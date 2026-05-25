using UnityEngine;

public class EffectOnClick : MonoBehaviour
{
    // Arraste o prefab do efeito aqui no Inspector
    public GameObject effectPrefab;

    // Offset: ajusta a altura do efeito embaixo do personagem
    public Vector3 offset = new Vector3(0f, 0.1f, 0f);

    // Tempo até o efeito ser destruído
    public float destroyAfter = 2f;

    void Update()
    {
        // Detecta clique esquerdo do mouse
        if (Input.GetKeyDown(KeyCode.R))
        {
            SpawnEffect();
        }
    }

    void SpawnEffect()
    {
        // Pega a posição dos pés do personagem + offset
        Vector3 spawnPos = transform.position + offset;

        // Cria o efeito na posição
        GameObject fx = Instantiate(
            effectPrefab,
            spawnPos,
            Quaternion.identity
        );

        // Destrói o efeito após X segundos
        Destroy(fx, destroyAfter);
    }
}