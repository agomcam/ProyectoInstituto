using UnityEngine;

public class AddPoints : MonoBehaviour
{
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameController.Instance.AddPoints(1);
        }
    }
}
