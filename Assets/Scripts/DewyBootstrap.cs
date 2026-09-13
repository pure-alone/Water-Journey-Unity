using UnityEngine;

public sealed class DewyBootstrap : MonoBehaviour
{
    private void Awake()
    {
        if (FindFirstObjectByType<DewyApp>() == null)
            gameObject.AddComponent<DewyApp>();
    }
}
