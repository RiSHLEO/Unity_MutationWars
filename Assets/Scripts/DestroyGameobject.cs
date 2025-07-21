using UnityEngine;

public class DestroyGameobject : MonoBehaviour
{
    private void Start()
    {
        Destroy(gameObject, 1f);
    }
}
