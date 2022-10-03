using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHotwire : MonoBehaviour
{
    private bool _hotwireEnabled = false;
    
    public void EnableHotwire()
    {
        _hotwireEnabled = true;
    }

    public bool DoJumpToBoss()
    {
        return _hotwireEnabled;
    }
}
