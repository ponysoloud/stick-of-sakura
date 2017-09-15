using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

//Состояние игры: в процессе, закончена
public enum GameState {playing, ended}; 

//
public enum EndState {win, lost};

public class Controller : MonoBehaviour {

	public static string GAME_SCENE = "Game_scene_iphone";//SystemInfo.deviceModel.Contains("iPad")?"Game_scene_ipad":"Game_scene_iphone"; 
	public static string MENU_SCENE = "Menu_scene_iphone";

	public static bool touchEvent;
	static public Level levelInfo;
	//Общедоступная переменная хранящая объект - отца всех генерируемых объектов
	[HideInInspector]
	public GameObject parentObject;
	//Префаб вершины
	public GameObject point;
	//Спрайт для анимации темной кнопки
	public Sprite pointBlocked;
	//Префаб для анимации темной кнопки
	public GameObject blockedGray;
	//Префаб для анимации темной кнопки
	public Sprite pointGray;
	//Префаб подложки вершины
	public GameObject filledPoint;
	//Префаб переднего фона
	public GameObject frontgroundPrefab;
	//Префабы соединяющего и несоединяющего ребер
	public GameObject connectedEdge, disconnectedEdge;
	//Матрица связности графа//
	public GameObject[,] graph;
	//Массив вершин графа//
	public GameObject[] points;
	//Служебная матрица для проверки связности
	static byte?[] _stuffConnectivity;
	//Состояние игры по умолчанию//
	public GameState state = GameState.playing;

	public GameObject parentGui;

	public GameObject soundController;

	bool endChecker;


	EndState endState;

	public GameObject firstLevelTutorial, secondLevelTutorial;

	//Всего видимых вершин //
	int _countOfVisible = 0;
	int movesCount;

	GameObject frontground;
	Color objectColor;
	float uninvisibleTime;

	public Text movesCounter;

	public static Controller shared;
	void Awake()
	{
		touchEvent = false;
		shared = this;
		graph = new GameObject[levelInfo.M * levelInfo.N, levelInfo.M * levelInfo.N];
		points = new GameObject[levelInfo.M * levelInfo.N];
		_stuffConnectivity = new byte?[levelInfo.M * levelInfo.N];
	}
	/// <summary>
	/// Начальная генерация сцены
	/// </summary>
	public void Start ()
	{
		if (!GuiController.tutorialEvent) {
			//PlayMusic
			soundController.GetComponent<GameSoundController> ().PlayMusic ();
		}

		//Создаем передний фон для анимации затемнения
		frontground = GameObject.Instantiate (frontgroundPrefab, new Vector3 (0, 0, -3), Quaternion.identity) as GameObject; 
		objectColor = frontground.GetComponent<SpriteRenderer> ().material.color;
		objectColor.a = 0.0f;
		frontground.GetComponent<SpriteRenderer> ().material.color = objectColor;

		//Обработка входных данных об уровне для генерации сцены
		movesCount = levelInfo.MovesCount;
		movesCounter.text = movesCount.ToString () + " moves";
		state = GameState.playing;

		//Определяем текущий объект-генератор, как "отца"
		parentObject = this.gameObject;

		//---------------------------------
		//Создаем вершины на сцене
		//Циклом от 1 до m*n-1 создаем объекты класса Point тем самым инициализируя их на сцене, не учитываем угловые
		for (int i = 1; i < levelInfo.M * levelInfo.N - 1; i++) {
			if (i != levelInfo.M - 1 && i != levelInfo.M * (levelInfo.N - 1)) {
				//Создаю объект вершины
				points [i] = Point.setPoint (i, levelInfo.IsBlocked(i));
				//Подсчет видимых вершин
				if (points [i].GetComponent<PointScript> ().IsVisible)
					_countOfVisible++;
			}
		}

		//---------------------------------
		//Создаем ребра на сцене
		for (int i = 0; i < levelInfo.M * levelInfo.N; i++) {
			if (points [i] != null) {
				if (i + 1 < levelInfo.N * levelInfo.M && points [i + 1] != null) {
					//Random.Range (1, 3) == 1
					graph [i, i + 1] = Edge.setEdge (points [i], points [i + 1], levelInfo.AdjancencyMatrix[i, i+1]==1);
					graph [i + 1, i] = graph [i, i + 1];
				}

				//Создаю вертикальное ребро 
			
				if (i + levelInfo.M < levelInfo.N * levelInfo.M && points [i + levelInfo.M] != null) {
					graph [i, i + levelInfo.M] = Edge.setEdge (points [i], points [i + levelInfo.M], levelInfo.AdjancencyMatrix [i, i + levelInfo.M]==1);
					graph [i + levelInfo.M, i] = graph [i, i + levelInfo.M];
				}
			}
		}

		//Масштабирование сгенерированных объектов в зависимости от количества вершин, так чтобы все были видны на сцене
		//parentObject.transform.localScale = SetScale();
	}

