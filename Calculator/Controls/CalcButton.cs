using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.Controls;

public class CalcButton : Button
{
    /*
    This control defines a calculator button that maintains a square shape by setting its height equal to its width whenever its size is allocated. 
    This ensures that the button remains visually consistent and proportionate, regardless of the layout changes in the application. 
    */
    protected override void OnSizeAllocated(double width, double height)
    {
        base.OnSizeAllocated(width, height);

        if (width <= 0)
            return;

        HeightRequest = width;
    }
}

