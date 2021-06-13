using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitButton : MonoBehaviour
{
    public void Quit()
    {
        if (!Application.isEditor) Application.Quit();
    }
}
