using UnityEngine;

public class DeleteBoss : MonoBehaviour
{
        [SerializeField] private float limitPosition;
        void Update()
        {
                if (transform.position.x <= limitPosition)
                {
                        GameController.Instance.BossDefeated();
                        Destroy(gameObject);
                        
                }
        }

}
