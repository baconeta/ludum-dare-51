namespace DefaultNamespace;
using UnityEngine;
public class SpriteSwapper :MonoBehaviour
{
    public delegate void Swap(bool swap);
    private Swap lightMode;
    private Swap darkMode;
    public void Start()
    {
        lightMode = true;
        darkMode = false;

    }

    void SwitchToFalse(bool boolean)
    {
        boolean = false;
    }

    void SwitchToTrue(bool boolean)
    {
        boolean = true;
    }

    public void Update()
    {
        if (darkMode)
        {
            // sprites get switched out
            // filter turns on
        }
    }
}