using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fader : MonoBehaviour {

    public Material faderMat;
    public float speed;

    public bool isFading;
    private float minFadeVal = 0f;
    private float maxFadeVal = 1f;

    private void Start() {
        faderMat.SetFloat("_Cutoff", minFadeVal);
    }

    private void Update() {
		//If its triggered to fade then set the alpha of the material multiplied to a speed multiplied to a direction
		// Positive Direction: fade in direction
		// Negative Direction: fade out direction
        if(isFading) {
            faderMat.SetFloat("_Cutoff", faderMat.GetFloat("_Cutoff") + speed);
            if (faderMat.GetFloat("_Cutoff") >= maxFadeVal || faderMat.GetFloat("_Cutoff") <= minFadeVal)
                isFading = false;
        }
    }

	/// <summary>
    /// Trigger a fade process and set up the material for it
    /// </summary>
    public void fadeIn() {
        isFading = true;
        faderMat.SetFloat("_Cutoff", minFadeVal + 0.001f);
        speed = Mathf.Abs(speed);
    }

	/// <summary>
    /// Trigger a fade process and set up the material for it
    /// </summary>
    public void fadeOut() {
        isFading = true;
        faderMat.SetFloat("_Cutoff", maxFadeVal - 0.001f);
        speed *= -1f;
    }
}
