using UnityEngine;
using System.Collections;

//Класс привязанный к объекту ребро на сцене и добавлющий ему свойство соединения
public class EdgeScript : MonoBehaviour {

	public bool IsConnected {
		get;
		set;
	}

	public GameObject Pivot {
		get;
		set;
	}
}
