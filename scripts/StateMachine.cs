using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    static bool cutScene = false;

    public static bool GetCutsene()
    {
        return cutScene;
    }
    public static void SetCutscene(bool b)
    {
        cutScene = b;
    }
}
