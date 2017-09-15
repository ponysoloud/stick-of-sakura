using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelButton : MonoBehaviour {

	public GameObject soundController;

	/// <summary>
	/// Ссылка на объект типа Level "привязанный" к этой кнопке
	/// </summary>
	public Level Level {
		get;
		set;
	}

	/// <summary>
	/// Обработка нажатия по кнопке уровня
	/// </summary>
	public void ClickEvent()
	{
		//StopMusic
		soundController.GetComponent<GameSoundController> ().StopMusic ();

		Controller.levelInfo = Level;
		if (Level.ButtonType==TypeOfButton.playable)
		Invoke ("LoadLevel", 1f);
	}

	/// <summary>
	/// Загрузка сцены с уровнем
	/// </summary>
	void LoadLevel()
	{
		SceneManager.LoadScene (Controller.GAME_SCENE);
	}
}
