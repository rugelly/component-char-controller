using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TEMPPLAYERVISUALIZER : MonoBehaviour
{
    private CapsuleCollider _collider;

    private void OnDrawGizmos()
    {
        _collider = GetComponentInParent<CapsuleCollider>();
        DrawWireCapsule(transform.position + Vector3.up * (_collider.height / 2), transform.rotation, 0.5f, _collider.height, Color.white);
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(transform.position + transform.TransformDirection(new Vector3(0, _collider.height * 0.75f, 0.5f)), new Vector3(0.2f, 0.2f, 0.2f));
    }

    // (c)toomasio @ unity answers
    // thanks a bunch
    public void DrawWireCapsule(Vector3 _pos, Quaternion _rot, float _radius, float _height, Color _color = default(Color))
    {
        if (_color != default(Color))
            Handles.color = _color;
        Matrix4x4 angleMatrix = Matrix4x4.TRS(_pos, _rot, Handles.matrix.lossyScale);
        using (new Handles.DrawingScope(angleMatrix))
        {
            var pointOffset = (_height - (_radius * 2)) / 2;

            //draw sideways
            Handles.DrawWireArc(Vector3.up * pointOffset, Vector3.left, Vector3.back, -180, _radius);
            Handles.DrawLine(new Vector3(0, pointOffset, -_radius), new Vector3(0, -pointOffset, -_radius));
            Handles.DrawLine(new Vector3(0, pointOffset, _radius), new Vector3(0, -pointOffset, _radius));
            Handles.DrawWireArc(Vector3.down * pointOffset, Vector3.left, Vector3.back, 180, _radius);
            //draw frontways
            Handles.DrawWireArc(Vector3.up * pointOffset, Vector3.back, Vector3.left, 180, _radius);
            Handles.DrawLine(new Vector3(-_radius, pointOffset, 0), new Vector3(-_radius, -pointOffset, 0));
            Handles.DrawLine(new Vector3(_radius, pointOffset, 0), new Vector3(_radius, -pointOffset, 0));
            Handles.DrawWireArc(Vector3.down * pointOffset, Vector3.back, Vector3.left, -180, _radius);
            //draw center
            Handles.DrawWireDisc(Vector3.up * pointOffset, Vector3.up, _radius);
            Handles.DrawWireDisc(Vector3.down * pointOffset, Vector3.up, _radius);

        }
    }
}
