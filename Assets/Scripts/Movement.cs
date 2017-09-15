using UnityEngine;
using System.Collections;

public enum MoveAxis {x, y};

public class Movement : MonoBehaviour {

	/// <summary>
	/// Смещение параболы (координата по oy вершина параболы); Время длительности анимации; Время в котором достигается вершина параболы (координата по ox вершины параболы);
	/// </summary>
	public float bias, timeLong=1, t0;
	/// <summary>
	/// Начальное положение объекта
	/// </summary>
	[HideInInspector] public float x0, defaultx0; 
	/// <summary>
	/// Ось по которой движется объект
	/// </summary>
	public MoveAxis axis;

	public int counter = 0;

	/// <summary>
	/// Задаем начальные значения, при генерации объекта с данным компонентом
	/// </summary>
	void Awake()
	{
		var pos = GetComponent<RectTransform> ().localPosition;
		defaultx0 = axis==MoveAxis.x ? pos.x : pos.y;
		x0 = defaultx0;
		moveObject = this.gameObject;
	}

	/// <summary>
	/// Параболическая функция задающая анимацию движения
	/// </summary>
	/// <returns>Значение перемещения по x</returns>
	/// <param name="t">Время для которого высчитывается перемещение</param>
	public float ParabolFunc(float t)
	{
		//Debug.Log (((timeLong - t) * (x0 * (timeLong - t0) * (t0 - t) + bias * timeLong * t)) / (timeLong * (timeLong - t0) * t0));
		return ((timeLong - t) * (x0 * (timeLong - t0) * (t0 - t) + bias * timeLong * t)) / (timeLong * (timeLong - t0) * t0);
	}
		
	/// <summary>
	/// Изменяет координату {0, x0} на {0, -x0}, чтобы повторно применять анимацию к уже раз анимированному объекту
	/// </summary>
	public void ReverseCC()
	{
		x0 = x0 * (-1);
	}

	public void ContinueMod(bool mode, int side)
	{
		if (mode) {
			reverseMove = true;
			if (side == -1)
				x0 = Mathf.Abs (x0);
			else
				x0 = Mathf.Abs (x0) * (-1);
		} else {
			reverseMove = false;
		}
	}

	public void ResetX0()
	{
		var pos = GetComponent<RectTransform> ().localPosition;
		x0 = axis==MoveAxis.x ? pos.x : pos.y;
	}

	public bool isMove;
	public bool reverseMove = false;
	GameObject moveObject;

	float time = 0;

	/// <summary>
	/// Анимация
	/// </summary>
	public void Update()
	{
		if (isMove) 
		{
			if (time > moveObject.GetComponent<Movement> ().timeLong || time < 0) 
			{
				isMove = false;
				//reverseMove = time > moveObject.GetComponent<Movement> ().timeLong;
				time = Mathf.Clamp (time, 0, moveObject.GetComponent<Movement> ().timeLong);
			}
			else 
			{
				time += Time.deltaTime * (reverseMove ? -1 : 1);
				var tTime = time;
				time = Mathf.Clamp (time, 0, moveObject.GetComponent<Movement> ().timeLong);
				var pos = moveObject.GetComponent<Movement> ().ParabolFunc (time);
				time = tTime;
				moveObject.GetComponent<RectTransform> ().localPosition = pos*(moveObject.GetComponent<Movement> ().axis == MoveAxis.x ? Vector3.right : Vector3.up);
			}
		}
	}

}
