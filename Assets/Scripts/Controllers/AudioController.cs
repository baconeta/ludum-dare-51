using UnityEngine;

namespace Controllers
{
    public class AudioController : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}