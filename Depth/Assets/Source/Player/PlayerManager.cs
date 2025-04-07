using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private const string _player_Position_Key_X = "playerPosX";
    private const string _player_Position_Key_Y = "playerPosY";
    private const string _player_Position_Key_Z = "playerPosZ";
    
    public void SavePlayerPosition(Vector3 position)
    {
        PlayerPrefs.SetFloat(_player_Position_Key_X, position.x);
        PlayerPrefs.SetFloat(_player_Position_Key_Y, position.y);
        PlayerPrefs.SetFloat(_player_Position_Key_Z, position.z);
        PlayerPrefs.Save();
    }
    
    public Vector3 LoadPlayerPosition()
    {
        float x = PlayerPrefs.GetFloat(_player_Position_Key_X, 0f); 
        float y = PlayerPrefs.GetFloat(_player_Position_Key_Y, 1f);
        float z = PlayerPrefs.GetFloat(_player_Position_Key_Z, 0f); 
        return new Vector3(x, y, z);
    }
    
    public void ResetPlayerPosition()
    {
        PlayerPrefs.DeleteKey(_player_Position_Key_X);
        PlayerPrefs.DeleteKey(_player_Position_Key_Y);
        PlayerPrefs.DeleteKey(_player_Position_Key_Z);
    }
}
