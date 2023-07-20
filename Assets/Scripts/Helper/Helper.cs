using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Helper
{
    public static float MapRange(float value, float sourceMin, float sourceMax, float destMin, float destMax)
    {
        return (value - sourceMin) / (sourceMax - sourceMin) * (destMax - destMin) + destMin;
    }
}