	/// <summary>
	/// Метод обработки касания вершины
	/// </summary>
	/// <param name="pointIndex">Индекс нажатой вершины</param>
	public void GetTouch (int pointIndex) {
		//Меняем местами ребра в матрице смежности
		//Меняем местами ребра в строке нажатой вершины
		GameObject staffObject1 = graph [pointIndex, pointIndex - 1];
		graph [pointIndex, pointIndex - 1] = graph [pointIndex, pointIndex - levelInfo.M];
		graph [pointIndex, pointIndex - levelInfo.M] = graph [pointIndex, pointIndex + 1];
		graph [pointIndex, pointIndex + 1] = graph [pointIndex, pointIndex + levelInfo.M];
		graph [pointIndex, pointIndex + levelInfo.M] = staffObject1;

		//Меняем местами "побочные" ребра
		GameObject staffObject2 = graph [pointIndex - 1, pointIndex];
		graph [pointIndex - 1, pointIndex] = graph [pointIndex - levelInfo.M, pointIndex];
		graph [pointIndex - levelInfo.M, pointIndex] = graph [pointIndex + 1, pointIndex];
		graph [pointIndex + 1, pointIndex] = graph [pointIndex + levelInfo.M, pointIndex];
		graph [pointIndex + levelInfo.M, pointIndex] = staffObject2;

		movesCount--;
		movesCounter.text = movesCount.ToString()+" moves";

		endChecker = Connectivity (pointIndex);
		Invoke ("CheckForEnd", 0.55f);
	}

	void CheckForEnd()
	{
		//Проверка на конец игры
		if (endChecker) {
			print ("Win");
			state = GameState.ended;
			endState = EndState.win;

			//Win Sound
			if (!GuiController.tutorialEvent)
				soundController.GetComponent<GameSoundController>().WinEffect(0.6f);
			else
				soundController.GetComponent<GameSoundController>().ShortWinEffect();

			//Запись обновленных данных об уровне
			levelInfo.LevelType = TypeOfLevel.played;
			if (movesCount <= levelInfo.MovesMin && levelInfo.StarsCount < 1)
				levelInfo.StarsCount = 1;
			else if (movesCount <= levelInfo.MovesMid / 3f && levelInfo.StarsCount < 2)
				levelInfo.StarsCount = 2;
			else
				levelInfo.StarsCount = 3;
			print (movesCount);
			
			levelInfo.UpdateLevelInfo ();
			print (levelInfo.StarsCount);

			//Если было обучение и оно закончилось - выйти в меню
			if (GuiController.tutorialEvent) {
				if (levelInfo.LevelId == 1) {
					TutorialContinueEvent ();
				} else if (levelInfo.LevelId == 2) {
					uninvisibleTime = 1f;
					Invoke ("InvokeExit", 1f);
				} 
			} 
			else {
					//levelInfo = LevelsHandler.levelArray [levelInfo.LevelId + 1];
					//Вызов анимаций
					Invoke ("EndAnimation", 0f);
					//Invoke ("InvokeLoadLevel", 60f);
			}
		} else if (movesCount == 0) {
			state = GameState.ended;
			//Запись обновленных данных об уровне
			endState = EndState.lost;

			print ("Lose");
			//Lost Sound
			soundController.GetComponent<GameSoundController>().LoseEffect(1f);

			//Вызов анимаций
			Invoke ("EndAnimation", 0f);
			//Invoke ("InvokeLoadLevel", 60f);
		}
	}

