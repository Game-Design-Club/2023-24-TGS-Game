using UnityEngine;

public class AutomaticPlay : MonoBehaviour
{
    private void Start() {
        GetComponent<ParticleSystem>().Play();
    }
}
