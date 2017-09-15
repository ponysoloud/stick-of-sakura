using UnityEngine;
using System.Collections;

//public enum AnimMode {};

public class PointAnimFunction : MonoBehaviour {

	public float scaleMax, scaleExit, timeLength, timePeak;

	bool isMove;
	float scaleStart;

	public bool animInProcess;

	public bool isEmpty;

	/// <summary>
	/// Тригонометрическая функция задающая значение масштаба объекта
	/// </summary>
	/// <returns>Значение масштаба</returns>
	/// <param name="t">Время для которого высчитывается перемещение</param>
	public float TrigonometricFunc(float t)
	{
		if (t <= timePeak) {
			return scaleStart + (scaleMax-scaleStart)* Mathf.Sin (Mathf.PI / 2 / timePeak * t);
		} else {
			return (scaleMax + scaleExit) / 2 + (scaleMax - scaleExit) / 2 * Mathf.Cos (Mathf.PI/(timeLength-timePeak)*t-(timePeak/(timeLength-timePeak))*Mathf.PI);
		}
	}

	/// <summary>
	/// Запуск анимации, где задается один из параметров функции
	/// </summary>
	public void EnableAnimation()
	{
		//Anim
		scaleStart = this.gameObject.transform.localScale.x;
		time = 0f;
		isMove = true;

		//Play sound
		if (!isEmpty)
			this.transform.parent.GetComponent<Controller>().soundController.GetComponent<GameSoundController>().PopEffect1();
	}

	/// <summary>
	/// Запуск анимации с параметром задержки начала
	/// </summary>
	public void EnableAnimationByTime(float time)
	{
		Invoke ("EnableAnimation", time);
	}

	float time = 0f;
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
				this.gameObject.transform.localScale = scale * Vector3.one;
			}
		}
	}
}
