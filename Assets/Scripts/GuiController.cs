using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

/// <summary>
/// Тип кнопки (доступен уровень для игры или нет)
/// </summary>
public enum TypeOfButton {playable, unplayable}

public class GuiController : MonoBehaviour {

	//Глобальная переменная отвечающая за включение/отключение обучающего режима
	public static bool tutorialEvent;

	const int LEVEL_COUNT = 21;

	/// <summary>
	/// Кнопки уровней
	/// </summary>
	GameObject[] levelsUIArray;

	/// <summary>
	/// Массив сеток уровней
	/// </summary>
	public GameObject[] levelGridArray;

	public GameObject topTutor, bottomTutor;
	public GameObject levelButtonPrefab;
	public Sprite unplayed, played, playable;
	public GameObject star;
	public GameObject levelGrid;
	public GameObject startButton;
	public GameObject pagesGrid;
	public GameObject pageImage;
	public GameObject helpButton;
	public Sprite active, inactive;
	GameObject[] pagesStroke;

	public GameObject soundController;

	int levelId;

	void Awake()
	{
		if (GameGuiController.isExit) {
			StartButton.pressed = true;
			startButton.SetActive (false);

			startButton.GetComponent<StartButton> ().ShowLevels ();
			if (tutorialEvent) {
				Invoke ("MenuTutorialAnimation", 0.3f);
				tutorialEvent = false;
			}
			startButton.GetComponent<AnimationEnabler> ().EnableDots ();
			GameGuiController.isExit = false;
		}

		//PlayMusic
		soundController.GetComponent<GameSoundController> ().PlayMusic ();
	}
		
	void MenuTutorialAnimation()
	{
		topTutor.SetActive (true);
		topTutor.GetComponent<MenuTutorialTextAppearance> ().TextAnimation (0.7f, AnimType.show);
		bottomTutor.SetActive (true);
		bottomTutor.GetComponent<MenuTutorialTextAppearance> ().TextAnimation (0.7f, AnimType.show);
	}

	public void HideTutorialText()
	{
		if (topTutor.activeSelf) {
			topTutor.GetComponent<MenuTutorialTextAppearance> ().TextAnimation (0.7f, AnimType.hide);
			bottomTutor.GetComponent<MenuTutorialTextAppearance> ().TextAnimation (0.7f, AnimType.hide);
		}
	}

	/// <summary>
	/// Генерация страницы уровней
	/// </summary>
	public void GenerateLevelButtons(){
		pagesStroke = new GameObject[LevelsHandler.levelArray.Length / 15 + 1];
		for (int j = 0; j <= LevelsHandler.levelArray.Length / 15; j++) {
			pagesStroke [j] = GameObject.Instantiate (pageImage, new Vector2 (0, 0), Quaternion.identity) as GameObject;
			pagesStroke [j].transform.SetParent (pagesGrid.transform);
			pagesStroke [j].transform.localScale = Vector3.one;
			pagesStroke [j].SetActive (true);
		}

		levelsUIArray = new GameObject[LevelsHandler.levelArray.Length];
		for (int j = 0; j <= LevelsHandler.levelArray.Length / 15; j++) {
			if (LevelsHandler.levelArray.Length % 15 != 0)
				for (int k = 1; k < 16; k++) {
					int i = j * 15 + k;
					if (i > LEVEL_COUNT)
						break;
					levelsUIArray [i] = GameObject.Instantiate (levelButtonPrefab, new Vector2 (0, 0), Quaternion.identity) as GameObject;
					//int j = i / 15 + 1;
					levelsUIArray [i].transform.SetParent (levelGridArray [j].transform);
					levelsUIArray [i].transform.localScale = Vector3.one;
					levelsUIArray [i].SetActive (true);
					levelsUIArray [i].GetComponent<LevelButton> ().Level = LevelsHandler.levelArray [i];
					//Установка спрайта кнопки, цвета номера уровня и типа кнопки в зависимости от типа уровня
					if (levelsUIArray [i].GetComponent<LevelButton> ().Level.LevelType == TypeOfLevel.played) {
						levelsUIArray [i].transform.GetChild (0).GetComponent<Image> ().sprite = played;
						levelsUIArray [i].GetComponent<LevelButton> ().Level.ButtonType = TypeOfButton.playable;
						levelsUIArray [i].transform.GetChild (0).GetChild (0).GetComponent<Text> ().color = new Color (44f / 255f, 62f / 255f, 80f / 255f);
						GenerateStars (levelsUIArray [i].GetComponent<LevelButton> ().Level.StarsCount, i);
					} else if ((i != 1 && levelsUIArray [i - 1].GetComponent<LevelButton> ().Level.LevelType == TypeOfLevel.played) || (i == 1 && levelsUIArray [i].GetComponent<LevelButton> ().Level.LevelType == TypeOfLevel.unplayed)) {
						levelsUIArray [i].transform.GetChild (0).GetComponent<Image> ().sprite = playable;
						levelsUIArray [i].GetComponent<LevelButton> ().Level.ButtonType = TypeOfButton.playable;
						levelsUIArray [i].transform.GetChild (0).GetChild (0).GetComponent<Text> ().color = new Color (44f / 255f, 62f / 255f, 80f / 255f);
					} else {
						levelsUIArray [i].transform.GetChild (0).GetComponent<Image> ().sprite = unplayed;
						levelsUIArray [i].GetComponent<LevelButton> ().Level.ButtonType = TypeOfButton.unplayable;
						levelsUIArray [i].transform.GetChild (0).GetChild (0).GetComponent<Text> ().enabled = false;
					}
					levelsUIArray [i].transform.GetChild (0).GetChild (0).GetComponent<Text> ().text = levelsUIArray [i].GetComponent<LevelButton> ().Level.LevelId.ToString ();
				}
		}
    }

