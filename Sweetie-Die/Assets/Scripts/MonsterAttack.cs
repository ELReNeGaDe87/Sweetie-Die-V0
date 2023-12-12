using UnityEngine;
using UnityEngine.Video;

public class MonsterAttack : MonoBehaviour
{
    public Transform player;
    public float attackRange = 2f;
    public float attackCooldown = 2f;
    public PlayerController playerController; // Asegúrate de asignar esto en el Inspector
    public VideoPlayer videoPlayer; // Asigna el componente VideoPlayer en el Inspector
    private int vida = 5;
    private GameOverScript gameOver;

    private float nextAttackTime = 0f;

    void Start()
    {
        gameOver = GetComponent<GameOverScript>();
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        Debug.Log("Puntos restantes: " + vida);
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
        if (vida < 1)
        {
            gameOver.GameOver();
        }
        else
        {
            if (playerController != null)
            {
                vida--;
                playerController.ReceiveAttack();
                // Reproducir el video al recibir el ataque
                if (videoPlayer != null)
                {
                    if (!videoPlayer.isPlaying)
                    {
                        videoPlayer.Play();
                    }
                }
            }
        }
    }
}