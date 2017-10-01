using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour {

    public static SaveManager m_instance
    {
        get; set;
    }

    public SaveState m_state;

    private void Awake()
    {
       // ResetSave();

        DontDestroyOnLoad(gameObject);
        m_instance = this;
        Load();

        // are we using the accelerometer?
        if(m_state.m_usingAccelerometor && !SystemInfo.supportsAccelerometer)
        {
            m_state.m_usingAccelerometor = false;
            Save();
        }
    }

    // save this script
    public void Save()
    {
         PlayerPrefs.SetString("save", Helper.Encrypt(Helper.Serialize<SaveState>(m_state)));
    }

    //Load previous save state
    public void Load()
    {
        if(PlayerPrefs.HasKey("save"))
        {
            m_state = Helper.Deserialize<SaveState>(Helper.Decrypt(PlayerPrefs.GetString("save")));
        }
        else
        {
            m_state = new SaveState();
            Save();
            Debug.Log("No save file found, creating a new one");
        }
    }

    public bool IsColorOwned(int index)
    {
        // check if bit is set, if so the color is owned
        return (m_state.m_colorOwned & (1 << index)) != 0;
    }

    public bool IsTrailOwned(int index)
    {
        // check if bit is set, if so the trail is owned
        return (m_state.m_trailOwned & (1 << index)) != 0;
    }

    public void UnlockColor(int index)
    {
        m_state.m_colorOwned |= 1 << index;
    }

    public void UnlockTrail(int index)
    {
        m_state.m_trailOwned |= 1 << index;
    }

    public void ResetSave()
    {
        PlayerPrefs.DeleteKey("save");
    }

    public bool BuyColor(int index, int cost)
    {
        if(m_state.m_gold >= cost)
        {
            m_state.m_gold -= cost;
            UnlockColor(index);

            // save progress
            Save();
            return true;
        }
        else
        {
            return false; 
        }
    }

    public bool BuyTrail(int index, int cost)
    {
        if(m_state.m_gold >= cost)
        {
            m_state.m_gold -= cost;
            UnlockTrail(index);

            // save progress
            Save();
            return true;
        }
        else
        {
            return false;
        }
    }

    public void CompleteLevel(int index)
    {
        if(m_state.m_completedLevel == index)
        {
            m_state.m_completedLevel++;
            Save();
        }
    }
}
