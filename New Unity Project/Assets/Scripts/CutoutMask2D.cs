using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class CutoutMask2D : Image
{
    private static readonly int StencilComp = Shader.PropertyToID("_StencilComp");

    public override Material material
    {
        get
        {
            Material materialLocal = new Material(base.material);
            materialLocal.SetInt(StencilComp,(int)CompareFunction.Equal);
            return materialLocal;
        }
    }
}
