using UnityEngine;

public class SimpleTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Trigger Entered: " + other.name);


    }
}
