using UnityEngine;
using System.Collections;

//Класс служащий для первичного создания объекта вершины на сцене и задания этому объекту определенных
public class Point : MonoBehaviour {

	//const float SIZE = 4.5f;
	const float SIZE = 5f;

	static float _coorx, _coory;
	static int m;
	static int n;

	/// <summary>
	/// Метод инициализации объекта вершины на сцене 
	/// </summary>
	/// <returns>Ссылку на вершину на сцене</returns>
	/// <param name="index">Индекс вершины</param>
	public static GameObject setPoint(int index, bool isBlocked)
	{
		m = Controller.levelInfo.M;
		n = Controller.levelInfo.N;
		_coory = (index / m - (float)(n - 1) / 2) * SIZE;
		_coorx = (index % m - (float)(m - 1) / 2) * SIZE;


		//Создание объекта вершина со свойством index / Quaternion.identity - нулевое значение rotation
		GameObject stuffPointObject =  GameObject.Instantiate (Controller.shared.point, new Vector2 (_coorx, _coory), Quaternion.identity) as GameObject;
		stuffPointObject.GetComponent<PointScript> ().Index = index;
		stuffPointObject.GetComponent<PointScript> ().IsBlocked = isBlocked;
		if (isBlocked)
			stuffPointObject.GetComponent<SpriteRenderer> ().sprite = Controller.shared.pointBlocked;

		//Делаем объект дочерним отцу
		stuffPointObject.transform.parent = Controller.shared.parentObject.transform;

		//Решаю, какие вершины нужно видеть пользователю, а какие нет (программно, каждый объект сохраняется, чтобы далее строить ребра)
		if (index / m != 0 && index / m != n - 1 && index % m != 0 && index % m != m - 1) {
			stuffPointObject.GetComponent<PointScript> ().IsVisible = true;
			//Задаем начальный масштаб для анимации появления вершины
			stuffPointObject.transform.localScale = Vector3.zero;
			//Включаю анимацию
			var k = Random.Range(0f, 0.7f);
			stuffPointObject.GetComponent<PointAnimFunction> ().EnableAnimationByTime (k);
			if (isBlocked) {
				GameObject it = setFilledPointDark (index);
				it.GetComponent<PointAnimFunction> ().EnableAnimationByTime (k + 0.2f);
				stuffPointObject.GetComponent<PointScript> ().changeSpriteByTime (k + 0.9f);
				Destroy (it, k + 0.95f);
			}
			//Sound
			//this.gameObject.GetComponent<PointScript> ().soundController.GetComponent<GameSoundController> ().PopEffect ();
		} else {
			stuffPointObject.GetComponent<PointScript> ().IsVisible = false;
			stuffPointObject.GetComponent<SpriteRenderer> ().enabled = false;
			stuffPointObject.GetComponent<Collider2D> ().enabled = false;
		}

		return stuffPointObject;
	}

	/// <summary>
	/// Инициализация вершины-"подложки" для анимации нажатия на вершину
	/// </summary>
	/// <param name="index">Индекс кнопки</param>
	public static void setFilledPoint(int index, bool isBlocked)
	{
		m = Controller.levelInfo.M;
		n = Controller.levelInfo.N;

		_coory = (index / m - (float)(n - 1) / 2) * SIZE;
		_coorx = (index % m - (float)(m - 1) / 2) * SIZE;

		GameObject stuffPointObject =  GameObject.Instantiate (Controller.shared.filledPoint, new Vector3 (_coorx, _coory, -1), Quaternion.identity) as GameObject;
		if (isBlocked)
			stuffPointObject.GetComponent<SpriteRenderer> ().sprite = Controller.shared.pointBlocked;
		
		Controller.shared.points [index].GetComponent<PointScript> ().Filling = stuffPointObject;
		stuffPointObject.transform.parent = Controller.shared.points [index].transform;
		Color objectColor = stuffPointObject.GetComponent<SpriteRenderer> ().material.color;
		objectColor.a = 0.3f;
		stuffPointObject.GetComponent<SpriteRenderer> ().material.color = objectColor;
	}

	public static GameObject setFilledPointDark(int index)
	{
		m = Controller.levelInfo.M;
		n = Controller.levelInfo.N;

		_coory = (index / m - (float)(n - 1) / 2) * SIZE;
		_coorx = (index % m - (float)(m - 1) / 2) * SIZE;

		GameObject stuffPointObject =  GameObject.Instantiate (Controller.shared.blockedGray, new Vector3 (_coorx, _coory, -1), Quaternion.identity) as GameObject;
		stuffPointObject.transform.localScale = Vector3.zero;
		return stuffPointObject;
	}
}
