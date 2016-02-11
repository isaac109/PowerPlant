using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class mainMenuHandler : MonoBehaviour {

    public GameObject[] menus = new GameObject[7];

    public void newGame()
    {
        closeMenus();
        menus[1].SetActive(true);
    }

    public void startNewGame()
    {
        SceneManager.LoadScene("map");
    }

    public void loadGame()
    {
        closeMenus();
        menus[2].SetActive(true);
    }

    public void loadSelectedGame()
    {
    }

    public void makeAMap()
    {
        closeMenus();
        menus[3].SetActive(true);
    }

    public void makeMapMenu()
    {
        SceneManager.LoadScene("mapMaker");
    }

    public void tutorial()
    {
        closeMenus();
        menus[4].SetActive(true);
    }

    public void tutorialMenu(int i)
    {

    }

    public void options()
    {
        closeMenus();
        menus[5].SetActive(true);
    }

    public void optionsMenu()
    {
    }

    public void exit()
    {
        closeMenus();
        menus[6].SetActive(true);
    }

    public void exitMenu()
    {

    }

    public void openMain()
    {
        closeMenus();
        menus[0].SetActive(true);
    }
    public void closeMenus()
    {
        for (int i = 0; i < menus.Length; i++)
        {
            menus[i].SetActive(false);
        }
    }
}
