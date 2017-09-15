using UnityEngine;
using UnityEngine.UI;
using System.Collections;


//Класс привязанный к объекту вершины на сцене и добавлющий ему свойства: индекс и видимость пользователю
public class PointScript : MonoBehaviour {

	public static bool IsAnimationInProcess;

	//Скорость поворота угла в секунду, т.е 180 градусов/сек
	public float rotationSpeed = -180;
	float _rotationTime = 0;
	public float fillingTime = 0;
	Color objectColor;

	void Start()
	{
		//Отключение вершин, пока не закончится анимация начала уровня
		IsAnimationInProcess = true;
		Invoke ("Prorogue", 1.4f);
	}

	/// <summary>
	/// Обработка нажатия на вершину
	/// </summary>
	void OnMouseUp () {
		//Проверка: заблокирована ли кнопка для нажатия или нет
		if (fillingTime==0 && Controller.shared.state == GameState.playing && !IsAnimationInProcess) {
			//Проверка: есть ли у вершины видимые ребра для поворота или у нее нет смежных ребер
			if (CheckForVisibleEdges() && !IsBlocked) {
				Controller.touchEvent = true;
				Controller.shared.GetTouch (Index);
				//Play sound
				this.transform.parent.GetComponent<Controller>().soundController.GetComponent<GameSoundController>().PopEffect2();
				//Время(_time) = расстояние необходимое для поворота (в данном случае 90 градусов) / скорость (speed)
				_rotationTime = Mathf.Abs (90 / rotationSpeed);
				//Блокировка кнопки на время анимации
				IsAnimationInProcess = true;
				//Разблокирование кнопки по истечению анимации
				Invoke ("Prorogue", 0.6f);
				//Создание вершины - подложки для анимации
				Point.setFilledPoint (Index, IsBlocked);
				objectColor = Filling.GetComponent<SpriteRenderer> ().material.color;
				fillingTime = 0.5f;
			} else {
				if (IsBlocked) {
					Point.setFilledPoint (Index, IsBlocked);
					objectColor = Filling.GetComponent<SpriteRenderer> ().material.color;
					fillingTime = 0.5f;
					this.transform.parent.GetComponent<Controller>().soundController.GetComponent<GameSoundController>().BadClick();
				}
				Handheld.Vibrate ();
			}
		}
	}

	//Разблокирование кнопки по окончанию анимации
	void Prorogue()
	{
		IsAnimationInProcess = false;
	}

	bool CheckForVisibleEdges()
	{
		if (CheckEdge (Index-1))
			return true;
		if (CheckEdge (Index+1))
			return true;
		if (CheckEdge (Index - Controller.levelInfo.M))
			return true;
		if (CheckEdge (Index + Controller.levelInfo.M))
			return true;
		return false;
	}

	bool CheckEdge(int x)
	{
		return Controller.shared.graph [Index, x].GetComponent<EdgeScript> ().IsConnected;
	}

	/// <summary>
	/// Анимация вращения всех ребер вокруг вершины
	/// </summary>
	void AnimateRotationEdges(){
		AnimateRotationEdge (Index - 1);
		AnimateRotationEdge (Index + 1);
		AnimateRotationEdge (Index - Controller.levelInfo.M);
		AnimateRotationEdge (Index + Controller.levelInfo.M);
	}

	/// <summary>
	/// Анимация вращения ребра
	/// </summary>
	/// <param name="x">Индекс второй вершины с которой соединяется ребро</param>
	void AnimateRotationEdge(int x){
		var edge = Controller.shared.graph [Index, x].transform;
		var point = Controller.shared.points [Index].transform;
		edge.RotateAround (point.position, Vector3.forward, rotationSpeed * Mathf.Min (Time.deltaTime, _rotationTime));
	}

	/// <summary>
	/// Анимация подложки
	/// </summary>
	public void AnimateFillingPoint()
	{
		Filling.transform.localScale += new Vector3 (Time.deltaTime * 1f / 0.5f, Time.deltaTime * 1f / 0.5f, 0);
		objectColor.a -= 0.6f*Time.deltaTime;
		Filling.GetComponent<SpriteRenderer> ().material.color = objectColor;
	}

	/// <summary>
	/// Анимация
	/// </summary>
	void Update(){
		if (_rotationTime > 0) {
			AnimateRotationEdges ();
			_rotationTime -= Time.deltaTime;
		} else {
			_rotationTime = 0;
		}

		if (fillingTime > 0) {
			AnimateFillingPoint ();
			fillingTime -= Time.deltaTime;
		} else if (Filling != null) {
			Destroy (Filling);
			fillingTime = 0;
		}
		else
			fillingTime = 0;

	}

	/// <summary>
	/// Ссылка на объект которым заполняется эта вершина
	/// </summary>
	public GameObject Filling {
		get;
		set;
	}

	public int Index {
		get{ return index; }
		set {
			index = value;
		}
	}

	public int index;

	public bool IsVisible {
		get;
		set;
	}

	public bool IsBlocked {
		get;
		set;
	}

	public void changeSprite()
	{
		this.gameObject.GetComponent<SpriteRenderer>().sprite = Controller.shared.pointGray;
	}

	public void changeSpriteByTime(float time)
	{
		Invoke ("changeSprite", time);
	}
}
