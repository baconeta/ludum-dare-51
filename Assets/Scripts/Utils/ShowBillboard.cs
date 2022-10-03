using UnityEngine;
using UnityEngine.UI;

namespace Utils
{
    public class ShowBillboard : MonoBehaviour
    {
        public float alphaValue;

        // Start is called before the first frame update
        public void SetAlpha()
        {
            Image i = GetComponent<Image>();
            i.color = new Color(255, 255, 255, alphaValue);
        }
    }
}