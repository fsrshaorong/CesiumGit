using UnityEngine;

public class ChangeMaterialOnCollision : MonoBehaviour
{
    public Material newMaterial; // �µĲ���

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Renderer wallRenderer = GetComponent<Renderer>();
            if (wallRenderer != null)
            {
                wallRenderer.material = newMaterial;
            }
        }
    }
}
