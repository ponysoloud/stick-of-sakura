using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameGuiController : MonoBehaviour {

	int stars;
	public static bool isExit;
	public GameObject pauseButton;
	public GameObject movesPanel;
	public GameObject winpanel, losepanel;
	public GameObject Text;
	//public Animator starsAnimator;
	public Animator firstStarAnim, secondStarAnim, thirdStarAnim;

	void Start()
	{
		if (!GuiController.tutorialEvent)
			Invoke ("StartUIAnimation", 1.3f);
	}

	void StartUIAnimation()
	{
		pauseButton.GetComponent<Movement> ().isMove = true;
		movesPanel.GetComponent<Movement> ().isMove = true;
	}

	void EndUIAnimation()
	{
		pauseButton.GetComponent<Movement> ().reverseMove = true;
		movesPanel.GetComponent<Movement> ().reverseMove = true;
		pauseButton.GetComponent<Movement> ().isMove = true;
		movesPanel.GetComponent<Movement> ().isMove = true;
	}

	/// <summary>
	/// Анимация кнопки Pause
	/// </summary>
	public void SwapImage()
	{
		pauseButton.transform.GetChild (0).gameObject.SetActive (!pauseButton.transform.GetChild (0).gameObject.activeSelf);
		pauseButton.transform.GetChild (1).gameObject.SetActive (!pauseButton.transform.GetChild (1).gameObject.activeSelf);
	}

	public void EndAnimation(int starsCount)
	{
		EndUIAnimation ();
		stars = starsCount;
		int i;
		winpanel.SetActive (true);
		//Показ звезд
		for (i = 0; i < 3; i++) {
			winpanel.transform.GetChild (0).GetChild (i).GetComponent<EndPanelAnimation> ().Animation (1f, AnimType.show);
		}
		Invoke("InvokeAnimatingStars", 0.2f);
		//Показ кнопок
		var controlsDelay = (stars - 1) * starsInterval * 0.5f;
		if (stars != 0) {
			Invoke ("InvokeShowingButtons", controlsDelay);
		} else
			Invoke ("InvokeShowingButton", controlsDelay);
	}

	float starsInterval = 0.4f;

	/// <summary>
	/// Анимация звезд с задрежкой
	/// </summary>
	void InvokeAnimatingStars()
	{
		//starsAnimator.SetInteger ("StarsCount", stars);

		if (stars > 0)
			firstStarAnim.SetBool ("starEntry", true);
		if (stars > 1)
			Invoke ("InvokeSecondStar", starsInterval);
		if (stars > 2)
			Invoke ("InvokeThirdStar", starsInterval*2); 
	}

	void InvokeSecondStar()
	{
		secondStarAnim.SetBool ("starEntry", true);
	}

	void InvokeThirdStar()
	{
		thirdStarAnim.SetBool ("starEntry", true);
	}

	/// <summary>
	/// Анимация кнопок с задержкой
	/// </summary>
	void InvokeShowingButtons()
	{
		winpanel.transform.GetChild (1).gameObject.SetActive (true);
		winpanel.transform.GetChild (2).gameObject.SetActive (true);
		winpanel.transform.GetChild (1).GetComponent<EndPanelAnimation> ().Animation (0.5f, AnimType.show);
		winpanel.transform.GetChild (2).GetComponent<EndPanelAnimation> ().Animation (0.5f, AnimType.show);
	}

	/// <summary>
	/// АНимация кнопки с задержкой
	/// </summary>
	void InvokeShowingButton()
	{
		winpanel.transform.GetChild (3).gameObject.SetActive (true);
		winpanel.transform.GetChild (3).GetComponent<EndPanelAnimation> ().Animation (0.4f, AnimType.show);
	}

	/// <summary>
	/// Нажатие по кнопке выход
	/// </summary>
	public void ExitClickEvent()
	{
		isExit = true;
		SceneManager.LoadScene (Controller.MENU_SCENE);
		StartButton.IsAnimationInProcess = false;
		/*
		startButton.SetActive (false);
		startButton.GetComponent<StartButton> ().ShowLevels (); */
		//StartButton.pressed = true;
	}


}
