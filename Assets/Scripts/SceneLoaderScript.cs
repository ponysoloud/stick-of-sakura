using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.IO;

public class SceneLoaderScript : MonoBehaviour {

	void Start () {
		print (1);
		//Если создан файл данных уровней...
		if (File.Exists (Path.Combine (Application.persistentDataPath, "levelsDataInfo.txt"))) {
			//...считываем содержимое файла
			print(2);
			string[] levelsDataInfostuff = File.ReadAllLines (Path.Combine (Application.persistentDataPath, "levelsDataInfo.txt"));
			//Если обучающие уровни не сыграны/не пройдены включаем tutorialmode
			if (levelsDataInfostuff [1] == "unplayed;" || levelsDataInfostuff [2] == "unplayed;")
				GuiController.tutorialEvent = true;
			else
				GuiController.tutorialEvent = false;
		} else
			GuiController.tutorialEvent = true;

		print (GuiController.tutorialEvent);
		SceneManager.LoadScene (Controller.MENU_SCENE);
	}
}
