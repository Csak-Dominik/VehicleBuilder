using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Side
{
    Top,
    Bottom,
    Left,
    Right,
    Front,
    Back
}

public static class Helper
{
    public static float MapRange(float value, float sourceMin, float sourceMax, float destMin, float destMax)
    {
        return (value - sourceMin) / (sourceMax - sourceMin) * (destMax - destMin) + destMin;
    }

    public static Side GetSide(Vector3 pos, Vector3 point) => GetSideFromDir(point - pos);

    public static Side GetSideFromDir(Vector3 dir)
    {
        var up = Vector3.up;
        var right = Vector3.right;
        var forward = Vector3.forward;

        var upDot = Vector3.Dot(dir, up);
        var rightDot = Vector3.Dot(dir, right);
        var forwardDot = Vector3.Dot(dir, forward);

        var upAbs = Mathf.Abs(upDot);
        var rightAbs = Mathf.Abs(rightDot);
        var forwardAbs = Mathf.Abs(forwardDot);

        if (upAbs > rightAbs && upAbs > forwardAbs)
        {
            return upDot > 0 ? Side.Top : Side.Bottom;
        }
        else if (rightAbs > upAbs && rightAbs > forwardAbs)
        {
            return rightDot > 0 ? Side.Right : Side.Left;
        }
        else
        {
            return forwardDot > 0 ? Side.Front : Side.Back;
        }
    }

    public static Side GetSideMixed(Vector3 pos, Vector3 point, Vector3 normal)
    {
        var dir = point - pos;
        var avg = (dir + normal).normalized;
        return GetSideFromDir(avg);
    }

    public static Vector3 BlockPosOnSide(Vector3 pos, Side side)
    {
        switch (side)
        {
            case Side.Top:
                return new Vector3(pos.x, pos.y + 0.25f, pos.z);

            case Side.Bottom:
                return new Vector3(pos.x, pos.y - 0.25f, pos.z);

            case Side.Left:
                return new Vector3(pos.x - 0.25f, pos.y, pos.z);

            case Side.Right:
                return new Vector3(pos.x + 0.25f, pos.y, pos.z);

            case Side.Front:
                return new Vector3(pos.x, pos.y, pos.z + 0.25f);

            case Side.Back:
                return new Vector3(pos.x, pos.y, pos.z - 0.25f);

            default:
                return pos;
        }
    }
}