using UnityEngine;
using System.Collections;

public enum AnimType { hide, show};

public class TutorialTextAppearance : MonoBehaviour {

	float animationTime;
	Color colorVector;
	int a;

	void Start(){
		colorVector = this.gameObject.GetComponent<SpriteRenderer> ().material.color;
		colorVector.a = 0.0f;
		this.gameObject.GetComponent<SpriteRenderer> ().material.color = colorVector;
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
	public void TextAnimation(float time, AnimType mode){
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
		colorVector.a += a * 3f * Time.deltaTime;
		this.gameObject.GetComponent<SpriteRenderer> ().material.color = colorVector;
	}


}
