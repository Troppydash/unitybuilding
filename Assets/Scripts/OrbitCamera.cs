using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitCamera : MonoBehaviour
{
    public bool enableRotation = true;

    [Header("Choose target")]
    public Transform target;

    //Camera fields
    private float _smoothness = 0.5f;
    private Vector3 _cameraOffset;

    //Mouse control fields
    [Space(2)]
    [Header("Mouse Controls")]
    public float rotationSpeedMouse = 5;
    public float zoomSpeedMouse = 10;

    private float _zoomAmountMouse = 0;
    private float _maxToClampMouse = 10;

    void Start()
    {
        _cameraOffset = transform.position - target.position;
        transform.LookAt(target);
    }

    void LateUpdate()
    {

        // Rotating camera with RMB dragging on PC.
        if (enableRotation && (Input.GetMouseButton(0)))
        {

            Quaternion camAngle = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * rotationSpeedMouse, Vector3.up);
            Quaternion vAngle = Quaternion.AngleAxis(Input.GetAxis("Mouse Y") * rotationSpeedMouse, -transform.right);

            Vector3 newPos = target.position + _cameraOffset;
            _cameraOffset = camAngle * _cameraOffset;
            _cameraOffset = vAngle * _cameraOffset;

            transform.position = Vector3.Slerp(transform.position, newPos, _smoothness);
            transform.LookAt(target);
        }
        else
        {
            // Translating camera on PC with mouse wheel.
            _zoomAmountMouse += Input.GetAxis("Mouse ScrollWheel");
            _zoomAmountMouse = Mathf.Clamp(_zoomAmountMouse, -_maxToClampMouse, _maxToClampMouse);

            var translate = Mathf.Min(Mathf.Abs(Input.GetAxis("Mouse ScrollWheel")), _maxToClampMouse - Mathf.Abs(_zoomAmountMouse));
            transform.Translate(0, 0, translate * zoomSpeedMouse * Mathf.Sign(Input.GetAxis("Mouse ScrollWheel")));

            _cameraOffset = transform.position - target.position;
        }

    }


}
