using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShooterRotator : MonoBehaviour {
    private enum RotateState {
        Idle, Vertical, Horizontal, Ready
    }
    private RotateState state = RotateState.Idle;
    public float verticalRatateSpeed = 360f;
    public float horizontalRatateSpeed = 360f;
    public BallShooter ballShooter;
    void Start() {

    }

    void Update() {
        switch(state) {
            case RotateState.Idle:
                if (Input.GetButtonDown("Fire1")) {
                    state = RotateState.Horizontal;
                }
                break;
            case RotateState.Horizontal:
                if (Input.GetButton("Fire1")) {
                    transform.Rotate(new Vector3(0, horizontalRatateSpeed * Time.deltaTime, 0));
                } else if (Input.GetButtonUp("Fire1")) {
                    state = RotateState.Vertical;
                }
                break;
            case RotateState.Vertical:
                if (Input.GetButton("Fire1")) {
                    transform.Rotate(new Vector3(-verticalRatateSpeed * Time.deltaTime, 0, 0));
                } else if (Input.GetButtonUp("Fire1")) {
                    state = RotateState.Ready;
                    ballShooter.enabled = true;
                }
                break;
            case RotateState.Ready:
                break; 
        }
    }
    private void OnEnable() {
        transform.rotation=Quaternion.identity;
        state=RotateState.Idle;
        ballShooter.enabled = false;
    }
}
