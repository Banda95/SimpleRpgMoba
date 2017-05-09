using UnityEngine;
using System.Collections;

public class CameraUtils
{
    public static Vector3? MouseToWorld(string layer, float y = 0)
    {
        return MouseToWorld(new string[] { layer }, y);
    }

    public static Vector3? MouseToWorld(string[] layers, float y = 0)
    {
        Vector3? destination = null;
        Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);

        //Move to terrain point
        RaycastHit info;
        Physics.Raycast(r, out info, 1000, LayerMask.GetMask(layers));
        if (info.collider != null)
            destination = new Vector3(info.point.x, info.point.y, info.point.z);

        return destination;
    }
}
