using UnityEngine;
using System.Collections;

public class PauseMoving : MonoBehaviour {

	GameObject pausePanel;

	void Awake()
	{
		pausePanel = this.gameObject.transform.GetChild (0).gameObject;
		pausePanel.GetComponent<Movement> ().reverseMove = true;
	}

	public void Move()
	{
		pausePanel.GetComponent<Movement> ().reverseMove = !pausePanel.GetComponent<Movement> ().reverseMove;
		MoveGuiPlace ();
	}

	/// <summary>
	/// Объявление анимации объекта moveObject
	/// </summary>
	public void MoveGuiPlace()
	{
		pausePanel.GetComponent<Movement>().isMove = true;
	}
}
