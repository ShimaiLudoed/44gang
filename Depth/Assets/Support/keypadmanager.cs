using System.Collections.Generic;
using UnityEngine;

public class KeypadManager : MonoBehaviour
{
    public static KeypadManager Instance;

    private Dictionary<string, bool> accessStates = new Dictionary<string, bool>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public bool IsAccessGranted(string id)
    {
        return accessStates.ContainsKey(id) && accessStates[id];
    }

    public void SetAccessGranted(string id)
    {
        if (accessStates.ContainsKey(id))
        {
            accessStates[id] = true;
        }
        else
        {
            accessStates.Add(id, true);
        }
    }
}
