using UnityEngine;
using System.Collections;
using System.IO;

public class Level{

	/// <summary>
	/// Конструктор для не пройденного уровня
	/// </summary>
	/// <param name="levelId">ID уровня</param>
	/// <param name="levelType">Тип уровня (проигранный, не игранный)</param>
	public Level(int levelId, TypeOfLevel levelType, int n, int m, int movesCount, int movesMid, int movesMin, int[] blocked, int[,] adjacencyMatrix, string[] levelsDataInfo)
	{
		LevelId = levelId;
		LevelType = levelType;
		N = n;
		M = m;
		MovesCount = movesCount;
		MovesMid = movesMid;
		MovesMin = movesMin;
		Blocked = blocked;
		AdjancencyMatrix = adjacencyMatrix;
		this.levelsDataInfo = levelsDataInfo;
	}

	/// <summary>
	/// Конструктор для пройденного уровня
	/// </summary>
	/// <param name="levelId">ID уровня</param>
	/// <param name="levelType">Тип уровня (выигранный)</param>
	/// <param name="starsCount">Количество звезд</param>
	public Level(int levelId, TypeOfLevel levelType, int n, int m, int starsCount, int movesCount, int movesMid, int movesMin, int[] blocked, int[,] adjacencyMatrix, string[] levelsDataInfo)
	{
		LevelId = levelId;
		LevelType = levelType;
		StarsCount = starsCount;
		N = n;
		M = m;
		MovesCount = movesCount;
		MovesMid = movesMid;
		MovesMin = movesMin;
		Blocked = blocked;
		AdjancencyMatrix = adjacencyMatrix;
		this.levelsDataInfo = levelsDataInfo;
	}

	public int[] Blocked {
		get;
		set;
	}

	//Id уровня
	public int LevelId { 
		get; 
		set;
	}

	//Тип уровня
	public TypeOfLevel LevelType {
		get;
		set;
	}

	//Количество звезд если уровень пройден
	public int StarsCount {
		get;
		set;
	}

	//Максимальное количество ходов
	public int MovesCount {
		get;
		set;
	}

	//Максимальное количество ходов
	public int MovesMin {
		get;
		set;
	}

	//Максимальное количество ходов
	public int MovesMid {
		get;
		set;
	}

	//Высота
	public int N {
		get;
		set;
	}

	//Длина
	public int M {
		get;
		set;
	}

	//Числовая двумерная матрица смежности
	public int[,] AdjancencyMatrix {
		get;
		set;
	}

	//Тип кнопки уровня
	public TypeOfButton ButtonType {
		get;
		set;
	}

	public bool IsBlocked(int index)
	{
		if (Blocked.Length == 0)
			return false;

		for (int i = 0; i < Blocked.Length; i++)
			if (index == Blocked [i])
				return true;

		return false;
	}


	//Содержание служебного файла прогресса игрока
	string[] levelsDataInfo;

	/// <summary>
	/// Обновление прогресса игрока
	/// </summary>
	public void UpdateLevelInfo()
	{
		string[] dataLevel = levelsDataInfo;
		if (LevelType == TypeOfLevel.played)
			dataLevel [LevelId] = "played; " + StarsCount;
		
		File.WriteAllLines(LevelsHandler.datapath, dataLevel);
	}
}
