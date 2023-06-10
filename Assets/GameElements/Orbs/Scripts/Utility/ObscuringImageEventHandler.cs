using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class ObscuringImageHandler
{
    internal static event Action OnObscuringImageFade;

    internal static void RaiseObscuringImageFade()
    {
        OnObscuringImageFade?.Invoke();
    }
}
