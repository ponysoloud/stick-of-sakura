using UnityEngine;
using System.Collections;

//Класс служащий для первичного создания объекта ребра на сцене и задания этому объекту определенных свойств
public class Edge : MonoBehaviour {

	static float _x, _y;
	static Quaternion _rotation;

	/// <summary>
	/// Метод инициализации объекта ребра на сцене
	/// </summary>
	/// <returns>Если ребро существует между заданными вершинами, то метод возвращает ссылку на соответсвующий объект ребра на сцене, иначе null</returns>
	/// <param name="pointA">Начальная точка ребра</param>
	/// <param name="pointB">Конечная точка ребра</param>
	/// <param name="isConnected">Соединяющее ребро или несоединяющее</param>
	/// <param name="connectedEdge">Префаб соединяющего ребра</param>
	/// <param name="disconnectedEdge">Префаб несоединяющего ребра</param>
	public static GameObject setEdge(GameObject pointA, GameObject pointB, bool isConnected)
	{
		//Отсекаем ненужные ребра между невидимыми вершинами
		if (!(!pointA.GetComponent<PointScript> ().IsVisible && !pointB.GetComponent<PointScript> ().IsVisible)) {
			//Определяем ориентацию ребра(горизонтальная или вертикальная)
			if (pointB.GetComponent<PointScript> ().Index - pointA.GetComponent<PointScript> ().Index == 1) {
				_x = (pointA.transform.position.x + pointB.transform.position.x) / 2;
				_y = pointA.transform.position.y;
				_rotation = Quaternion.Euler (0, 0, 0);
			} else {
				_x = pointA.transform.position.x;
				_y = (pointA.transform.position.y + pointB.transform.position.y) / 2;
				_rotation = Quaternion.Euler (0, 0, 90);
			}

			//Создание объекта ребро со свойством IsConnected
			GameObject stuffEdgeObject = GameObject.Instantiate (isConnected ? Controller.shared.connectedEdge : Controller.shared.disconnectedEdge, new Vector2 (_x, _y), _rotation) as GameObject;
			stuffEdgeObject.GetComponent<EdgeScript> ().IsConnected = isConnected;

			//Создание pivot
			if (isConnected) {
				var pivot = new GameObject ("pivot");
				pivot.transform.parent = Controller.shared.parentObject.transform;
				pivot.transform.position = pointA.GetComponent<PointScript> ().IsVisible ? pointA.transform.position : pointB.transform.position;
				stuffEdgeObject.GetComponent<EdgeScript> ().Pivot = pivot;

				//Делаем сгенерированный объект дочерним отцу
				stuffEdgeObject.transform.parent = pivot.transform;

				//Задаем свойство горизонтальности
				stuffEdgeObject.GetComponent<EdgeAnimFunc> ().isHorizontal = _rotation == Quaternion.Euler (0, 0, 0) ? true : false;
				//Устанавливаем начальные значения для начала анимации
				pivot.transform.localScale = new Vector3 (stuffEdgeObject.GetComponent<EdgeAnimFunc> ().isHorizontal ? 0 : 1, stuffEdgeObject.GetComponent<EdgeAnimFunc> ().isHorizontal ? 1 : 0, 1);
				//Начинаем анимацию
				var k =Random.Range(0f, 0.6f);
				stuffEdgeObject.GetComponent<EdgeAnimFunc> ().EnableAnimationByTime (0.7f + k);
			} else
				stuffEdgeObject.transform.parent = Controller.shared.parentObject.transform;


			return stuffEdgeObject;
		} else
			return null;
	}

}
