using UnityEngine;
using System.Collections;

public class EdgeAnimFunc : MonoBehaviour {

	public float scaleTarget=1f, timeLength=0.4f;
	public bool isHorizontal;

	bool isMove;
	float scaleStart;

	/// <summary>
	/// Тригонометрическая функция задающая значение масштаба объекта
	/// </summary>
	/// <returns>Значение масштаба</returns>
	/// <param name="t">Время для которого высчитывается перемещение</param>
	public float TrigonometricFunc(float t)
	{
		return scaleTarget * Mathf.Sin (Mathf.PI / 2 / timeLength * t);
	}

	/// <summary>
	/// Запуск анимации, где задается один из параметров функции
	/// </summary>
	public void EnableAnimation()
	{
		isMove = true;
	}

	/// <summary>
	/// Запуск анимации с параметром задержки начала
	/// </summary>
	public void EnableAnimationByTime(float time)
	{
		Invoke ("EnableAnimation", time);
	}

	float time = 0;
	void Update()
	{
		if (isMove) {
			if (time > timeLength || time < 0) {
				isMove = false;
				time = Mathf.Clamp (time, 0, timeLength);
			} else {
				time += Time.deltaTime;
				var tTime = time;
				time = Mathf.Clamp (time, 0, timeLength);
				var scale = this.TrigonometricFunc (time);
				time = tTime;
				this.gameObject.transform.parent.localScale = new Vector3 (isHorizontal ? scale : 1, isHorizontal ? 1 : scale, 1);
			}
		}
	}
}
