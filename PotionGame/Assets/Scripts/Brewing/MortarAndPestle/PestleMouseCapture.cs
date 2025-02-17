using UnityEngine;
using UnityEngine.UIElements;
using System;
public class PestleMouseCapture : VisualElement
{

    MortarAndPestleView parentPestle;

    public PestleMouseCapture(MortarAndPestleView parentPestle)
    {
        this.parentPestle = parentPestle;

        style.height = new StyleLength(new Length(100f, LengthUnit.Percent));
        style.width = new StyleLength(new Length(100f, LengthUnit.Percent));

        RegisterCallback<MouseDownEvent>(OnMouseDownEvent);
    }

    ~PestleMouseCapture()
    {
        UnregisterCallback<MouseDownEvent>(OnMouseDownEvent);
    }

    private void OnMouseDownEvent(MouseDownEvent mouseEvent)
    {
        parentPestle.OnClick();
    }

}
