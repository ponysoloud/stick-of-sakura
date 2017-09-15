using UnityEngine;
using System.Collections;

public class AnimationEnabler : MonoBehaviour {

	public GameObject soundController;
	public Animator startAnim;
	public Animator helpAnim;
	public Animator dotsAnim;

	public void Start (){
		if (startAnim) {
			Invoke ("EnableAnim", 0f);
			//Invoke ("EnableAnim", 4.3f);
			if (!GuiController.tutorialEvent)
				Invoke ("EnableDots", 1f);
		}
	}

	void EnableAnim()
	{
		startAnim.enabled = true;
	}

	public void EnableDots()
	{
		dotsAnim.enabled = true;
	}


	void Update()
	{
		if (!StartButton.waspressed && Time.timeSinceLevelLoad > 5.5f && !DotsButtonAnim.wasShowing)
			helpAnim.enabled = true;

		if (helpAnim.enabled)
			helpAnim.SetBool ("pressed", StartButton.waspressed);
	}
}