	void InvokeLoadLevel()
	{
		SceneManager.LoadScene(Controller.GAME_SCENE);
	}

	void InvokeExit()
	{
		parentGui.GetComponent<GameGuiController> ().ExitClickEvent ();
	}
	/// <summary>
	/// Рекурсивный поиск в глубину
	/// </summary>
	/// <param name="i">Вершина</param>
	void CheckTheState(int i)
	{
		_stuffConnectivity [i] = 1;
		if (graph [i, i + 1].GetComponent<EdgeScript>().IsConnected && _stuffConnectivity [i + 1] == 0)
			CheckTheState (i + 1);
		
		if (graph [i, i - 1].GetComponent<EdgeScript>().IsConnected && _stuffConnectivity [i - 1] == 0)
			CheckTheState (i - 1);
		
		if (graph [i, i + levelInfo.M].GetComponent<EdgeScript>().IsConnected && _stuffConnectivity [i + levelInfo.M] == 0)
			CheckTheState (i + levelInfo.M);
		
		if (graph [i, i - levelInfo.M].GetComponent<EdgeScript>().IsConnected && _stuffConnectivity [i - levelInfo.M] == 0)
			CheckTheState (i - levelInfo.M);
	}

	/// <summary>
	/// Проверка связности графа (конец игры)
	/// </summary>
	/// <param name="pointIndex">Вершина от которой начинаем проверку</param>
	public bool Connectivity(int pointIndex)
	{
		int staffCount = 0;

		//Отчистка служебного массива перед новой проверкой
		for (int i = 0; i < levelInfo.M * levelInfo.N; i++) {
			if (points [i] != null)
			if (points [i].GetComponent<PointScript>().IsVisible)
				_stuffConnectivity [i] = 0;	
		}

		//Рекурентное заполнение служебного массива (поиск в глубину)
		CheckTheState (pointIndex);

		//Проверка результата выполнения поиска
		for (int i = 0; i < levelInfo.M * levelInfo.N; i++) {
			if (_stuffConnectivity [i] == 1)
				staffCount++;
			/*
			if(points [i])
				points [i].GetComponent<SpriteRenderer> ().color = _stuffConnectivity [i] == 1 ? Color.yellow : Color.white;
			*/
		}

		//Вывод результата (связный или не связный граф)
		return (staffCount==_countOfVisible)? true : false;
	}

	/// <summary>
	/// Вызов анимаций по окончанию игры
	/// </summary>
	public void EndAnimation()
	{
		//Stop Music
		soundController.GetComponent<GameSoundController> ().StopMusic ();

		uninvisibleTime = 1f;
		Invoke ("InvokeGuiAnim", 1.3f);
	}

	void InvokeGuiAnim()
	{
		parentGui.GetComponent<GameGuiController> ().EndAnimation (endState == EndState.lost?0:movesCount <= levelInfo.MovesMin?1:movesCount <= levelInfo.MovesMid?2:3);
		print (levelInfo.StarsCount);
	}

	/// <summary>
	/// Анимация затемнения
	/// </summary>
	void UninvisibleFrontground()
	{
		objectColor.a += 1f*Time.deltaTime;
		frontground.GetComponent<SpriteRenderer> ().material.color = objectColor;
	}

	/// <summary>
	/// Нажатие по кнопке "Restart"
	/// </summary>
	public void Restart()
	{
		//АНИМАЦИЮ ДОБАВИТЬ
		uninvisibleTime = 1f;
		Invoke ("LoadLevel", 1.3f);
	}

