using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class MakeObjInvisible : MonoBehaviour {


	float animationTime = 0;
	float opacity = 0f;
	int a; 
	Color stuffColor;

	public void Start() 
	{
		this.GetComponent<Image> ().color = new Color (239f/255, 243f/255, 247f/255, 0); 
	}

	public void MakeInvisible(float animTime)
	{
		a = 1;
		animationTime = animTime;
	}

	public void MakeVisible(float animTime)
	{
		a = -1;
		animationTime = animTime;
	}
		
	void ChangeOpacity()
	{
		opacity += a * 2f * Time.deltaTime;
		this.GetComponent<Image> ().color = new Color (239f/255, 243f/255, 247f/255, opacity);
	}
		
	/// <summary>
	/// Анимация
	/// </summary>
	void Update () 
	{
		if (animationTime > 0) 
		{
			ChangeOpacity ();
			animationTime -= Time.deltaTime;
		} else
			animationTime = 0;
	}
}