	/// <summary>
	/// Генерация звезд под кнопкой уровня
	/// </summary>
	/// <param name="count">Количество звезд для генерации</param>
	/// <param name="i">Индекс уровня для которого герерируем звезды</param>
	void GenerateStars(int count, int i)
	{
		levelId = i;
		switch (count) {
		case 1:
			{
				CreateStar (new Vector3 (0, -120f, 0), 0);
				break;
			}
		case 2:
			{
				CreateStar (new Vector3 (-35, -120f, 0), 0);
				CreateStar (new Vector3 (35, -120f, 0), 0);
			}
			break;
		case 3:
			{
				CreateStar (new Vector3 (0, -120f, 0), 0);
				CreateStar (new Vector3 (65, -100f, 0), 21f);
				CreateStar (new Vector3 (-65, -100f, 0), -21f);
			}
			break;
		}

	}

	/// <summary>
	/// Создание звезды
	/// </summary>
	/// <param name="position">Координаты звезды</param>
	/// <param name="angle">Угол наклона</param>
	void CreateStar(Vector3 position, float angle)
	{
		GameObject starObject = GameObject.Instantiate (star, new Vector2 (0, 0), Quaternion.identity) as GameObject;
		starObject.transform.SetParent (levelsUIArray [levelId].transform);
		starObject.transform.localScale = Vector3.one;
		starObject.GetComponent<RectTransform> ().transform.localPosition = position;
		starObject.transform.Rotate (new Vector3 (0, 0, 1), angle);
		starObject.SetActive (true);
	}
		
	void Update()
	{
		if (LevelGridMoving.firstMoveIdentifier) {
			pagesGrid.GetComponent<Movement> ().isMove = true;
			LevelGridMoving.firstMoveIdentifier = false;
		}
		if (LevelGridMoving.moveIdentifier) {
			pagesStroke [LevelGridMoving.shownId].GetComponent<Image> ().sprite = active;
			if (LevelGridMoving.shownId - 1 >= 0)
				pagesStroke [LevelGridMoving.shownId - 1].GetComponent<Image> ().sprite = inactive;
			if (LevelGridMoving.shownId + 1 < pagesStroke.Length)
				pagesStroke [LevelGridMoving.shownId + 1].GetComponent<Image> ().sprite = inactive;
			LevelGridMoving.moveIdentifier = false;
		}
			
	}


}
