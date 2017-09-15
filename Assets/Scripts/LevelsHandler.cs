using UnityEngine;
using System.Collections;
using System.IO;
using System;

/// <summary>
/// Тип уровня (уже игрался или не игрался)
/// </summary>
public enum TypeOfLevel {played, unplayed};

public class LevelsHandler : MonoBehaviour {

	/// <summary>
	/// Массив хранящий текстовую интерпретацию уровня (txt format)
	/// </summary>
	public TextAsset[] levels;
	[HideInInspector]
	public string[] levelsDataInfo;

	string[] levelsDataInfostuff;
	[HideInInspector]
	public static Level[] levelArray;

	public static string datapath;

	/// <summary>
	/// Путь к файлу хранящему информацию о прогрессе игрока
	/// </summary>
	void Awake()
	{
		datapath = Path.Combine (Application.persistentDataPath, "levelsDataInfo.txt");
	}

	/// <summary>
	/// При генерации страницы уровней, их обработка и данных прогресса игрока
	/// </summary>
	void Start () {
		if (File.Exists (datapath))
			levelsDataInfostuff = File.ReadAllLines (datapath);
		else
			levelsDataInfostuff = new string[levels.Length + 1];

		levelsDataInfo = new string[levels.Length + 1];
		for (int i = 1; i < levels.Length+1; i++) {
			if (i >= levelsDataInfostuff.Length)
				levelsDataInfo [i] = "unplayed;";
			else if (levelsDataInfostuff [i] != null)
				levelsDataInfo [i] = levelsDataInfostuff [i];
			else
				levelsDataInfo [i] = "unplayed;";
		}
			
		File.WriteAllLines (datapath, levelsDataInfo);

		levelArray = new Level[levels.Length + 1];
		for (int i = 1; i < levels.Length + 1; i++) {
			levelArray [i] = ParseLevelFile (levels [i-1], i);
		}
	}

	/// <summary>
	/// Обработка текстового представления уровня
	/// </summary>
	/// <returns>Объект типа Level</returns>
	/// <param name="levelFilePath">Путь к текстовому файлу</param>
	public Level ParseLevelFile(TextAsset levelFile, int id)
	{
		//Переменные, хранящие данные об уровне, чтобы потом объявить объект класса Level
		int n, m, movesCount, movesMid, movesMin, starsCount = 0;
		TypeOfLevel levelType;

		//Обработка файла с информацией об уровнях
		string[] defineLevelInfoString = levelsDataInfo[id].Split(';');
		levelType = (TypeOfLevel)Enum.Parse (typeof(TypeOfLevel), defineLevelInfoString [0]);
		if (levelType == TypeOfLevel.played)
			starsCount = int.Parse (defineLevelInfoString [1]);

            
		//Обработка файла уровня
		string[] defineLevelString = levelFile.text.Split('\n');
		string[] firstStringData = defineLevelString [0].Split (';');

		n = int.Parse (firstStringData [0]);
		m = int.Parse (firstStringData [1]);
		movesCount = int.Parse (firstStringData [2]);
		movesMid = int.Parse (firstStringData [3]);
		movesMin = int.Parse (firstStringData [4]);

		int count = int.Parse (firstStringData [5]);
		int[] blocked = new int[count];
		for (int i = 0; i < count; i++) {
			blocked[i] = int.Parse (firstStringData [6 + i]);
		}
			


		//Обработка матрицы смежности
		int[,] adjacencyMatrix = new int[n * m, n * m];
		for (int i = 1; i < n * m + 1; i++) {
			string[] stuffHandlerStr = defineLevelString [i].Split (new char[]{ ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
			for (int j = 0; j < stuffHandlerStr.Length; j++) {
				adjacencyMatrix [i - 1, j] = int.Parse (stuffHandlerStr [j]);
			}
		}
			
		print (id);
		//Создание объекта класса Level в зависимости от данных в файле уровня
		if (levelType == TypeOfLevel.played) {
			return new Level (id, levelType, n, m, starsCount, movesCount, movesMid, movesMin, blocked, adjacencyMatrix, levelsDataInfo);
		} else
			return new Level (id, levelType, n, m, movesCount, movesMid, movesMin, blocked, adjacencyMatrix, levelsDataInfo);
	}
}
