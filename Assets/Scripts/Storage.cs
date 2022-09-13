using UnityEngine;

public static class UserData
{
    public static class Record
    {
        public static float Height { get; set; }
        public static int MaxCombo { get; set; }
    }
    
    public static class Cache
    {
        public static float Height { get; set; }
        public static int MaxCombo { get; set; }
    }

    public static int UserId { get; set; } = -1;
    public static string UserName { get; set; } = "Developer";
    public static string UserToken { get; set; } = "";
    public static float Brightness { get; set; } = 1;

    public static int ItemHat { get; set; }
    public static int ItemGlobe { get; set; }
    public static int ItemShose { get; set; }
    public static Color Color { get; set; }
    public static string ColorStr { get; set; }

    public static int StageCoin { get; set; }
    public static int Coin { get; set; }

    public static void Save()
    {
        PlayerPrefs.SetFloat("Brightness", Brightness);
        PlayerPrefs.SetInt("UserId", UserId);
        PlayerPrefs.SetString("UserName", UserName);
        PlayerPrefs.SetString("UserToken", UserToken);
        PlayerPrefs.SetFloat("Height", Record.Height);
        PlayerPrefs.SetInt("MaxCombo", Record.MaxCombo);
        PlayerPrefs.SetInt("Hat", ItemHat);
        PlayerPrefs.SetInt("Globe", ItemGlobe);
        PlayerPrefs.SetInt("Shose", ItemShose);
        PlayerPrefs.SetString("Color", ColorStr);
        PlayerPrefs.SetInt("Coin", Coin);
    }

    public static void Load()
    {
        Brightness = PlayerPrefs.GetFloat("Brightness", 1);
        UserId = PlayerPrefs.GetInt("UserId", -1);
        UserName = PlayerPrefs.GetString("UserName", "");
        UserToken = PlayerPrefs.GetString("UserToken", "");
        Record.Height = PlayerPrefs.GetFloat("Height", 0);
        Record.MaxCombo = PlayerPrefs.GetInt("MaxCombo", 0);
        ItemHat = PlayerPrefs.GetInt("Hat", 0);
        ItemGlobe = PlayerPrefs.GetInt("Globe", 0);
        ItemShose = PlayerPrefs.GetInt("Shose", 0);
        ColorStr = PlayerPrefs.GetString("Color", "#ff0000");
        Coin = PlayerPrefs.GetInt("Coin", 0);
    }

    public static void Clear()
    {
        PlayerPrefs.DeleteKey("Brightness");
        PlayerPrefs.DeleteKey("UserId");
        PlayerPrefs.DeleteKey("UserName");
        PlayerPrefs.DeleteKey("UserToken");
        PlayerPrefs.DeleteKey("Height");
        PlayerPrefs.DeleteKey("MaxCombo");
        PlayerPrefs.DeleteKey("Hat");
        PlayerPrefs.DeleteKey("Globe");
        PlayerPrefs.DeleteKey("Shose");
        PlayerPrefs.DeleteKey("Color");
        PlayerPrefs.DeleteKey("Coin");
    }

    static UserData()
    {
        Load();
    }
}
