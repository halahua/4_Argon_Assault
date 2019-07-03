using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour {

    [Tooltip("In ms^-1")] [SerializeField] float Speed = 75f;
    [Tooltip("In m")] [SerializeField] float xRange = 32f;
    [Tooltip("In m")] [SerializeField] float yRange = 15f;

    [Header("Y axis")]
    [Space(5)]
    [SerializeField] float positionPitchFactor = -1.3f;
    [SerializeField] float controlPitchFactor = 15f;

    [Header("X axis")]
    [Space(5)]
    [SerializeField] float positionYawFactor = 1f;

    [Header("Z axis")]
    [Space(5)]
    [SerializeField] float controlRollFactor = -35f;

    float xThrow, yThrow;




    void OnCollisionEnter (Collision collision)
    {
        print("Player collided with something");
    }

    void OnTriggerEnter (Collider collider)
    {
        print("Player triggered something");
    }

    void Update()
    {
        ProcessTranslate();
        ProcessRotation();
    }

    private void ProcessRotation()
    {
        float pitchDueToPosition = transform.localPosition.y * positionPitchFactor;
        float pitchDueToControlThrow = yThrow * controlPitchFactor;
        float pitch = pitchDueToPosition + pitchDueToControlThrow;

        float yaw = transform.localPosition.x * positionYawFactor;

        float roll = xThrow * controlRollFactor;

        transform.localRotation = Quaternion.Euler(pitch, yaw, roll); //x, y, z, respectivelly
    }

    private void ProcessTranslate()
    {
        xThrow = CrossPlatformInputManager.GetAxis("Horizontal");
        yThrow = CrossPlatformInputManager.GetAxis("Vertical");

        float xOffset = xThrow * Speed * Time.deltaTime;
        float yOffset = yThrow * Speed * Time.deltaTime;

        float rawXPos = transform.localPosition.x + xOffset;
        float clampXPos = Mathf.Clamp(rawXPos, -xRange, xRange);
        float rawYPos = transform.localPosition.y + yOffset;
        float clampYPos = Mathf.Clamp(rawYPos, -yRange, yRange);

        transform.localPosition = new Vector3(clampXPos, clampYPos, transform.localPosition.z);
    }
}
