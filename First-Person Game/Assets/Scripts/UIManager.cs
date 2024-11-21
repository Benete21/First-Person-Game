using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public Camera mainCamera; // Reference to the camera (assign in the Inspector)
    //public float rotationSpeed = 45f; // Speed at which the camera rotates
    //private bool isRotating = false; // Track if the camera is currently rotating
    public GameObject[] UIElements; // any other UI elemets
    public GameObject initialButton; // begin button
    public Animator animatorbook;

    // Method to rotate the camera left by 90 degrees
   /* public void RotateCameraLeftBy90Degrees()
    {
        if (!isRotating) // Prevent triggering multiple rotations simultaneously
        {
            StartCoroutine(RotateCameraCoroutine(90f)); 
        }
    }*/
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void initiateGame()
    {
        initialButton.SetActive(false);
        animatorbook.SetBool("Begin", true);
        StartCoroutine(DisplayWait());

    }
    private IEnumerator DisplayWait()
    {
        yield return new WaitForSeconds(1.5f); // Wait for 3 seconds
        foreach (GameObject UIelement in UIElements)
        {
            UIelement.SetActive(true);
        }
    }

    // Coroutine to smoothly rotate the camera
    /*private IEnumerator RotateCameraCoroutine(float angle)
    {
        isRotating = true;

        Quaternion startRotation = mainCamera.transform.rotation; // Initial rotation
        Quaternion endRotation = startRotation * Quaternion.Euler(0, -angle, 0); // Target rotation

        float rotationProgress = 0f;
        while (rotationProgress < 1f)
        {
            rotationProgress += Time.deltaTime * (rotationSpeed / angle); // Normalize the rotation speed based on angle
            mainCamera.transform.rotation = Quaternion.Lerp(startRotation, endRotation, rotationProgress); // Smoothly interpolate rotation
            yield return null;
        }

        mainCamera.transform.rotation = endRotation; // Ensure exact final rotation
        isRotating = false;

        initialButton.SetActive(false);

        foreach (GameObject UIelement in UIElements)
        {
            UIelement.SetActive(true);
        }
    }*/
}

