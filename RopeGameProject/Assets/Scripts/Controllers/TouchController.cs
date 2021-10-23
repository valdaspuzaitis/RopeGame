using UnityEngine;

public class TouchController : MonoBehaviour
{
    void Update()
    {
        if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.touches[0].position), Vector2.zero);

            if (hit && hit.collider.CompareTag("Gem"))
            {
                hit.collider.GetComponent<GemController>().OnTouch();
            }
        }
    }
}
