using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuScene : MonoBehaviour
{

    private CanvasGroup m_fadeGroup;
    private float m_fadeInSpeed = 0.33f;

    public Transform m_colorPanel;
    public Transform m_trailPanel;
    public Transform m_levelPanel;
    public RectTransform m_menuContainer;

    public Text m_colorBuySetText;
    public Text m_trailBuySetText;
    public Text m_goldText;

    private int[] m_colorCost = new int[] { 0, 5, 5, 5, 10, 10, 10, 15, 15, 10 };
    private int[] m_trailCost = new int[] { 0, 20, 40, 40, 60, 60, 80, 80, 100, 100 };
    private int m_selectColorIndex;
    private int m_selectedTrailIndex;
    private int m_activeColorIndex;
    private int m_activeTrailIndex;
    private MenuCamera m_MenuCam; 

    private Vector3 m_desiredMenuPosition;
    private GameObject m_currentTrail; 

    public AnimationCurve m_enteringLevelZoomCurve;
    private bool m_isEnteringLevel = false;
    private float m_zoomDuration = 3f;
    private float m_zoomTransition;

    void Start()
    {
        // temp
        SaveManager.m_instance.m_state.m_gold = 999;
        UpdateGoldText();

        m_MenuCam = FindObjectOfType<MenuCamera>();
        //grab only canvas grp in scene
        m_fadeGroup = FindObjectOfType<CanvasGroup>();

        m_fadeGroup.alpha = 1f;

        InitShop();
        InitLevel();

        // Set playerPrefs for color and trail
        OnColorSelect(SaveManager.m_instance.m_state.m_activeColor);
        SetColor(SaveManager.m_instance.m_state.m_activeColor);

        OnTrailSelect(SaveManager.m_instance.m_state.m_activeTrail);
        SetTrail(SaveManager.m_instance.m_state.m_activeTrail);

        m_colorPanel.GetChild(SaveManager.m_instance.m_state.m_activeColor).GetComponent<RectTransform>().localScale = Vector3.one * 1.125f;
        m_trailPanel.GetChild(SaveManager.m_instance.m_state.m_activeTrail).GetComponent<RectTransform>().localScale = Vector3.one * 1.125f;

        SetCameraTo(Manager.Instance.m_menuFocus);
    }

    void Update()
    {
        m_fadeGroup.alpha = 1 - Time.timeSinceLevelLoad * m_fadeInSpeed;
        m_menuContainer.anchoredPosition3D = Vector3.Lerp(m_menuContainer.anchoredPosition3D, m_desiredMenuPosition, 0.1f);

        if(m_isEnteringLevel)
        {
            m_zoomTransition += (1 / m_zoomDuration) * Time.deltaTime;

            m_menuContainer.localScale = Vector3.Lerp(Vector3.one, Vector3.one * 5f, m_enteringLevelZoomCurve.Evaluate(m_zoomTransition));

            Vector3 newDesiredPosition = m_desiredMenuPosition * 5;
            RectTransform rt = m_levelPanel.GetChild(Manager.Instance.m_currentLevel).GetComponent<RectTransform>();
            newDesiredPosition -= rt.anchoredPosition3D * 5;

            m_menuContainer.anchoredPosition3D = Vector3.Lerp(m_desiredMenuPosition, newDesiredPosition, m_enteringLevelZoomCurve.Evaluate(m_zoomTransition));

            // fade
            m_fadeGroup.alpha = m_zoomTransition;

            if(m_zoomTransition >= 1)
            {
                SceneManager.LoadScene("Game");
            }
        }
    }

    private void InitShop()
    {
        // assign references
        if(!m_colorPanel || !m_trailPanel)
        {
            Debug.Log("You must assign the color/trail panels");
        }

        int i = 0;
        foreach(Transform t in m_colorPanel)
        {
            int currentIndex = i;
            Button b = t.GetComponent<Button>();
            b.onClick.AddListener(() => OnColorSelect(currentIndex));

            //set color of owned or not
            Image img = t.GetComponent<Image>();
            img.color = SaveManager.m_instance.IsColorOwned(i) ? Manager.Instance.m_playerColors[currentIndex] : Color.Lerp(Manager.Instance.m_playerColors[currentIndex], new Color(0, 0, 0, 1), .1f);
            i++;
        }

        i = 0;
        foreach(Transform t in m_trailPanel)
        {
            int currentIndex = i;
            Button b = t.GetComponent<Button>();
            b.onClick.AddListener(() => OnTrailSelect(currentIndex));

            //set trail of owned or not
            Image img = t.GetComponent<Image>();
            img.color = SaveManager.m_instance.IsTrailOwned(i) ? Manager.Instance.m_playerColors[currentIndex] : new Color(0.7f, 0.7f, 0.7f);

            i++;
        }
    }

    private void InitLevel()
    {
        // assign references
        if(!m_levelPanel)
        {
            Debug.Log("You must assign the level panels");
        }

        int i = 0;
        foreach(Transform t in m_levelPanel)
        {
            int currentIndex = i;
            Button b = t.GetComponent<Button>();
            b.onClick.AddListener(() => OnLevelSelect(currentIndex));

            Image img = t.GetComponent<Image>();
            //is lvl unlocked
            if(i <= SaveManager.m_instance.m_state.m_completedLevel)
            {
                //it is unlocked
                if(i == SaveManager.m_instance.m_state.m_completedLevel)
                {
                    // not completed 
                    img.color = Color.white;
                }
                else
                {
                    // lvl completed
                    img.color = Color.green;
                }

            }
            else
            {
                //level isnt unlocked, disable btn
                b.interactable = false;
                img.color = Color.grey;
            }

            i++;
        }
    }

    private void OnLevelSelect(int index)
    {
        Debug.Log("Selecting level {0}" + index);
        Manager.Instance.m_currentLevel = index;
        m_isEnteringLevel = true;
       // SceneManager.LoadScene("Game");
    }

    private void OnColorSelect(int index)
    {
        Debug.Log("Selecting color " + index);

        //Set the selected color
        if(m_selectColorIndex == index)
        {
            return;
        }

        // Make icon bigger
        m_colorPanel.GetChild(index).GetComponent<RectTransform>().localScale = Vector3.one * 1.125f;
        // put previous on normal scale
        m_colorPanel.GetChild(m_selectColorIndex).GetComponent<RectTransform>().localScale = Vector3.one;

        m_selectColorIndex = index;

        // change the context of the buy/set button
        if(SaveManager.m_instance.IsColorOwned(index))
        {
            if(m_activeColorIndex == index)
            {
                m_colorBuySetText.text = "Current";
            }
            else
            {
                m_colorBuySetText.text = "Select";
            }
        }
        else
        {
            m_colorBuySetText.text = "Buy $" + m_colorCost[index];
        }
    }

    private void OnTrailSelect(int index)
    {
        Debug.Log("Selecting trail " + index);

        //Set the selected color
        if(m_selectedTrailIndex == index)
        {
            return;
        }

        // Make icon bigger
        m_trailPanel.GetChild(index).GetComponent<RectTransform>().localScale = Vector3.one * 1.125f;
        // put previous on normal scale
        m_trailPanel.GetChild(m_selectedTrailIndex).GetComponent<RectTransform>().localScale = Vector3.one;

        //Set the selected trail
        m_selectedTrailIndex = index;

        // change the context of the buy/set button
        if(SaveManager.m_instance.IsTrailOwned(index))
        {
            if(m_activeTrailIndex == index)
            {
                m_trailBuySetText.text = "Current";
            }
            else
            {
                m_trailBuySetText.text = "Select";
            }
        }
        else
        {
            m_trailBuySetText.text = "Buy $" + m_trailCost[index];
        }
    }

    private void NavigateTo(int menuIndex)
    {
        switch(menuIndex)
        {
            default:
            case 0:
            m_desiredMenuPosition = Vector3.zero;
            m_MenuCam.BackToMainMenu();
            break;

            case 1:
            m_desiredMenuPosition = Vector3.right * 1280;
            m_MenuCam.MoveToLevel();
            break;

            case 2:
            m_desiredMenuPosition = Vector3.left * 1280;
            m_MenuCam.MoveToShop();
            break;
        }
    }

    private void SetCameraTo(int menuIndex)
    {
        NavigateTo(menuIndex);
        m_menuContainer.anchoredPosition3D = m_desiredMenuPosition;    
    }

    public void OnPlayClick()
    {
        NavigateTo(1);
        Debug.Log("Play Click");
    }

    public void OnShopClick()
    {
        NavigateTo(2);
        Debug.Log("Shop clicked");
    }

    public void OnColorBuySet()
    {
        Debug.Log("Buy Color Set");

        // is the color owned? 
        if(SaveManager.m_instance.IsColorOwned(m_selectColorIndex))
        {
            SetColor(m_selectColorIndex);
        }
        else
        {
            // attempt to buy color
            if(SaveManager.m_instance.BuyColor(m_selectColorIndex, m_colorCost[m_selectColorIndex]))
            {
                //success
                SetColor(m_selectColorIndex);

                m_colorPanel.GetChild(m_selectColorIndex).GetComponent<Image>().color = Manager.Instance.m_playerColors[m_selectColorIndex];

                UpdateGoldText();
            }
            else
            {
                Debug.Log("Not Enough Gold");
            }
        }
    }

    public void OnTrailBuySet()
    {
        Debug.Log("Buy Trail Set");

        // is the color owned? 
        if(SaveManager.m_instance.IsTrailOwned(m_selectedTrailIndex))
        {
            SetTrail(m_selectedTrailIndex);
        }
        else
        {
            // attempt to buy color
            if(SaveManager.m_instance.BuyTrail(m_selectedTrailIndex, m_trailCost[m_selectedTrailIndex]))
            {
                //success
                SetTrail(m_selectedTrailIndex);

                m_trailPanel.GetChild(m_selectedTrailIndex).GetComponent<Image>().color = Color.white;

                UpdateGoldText();
            }
            else
            {
                Debug.Log("Not Enough Gold");
            }
        }
    }

    public void OnBackClick()
    {
        NavigateTo(0);
        Debug.Log("On back click clicked");
    }

    private void SetColor(int index)
    {
        m_colorBuySetText.text = "Current";
        SaveManager.m_instance.m_state.m_activeColor = index;

        Manager.Instance.m_playerMaterial.color = Manager.Instance.m_playerColors[index];
        m_activeColorIndex = index;

        SaveManager.m_instance.Save();
    }

    private void SetTrail(int index)
    {
        m_trailBuySetText.text = "Current";
        SaveManager.m_instance.m_state.m_activeTrail = index;

        if(m_currentTrail != null)
        {
            Destroy(m_currentTrail);
        }

        m_currentTrail = Instantiate(Manager.Instance.m_playerTrails[index]) as GameObject;
        m_currentTrail.transform.SetParent(FindObjectOfType<MenuPlayer>().transform);

        m_currentTrail.transform.localPosition = Vector3.zero;
        m_currentTrail.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
        m_currentTrail.transform.localScale = Vector3.one * 0.01f; 

        m_activeTrailIndex = index;

        SaveManager.m_instance.Save();
    }

    private void UpdateGoldText()
    {
        m_goldText.text = SaveManager.m_instance.m_state.m_gold.ToString();
    }



}
