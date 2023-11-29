using UnityEngine;

public class MonsterAttack : MonoBehaviour
{
    public Transform player;
    public float attackRange = 2f;
    public float attackCooldown = 2f;
    public PlayerController playerController; // Asegúrate de asignar esto en el Inspector

    private float nextAttackTime = 0f;

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange && Time.time >= nextAttackTime)
        {
            Attack();
            nextAttackTime = Time.time + 1f / attackCooldown;
        }
    }

    void Attack()
    {
        // Lógica de ataque del NPC
        Debug.Log("NPC atacando al jugador");

        // Llama al método ReceiveAttack del PlayerController
        if (playerController != null)
        {
            playerController.ReceiveAttack();
        }
    }
}
