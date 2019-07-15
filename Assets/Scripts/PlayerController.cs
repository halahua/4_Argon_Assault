using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerController : MonoBehaviour {

    [Header("General")]
    [Tooltip("In ms^-1")] [SerializeField] float controlSpeed = 75f;
    [Tooltip("In m")] [SerializeField] float xRange = 32f;
    [Tooltip("In m")] [SerializeField] float yRange = 15f;
    [SerializeField] GameObject[] guns;

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
    bool isControlEnabled = true;



    void Update()
    {
        if (isControlEnabled)
        {
            ProcessTranslate();
            ProcessRotation();
            ProcessFiring();
        }
    }


    void OnPlayerDeath()
    {
        isControlEnabled = false;
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

        float xOffset = xThrow * controlSpeed * Time.deltaTime;
        float yOffset = yThrow * controlSpeed * Time.deltaTime;

        float rawXPos = transform.localPosition.x + xOffset;
        float clampXPos = Mathf.Clamp(rawXPos, -xRange, xRange);
        float rawYPos = transform.localPosition.y + yOffset;
        float clampYPos = Mathf.Clamp(rawYPos, -yRange, yRange);

        transform.localPosition = new Vector3(clampXPos, clampYPos, transform.localPosition.z);
    }

    private void ProcessFiring()
    {
        if (CrossPlatformInputManager.GetButton("Fire"))
        {
            SetGunsActive(true);
        }
        else
        {
            SetGunsActive(false);
        }
    }

    private void SetGunsActive(bool isActive)
    {
        foreach (GameObject gun in guns)
        {
            var EmissionModule = gun.GetComponent<ParticleSystem>().emission;
            EmissionModule.enabled = isActive;
        }
    }
}
