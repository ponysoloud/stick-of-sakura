using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour {

	/// <summary>
	/// В процессе ли анимация кнопки
	/// </summary>
	public static bool IsAnimationInProcess = true;

	public static bool IsPressed = false;

	/// <summary>
	/// Отец всех UI объектов
	/// </summary>
	public GameObject parentGui;

	/// <summary>
	/// Префаб подложки кнопки "Старт" для анимации
	/// </summary>
	public GameObject backPrefab;

	float animationTime = 0, animationTimeInvise=0;
	Color stuffColor, objectColor;
	GameObject stuffGuiObject;
	int a;

	//Нажата ли только что кнопка, почти сразу после нажатия становится false(нужно для первого движения страниц уровней)/ Была ли нажата кнопка, после нажатия все время true
	public static bool pressed, waspressed = false;

	void Awake()
	{
		Application.targetFrameRate = 60;

		//Invoke ("InvokeAnimationInProcess", 1f);
	}

	void InvokeAnimationInProcess()
	{
		IsAnimationInProcess = false;
	}

	void LogAnimation(){
		print ("Animation is stopped!");
	}

	void OnMouseUp()
	{
		if (!DotsButtonAnim.creditsShowing) {
			if (!IsAnimationInProcess) {
				IsAnimationInProcess = true;
				pressed = true;
				waspressed = true;
				IsPressed = true;
				PreparationToAnimation (backPrefab);
				animationTime = 0.5f;
				a = 1;
				Invoke ("PrepareToMakeButtonInvisible", 0.5f);
				//Запускаем обучающие уровни если включен обучающий режим
				if (GuiController.tutorialEvent) {
					Controller.levelInfo = LevelsHandler.levelArray [1];
					Invoke ("ChangeScene", 1f);
				} else {
					Invoke ("ShowLevels", 1f);
				}
			}
			Invoke ("InvokeStaff", 1.1f);
		}
	}

	void InvokeStaff()
	{
		IsAnimationInProcess = false;
		this.gameObject.SetActive (false);
	}

	//0.5 sec
	public void SimulateClick()
	{
		waspressed = true;
		a = 1;
		Invoke ("PrepareToMakeButtonInvisible", 0f);
	}

	public void SimulateStart()
	{
		waspressed = false;
		a = -1;
		PrepareToMakeButtonInvisible ();
	}

	public void ShowLevels()
	{
		ShowLevelGrid ();
		Invoke ("InvokeMoveGui", 0.1f);
	}

	void ChangeScene()
	{
		SceneManager.LoadScene (Controller.GAME_SCENE);		
	}

	void ShowLevelGrid()
	{
		parentGui.GetComponent<GuiController>().GenerateLevelButtons ();
	}

	void InvokeMoveGui()
	{
		parentGui.transform.GetChild (0).gameObject.GetComponent<LevelGridMoving> ().Move(1);
	}
		
	/// <summary>
	/// Создание копии кнопки для последующей анимации
	/// </summary>
	/// <param name="guiObject">Объект кнопки</param>
	public void PreparationToAnimation(GameObject guiObject){
		stuffGuiObject = GameObject.Instantiate (guiObject, new Vector3 (0, 0, 1), Quaternion.identity) as GameObject;
		stuffGuiObject.transform.parent = this.gameObject.transform;
		objectColor = stuffGuiObject.GetComponent<SpriteRenderer> ().material.color;
		objectColor.a = 0.3f;
		stuffGuiObject.GetComponent<SpriteRenderer> ().material.color = objectColor;
	}

	/// <summary>
	/// Анимация кнопки
	/// </summary>
	public void ButtonAnimation(){
		stuffGuiObject.transform.localScale += new Vector3 (Time.deltaTime * 1f / 0.5f, Time.deltaTime * 1f / 0.5f, 0);
		objectColor.a -= 0.6f*Time.deltaTime;
		stuffGuiObject.GetComponent<SpriteRenderer> ().material.color = objectColor;
	}

	/// <summary>
	/// Подготовка сделать основную кнопку "Старт" невидимой
	/// </summary>
	void PrepareToMakeButtonInvisible(){
		stuffColor = this.gameObject.GetComponent<SpriteRenderer> ().material.color;
		animationTimeInvise = 0.5f;
	}

	/// <summary>
	/// Анимация невидимости основной кнопки
	/// </summary>
	void MakeButtonInvisible()
	{
		stuffColor.a -= a*2f*Time.deltaTime;
		this.gameObject.GetComponent<SpriteRenderer> ().material.color = stuffColor;
	}
		
		
	/// <summary>
	/// Анимация
	/// </summary>
	void Update () {
		if (animationTime > 0) {
			ButtonAnimation ();
			animationTime -= Time.deltaTime;
		} else
			animationTime = 0;

		if (animationTimeInvise > 0) {
			MakeButtonInvisible ();
			animationTimeInvise -= Time.deltaTime;
		} else
			animationTimeInvise = 0;
	}


}
