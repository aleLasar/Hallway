using System.Collections;
using UnityEngine;

public class DestroyObject : MonoBehaviour
{
    void OnBecameInvisible()
    {
        Debug.Log(gameObject.name);
        Destroy(gameObject);
    }
}
