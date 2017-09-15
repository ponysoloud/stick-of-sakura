using UnityEngine;
using System.Collections;

public enum MovingSide {left, right};

/// <summary>
/// Класс привязанный к объекту - отцу всех grid
/// </summary>
public class LevelGridMoving : MonoBehaviour {

	float xStart = 0.0f;
	float xEnd = 0.0f;
	bool sendCall = true;

	public static int shownId;
	public static bool firstMoveIdentifier;
	public static bool moveIdentifier;
	public GameObject rightarrow, leftarrow;
	public GameObject guiControllerParentObject;
	GameObject[] levelGridArray;
	GameObject moveObject;
	int pubmode;
	bool rightArrowActive, leftArrowActive;
	int shownElementId = 0;

	void Awake()
	{
		firstMoveIdentifier = false;
		moveIdentifier = false;
		shownId = 0;
		levelGridArray = guiControllerParentObject.GetComponent<GuiController> ().levelGridArray;
	}

	//mode=1 - left //mode=-1 - right
	void MoveIn(int mode)
	{
		print (shownElementId);
		moveObject = this.gameObject.transform.GetChild (shownElementId).gameObject;
		moveObject.GetComponent<Movement> ().ContinueMod (false, mode); //reverse mode = false
		MoveGuiPlace ();
	}

	void MoveOut(int mode)
	{
		moveObject = this.gameObject.transform.GetChild (shownElementId).gameObject;
		moveObject.GetComponent<Movement> ().ContinueMod(true, mode); //x0= - x0; reverse mode = true
		MoveGuiPlace ();
	}

	public void Move(int mode)
	{
		pubmode = mode;
		//Скрытие текущей страницы уровней если она есть
		if (StartButton.pressed) {
			MoveIn (mode);
			firstMoveIdentifier = true;
			shownId = shownElementId;
			moveIdentifier = true;
			StartButton.pressed = false;
			leftArrowActive = true;
			rightArrowActive = false;
		} else {
			//Ограничение на конец сеток уровней
			if ((mode == 1 && shownElementId < levelGridArray.Length - 1) || (mode == -1 && shownElementId > 0)) {
				MoveOut (mode);
				shownElementId += mode;
				shownId = shownElementId;
				moveIdentifier = true;
				Invoke ("InvokeMoveIn", 0.5f);
				if (shownElementId == levelGridArray.Length - 1)
					leftArrowActive = false;
				else
					leftArrowActive = true;
				if (shownElementId == 0)
					rightArrowActive = false;
				else
					rightArrowActive = true;
			}
		}
		/*
		if (shownElementId > 0 && shownElementId < levelGridArray.Length - 1)
		if (mode == 1)
			for (int i = levelGridArray.Length - 1; i > shownElementId; i--) {
				levelGridArray [i].transform.localPosition = levelGridArray [i - 1].transform.localPosition;
				levelGridArray [i].GetComponent<Movement> ().ResetX0 ();
			}
		else
			for (int i = 0; i < shownElementId; i++) {
				levelGridArray [i].transform.localPosition = levelGridArray [i + 1].transform.localPosition;
				levelGridArray [i].GetComponent<Movement> ().ResetX0 ();
			}
		*/
	}

	void InvokeMoveIn()
	{
		MoveIn (pubmode);
	}

	/// <summary>
	/// Объявление анимации объекта moveObject
	/// </summary>
	public void MoveGuiPlace()
	{
		moveObject.GetComponent<Movement>().isMove = true;
	}

	void Update()
	{
		rightarrow.SetActive (rightArrowActive);
		leftarrow.SetActive (leftArrowActive);

		foreach (Touch touch in Input.touches) {

			if (touch.phase == TouchPhase.Began) {
				xStart = touch.position.x;
			}

			if (touch.phase == TouchPhase.Moved) {
				xEnd = touch.position.x;

				if ((100f < xEnd-xStart) && sendCall) {
					Move (-1);
					//Скрываю надписи обучения
					guiControllerParentObject.GetComponent<GuiController> ().HideTutorialText ();
					sendCall = false;
				}
				if ((-100f > xEnd-xStart) && sendCall) {
					Move (1);
					//Скрываю надписи обучения
					guiControllerParentObject.GetComponent<GuiController> ().HideTutorialText ();
					sendCall = false;
				}
			}

			if (touch.phase == TouchPhase.Ended) {
				xStart = 0.0f;    // resetting start and end x position.
				xEnd = 0.0f;
				sendCall = true;      //Reset to send call again after touch has been completed.
			}

		}
	}
}