	public void RestartFromEnd()
	{
		//АНИМАЦИЮ ДОБАВИТЬ
		//uninvisibleTime = 2f;
		Invoke ("LoadLevel", 0.2f);
	}

	void LoadLevel()
	{
		//levelInfo = LevelsHandler.levelArray [levelInfo.LevelId + 1];
		SceneManager.LoadScene (Controller.GAME_SCENE);
	}

	public void Continue()
	{
		levelInfo = LevelsHandler.levelArray [levelInfo.LevelId + 1];
		SceneManager.LoadScene (Controller.GAME_SCENE);
	}
		
	bool shown;

	/// <summary>
	/// Проверка нужно ли обучение
	/// </summary>
	void CheckForTutorial()
	{
		if (levelInfo.LevelId == 1 && !shown) {
			Invoke("FirsLevelTutorialEvent", 1.5f);
			shown = true;
		} else if (levelInfo.LevelId == 2 && !shown) {
			Invoke("SecondLevelTutorialEvent", 1.5f);
			shown = true;
		}
	}

	void FirsLevelTutorialEvent()
	{
		firstLevelTutorial.SetActive (true);
		firstLevelTutorial.GetComponent<TutorialTextAppearance> ().TextAnimation (0.3f, AnimType.show);
	}

	void SecondLevelTutorialEvent ()
	{
		secondLevelTutorial.SetActive (true);
		secondLevelTutorial.GetComponent<TutorialTextAppearance> ().TextAnimation (0.3f, AnimType.show);
		InvokeRepeating("RepeatAction", 0.5f, 1f);
	}

	//Мигание вершины в режими туториала
	void RepeatAction()
	{
		points [7].GetComponent<PointAnimFunction> ().EnableAnimation ();
		if (touchEvent) CancelInvoke("RepeatAction");
	}
    
	void Update()
	{
		if (Input.GetKeyDown (KeyCode.O)) {
			endChecker = true;
			CheckForEnd ();
		}

		if (uninvisibleTime > 0) {
			UninvisibleFrontground ();
			uninvisibleTime -= Time.deltaTime;
		} else
			uninvisibleTime = 0;

		if (GuiController.tutorialEvent)
			CheckForTutorial ();	
	}

	//Переход между обучающими уровнями
	public void TutorialContinueEvent()
	{
		uninvisibleTime = 1f;
		Invoke ("Continue", 1f);
	}

	//Начало обучения
	public void StartTutorial()
	{
		GuiController.tutorialEvent = true;
		levelInfo = LevelsHandler.levelArray [1];
		print (levelInfo);
		SceneManager.LoadScene (Controller.GAME_SCENE);
	}
	/// <summary>
	/// Определение масштаба генерируемого графа, чтобы граф был весь виден на сцене
	/// </summary>
	/// <returns>Значение масштаба в формате Vector3</returns>
	 /*
	public Vector3 SetScale()
	{
		float maxValueX = (((float)Screen.width / Screen.height >= 5f / 4f) && ((float)Screen.width / Screen.height <= 4f / 3f)) ? 12f : 16f;
		float maxValueY = (((float)Screen.width / Screen.height >= 5f / 4f) && ((float)Screen.width / Screen.height <= 4f / 3f)) ? 8f : 8f;
		float scaley = maxValueY / points [levelInfo.M * levelInfo.N - 2].transform.position.y >= 1 ? 1 : maxValueY / points [levelInfo.M * levelInfo.N - 2].transform.position.y;
		float scalex = maxValueX / points [2 * levelInfo.M - 1].transform.position.x >= 1 ? 1 : maxValueX / points [2 * levelInfo.M - 1].transform.position.x;
		return new Vector3(Mathf.Min (scaley, scalex), Mathf.Min (scaley, scalex), Mathf.Min (scaley, scalex));
	}*/
		
}