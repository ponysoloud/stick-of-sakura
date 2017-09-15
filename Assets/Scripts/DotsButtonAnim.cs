using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class DotsButtonAnim : MonoBehaviour {

	public static bool creditsShowing, wasShowing;


	public GameObject inviseUI;
	public GameObject soundController;
	public GameObject panel;
	public GameObject parent;
	public Image credits;

	AnimType animMode;

	void SwapImage()
	{
		this.gameObject.transform.GetChild (0).gameObject.SetActive (!this.gameObject.transform.GetChild (0).gameObject.activeSelf);
		this.gameObject.transform.GetChild (1).gameObject.SetActive (!this.gameObject.transform.GetChild (1).gameObject.activeSelf);
	}

	public void DotsButtonMethod()
	{
		print ("1");
		if (!StartButton.IsAnimationInProcess) {
			print ("2");
			wasShowing = true;
			StartButton.IsAnimationInProcess = true;
			creditsShowing = true;
			SwapImage ();
			soundController.GetComponent<GameSoundController> ().ButtonClick ();
			panel.GetComponent<PauseMoving> ().Move ();
			if (!this.gameObject.transform.GetChild (0).gameObject.activeSelf) {
				//Открытие
				if (!StartButton.IsPressed)
					parent.GetComponent<StartButton> ().SimulateClick ();
				else {
					//Make level UIs invisible
					inviseUI.GetComponent<MakeObjInvisible>().MakeInvisible(0.5f);
					inviseUI.GetComponent<Image> ().raycastTarget = true;
				}
				credits.gameObject.SetActive (true);
				Invoke ("InvokeCredits", 0.8f);
			} else {
				//Закрытие
				InvokeCredits ();
				Invoke ("InvokeCreditsExit", 0.5f);
				Invoke ("InvokeCreditsInactive", 0.6f);
			}
			Invoke ("InvokeEndingAnim", 1.3f);

		}
	}

	void InvokeEndingAnim()
	{
		StartButton.IsAnimationInProcess = false;
	}
		
	void InvokeCreditsInactive()
	{
		credits.gameObject.SetActive (false);
	}

	void InvokeCredits()
	{
		if (this.gameObject.transform.GetChild (0).gameObject.activeSelf)
			EnableAnimation (AnimType.hide);
		else
			EnableAnimation (AnimType.show);
	}

	void InvokeCreditsExit()
	{
		creditsShowing = false;
		if (!StartButton.IsPressed)
			parent.GetComponent<StartButton> ().SimulateStart ();
		else {
			//Make level UIs visible
			inviseUI.GetComponent<MakeObjInvisible>().MakeVisible(0.5f);
			inviseUI.GetComponent<Image> ().raycastTarget = false;
		}
	}
		
	public float timeLength;
	Color colorVector;
	int a;
	bool isMove;
	float time;

	void Start(){
		colorVector = credits.GetComponent<Image> ().color;
		colorVector.a = 0.0f;
		credits.GetComponent<Image> ().color = colorVector;
	}

	// Update is called once per frame
	void Update () {
		if (isMove) {
			if (time > timeLength || time < 0) {
				isMove = false;
				time = Mathf.Clamp (time, 0, timeLength);
			} else {
				time += Time.deltaTime;
				var tTime = time;
				time = Mathf.Clamp (time, 0, timeLength);
				colorVector.a = this.TrigonometricFunc (time);
				credits.GetComponent<Image> ().color = colorVector;
				time = tTime;
			}
		}

		/*
		if (StartButton.pressed)
			this.gameObject.SetActive (false);
			*/
		
	}
		
	public float TrigonometricFunc(float t)
	{
		if (a==1) {
			return 1f / timeLength * t;
		} else {
			return 1f - 1f / timeLength * t;
		}
	}

	/// <summary>
	/// Запуск анимации, где задается один из параметров функции
	/// </summary>
	public void EnableAnimation(AnimType mode)
	{
		if (mode == AnimType.show)
			a = 1;
		else
			a = -1;
		time = 0f;
		isMove = true;
	}

}
