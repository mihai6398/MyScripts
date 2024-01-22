using UnityEngine;
using System;

public static class PlayerStatistics
{
    private const string FIRST_LOG_IN_TAG = "First log in";
    private const string LAST_LOG_IN_TAG = "Last log in";
    private const string DAY_IN_GAME_ROW_TAG = "Player days in game a row";
    private const string DAY_IN_GAME_TAG = "Player days in game";

    public static bool IsFirstSession => _isFirstSession;
    public static int DayInGameRow => GetDaysInGameRow();
    public static int DayInGame => GetDaysInGame();
    public static DateTime StartGameSession { get; private set; } = DateTime.Now.ToUniversalTime();

    private static int DayRecess => (DateTime.Now.ToUniversalTime().Date - _lastLogIn.ToUniversalTime().Date).Days;

    private static DateTime _firstLogIn = DateTime.Now.ToUniversalTime();
    private static DateTime _lastLogIn = DateTime.Now.ToUniversalTime();
    private static bool _isFirstSession = true;

    public static void LogIn()
    {
        Debug.Log($"PlayerStatistics -> loading save dates");
        _isFirstSession = string.IsNullOrEmpty(PlayerPrefs.GetString(FIRST_LOG_IN_TAG, null));
        _firstLogIn = LoadDate(FIRST_LOG_IN_TAG);
        _lastLogIn = LoadDate(LAST_LOG_IN_TAG);
        StartGameSession = DateTime.Now.ToUniversalTime();
        int dayRecess = DayRecess;

        RefreshData();

        Debug.Log($"Is First Session {_isFirstSession};  Day recess {dayRecess};\tDays in game {GetDaysInGame()};\tDays in game row {GetDaysInGameRow()};\n");
        Debug.Log($"\tLast LogIn\t{_lastLogIn};\n\t\tFirst LogIn\t{_firstLogIn};");
    }

    public static void TimeStamp()
    {
        Debug.Log($"Log Out {LAST_LOG_IN_TAG}  {DateTime.Now}");
        _isFirstSession = false;
        RefreshData();
    }

    private static void RefreshData()
    {
        Debug.Log($"PlayerStatistics -> Refreshing date");
        GetDaysInGame();
        GetDaysInGameRow();
        _lastLogIn = DateTime.Now.ToUniversalTime();
        SaveDate(LAST_LOG_IN_TAG, _lastLogIn);
    }

    private static int GetDaysInGameRow()
    {
        int dayInGameRow = PlayerPrefs.GetInt(DAY_IN_GAME_ROW_TAG, 1);
        int dayRecess = DayRecess;

        if (dayRecess == 1)
            dayInGameRow++;
        else if (dayRecess != 0)
            dayInGameRow = 1;

        if (dayRecess == 0) 
            return dayInGameRow;
        
        PlayerPrefs.SetInt(DAY_IN_GAME_ROW_TAG, dayInGameRow);
        PlayerPrefs.Save();
        return dayInGameRow;
    }

    private static int GetDaysInGame()
    {
        int dayInGame = PlayerPrefs.GetInt(DAY_IN_GAME_TAG, 1);
        int dayRecess = DayRecess;

        if (dayRecess <= 0) 
            return dayInGame;
        
        dayInGame++;
        PlayerPrefs.SetInt(DAY_IN_GAME_TAG, dayInGame);
        PlayerPrefs.Save();
        return dayInGame;
    }

    private static void SaveDate(string tag, DateTime date)
    {
        PlayerPrefs.SetString(tag, date.ToUniversalString());
        PlayerPrefs.Save();
    }

    private static DateTime LoadDate(string tag)
    {
        string dateStr = PlayerPrefs.GetString(tag, null);
        DateTime dateTime = dateStr.ToUniversalDateTime();
        
        if (dateTime.Ticks != 0)
            return dateTime;

        dateTime = DateTime.Now.ToUniversalTime();
        SaveDate(tag, dateTime);
        return dateTime;
    }
}