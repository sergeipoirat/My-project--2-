using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class DestroyableObjects : MonoBehaviour
{
    [SerializeField] private GameObject _destroyVFX;

    private void OnTriggerEnter2D (Collider2D other)
    {
        if (other.gameObject.GetComponent<Sword>())
        {
            Instantiate(_destroyVFX, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
