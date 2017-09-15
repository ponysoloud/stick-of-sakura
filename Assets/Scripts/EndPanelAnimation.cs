using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EndPanelAnimation : MonoBehaviour {


	float animationTime;
	Color colorVector;
	int a;

	void Start(){
		colorVector = this.gameObject.GetComponent<Image> ().color;
		colorVector.a = 0.0f;
		this.gameObject.GetComponent<Image> ().color = colorVector;
	}

	// Update is called once per frame
	void Update () {
		if (animationTime > 0) {
			MakeButtonInvisible ();
			animationTime -= Time.deltaTime;
		} else
			animationTime = 0;
	}

	/// <summary>
	/// Метод начала анимации
	/// </summary>
	public void Animation(float time, AnimType mode){
		animationTime = time;
		if (mode == AnimType.show)
			a = 1;
		else
			a = -1;
	}

	/// <summary>
	/// Анимация невидимости
	/// </summary>
	void MakeButtonInvisible()
	{
		colorVector.a += a * 1.4f * Time.deltaTime;
		this.gameObject.GetComponent<Image> ().color = colorVector;
	}
}
