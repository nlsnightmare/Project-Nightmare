using System;
public static class PlayerStats {
    public static float HP;
    public static float maxHP;
    public static void Load(){
	HP = float.Parse(GameData.SaveData["playerCurrentHP"]);
	maxHP = float.Parse(GameData.SaveData["playerMaxHP"]);
    }

    public static float GetStat(string stat){
	switch (stat) {
	    case "currentHP":
		return HP;
	    case "maxHP":
		return maxHP;
		
	    default:
		throw new Exception("Player doesn't have the stat '" + stat + "'!");
	}
    }
}
