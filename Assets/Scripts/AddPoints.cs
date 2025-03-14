using UnityEngine;

public class AddPoints : MonoBehaviour
{
    [SerializeField] private int points;
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameController.Instance.AddPoints(points);
        }
    }
}
