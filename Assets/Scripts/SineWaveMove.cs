using UnityEngine;

/*
    Script: SineWaveMove
    Author: Gareth Lockett
    Version: 1.0
    Description:    Simple script for moving the object along an axis using a sine wave.
*/

public class SineWaveMove : MonoBehaviour
{
    public enum Axis { X_AXIS, Y_AXIS, Z_AXIS }

    public Axis axis;
    public float moveDistance;
    public float moveSpeed;

    void Update()
    {
        // Set the move direction based on the selected axis.
        Vector3 moveDirection = Vector3.zero;
        switch( this.axis )
        {
            case Axis.X_AXIS:
                moveDirection = this.transform.right;
                break;

            case Axis.Y_AXIS:
                moveDirection = this.transform.up;
                break;

            case Axis.Z_AXIS:
                moveDirection = this.transform.forward;
                break;
        }

        // Do the actual move.
        this.transform.position += moveDirection * Time.deltaTime * this.moveDistance * Mathf.Sin( Time.time *this.moveSpeed );
    }
}
