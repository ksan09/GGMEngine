using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(FieldOfView))]
public class FieldOfViewEditor : Editor
{
    private void OnSceneGUI()
    {
        FieldOfView fow = (FieldOfView)target;
        Handles.color = Color.red;
        Handles.DrawWireArc(fow.transform.position,
            Vector3.up, Vector3.forward, 360, fow.viewRadius);

        Vector3 viewAngleA = fow.DirFromAngle(-fow.viewAngle / 2, false);
        Vector3 viewAngleB = fow.DirFromAngle(fow.viewAngle / 2, false);

        Handles.DrawLine(fow.transform.position, 
            fow.transform.position + viewAngleA * fow.viewRadius);
        Handles.DrawLine(fow.transform.position,
            fow.transform.position + viewAngleB * fow.viewRadius);

        Handles.color = Color.white;
        for(int i = 0; i < fow.visibleTargets.Count; ++i)
        {
            Handles.DrawLine(fow.transform.position,
                fow.visibleTargets[i].position);
        }
    }
}
