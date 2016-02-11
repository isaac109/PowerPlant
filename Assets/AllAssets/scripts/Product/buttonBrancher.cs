using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class buttonBrancher : MonoBehaviour {
    [System.Serializable]
    public class ButtonScaler
    {
        enum ScaleMode
        {
            MATCH_WIDTH_HEIGHT,
            INDEPENDENT_WIDTH_HEIGHT
        }
        ScaleMode mode;
        Vector2 referenceButtonSize;

        [HideInInspector]
        public Vector2 referenceScreenSize;
        public Vector2 newButtonSize;

        public void Initialize(Vector2 refButtonSize, Vector2 refScreenSize, int scaleMode)
        {
            mode = (ScaleMode)scaleMode;
            referenceButtonSize = refButtonSize;
            referenceScreenSize = refScreenSize;
            setNewButtonSize();
        }

        void setNewButtonSize()
        {
            if (mode == ScaleMode.INDEPENDENT_WIDTH_HEIGHT)
            {
                newButtonSize.x = (referenceButtonSize.x * Screen.width) / referenceScreenSize.x;
                newButtonSize.y = (referenceButtonSize.y * Screen.height) / referenceScreenSize.y;
            }
            else if (mode == ScaleMode.MATCH_WIDTH_HEIGHT)
            {
                newButtonSize.x = (referenceButtonSize.x * Screen.width) / referenceScreenSize.x;
                newButtonSize.y = newButtonSize.x;
            }
        }
    }
    [System.Serializable]
    public class RevealSettings
    {
        public enum RevealOption
        {
            LINEAR,
            CIRCULAR
        }
        public RevealOption option;
        public float translateSmooth = 5f;
        public float fadeSmooth = 0.01f;
        public bool revealOnStart = false;

        [HideInInspector]
        public bool opening = false;
        [HideInInspector]
        public bool spawned = false;
    }
    [System.Serializable]
    public class LinearSpawner
    {
        public enum RevealStyle
        {
            SLIDE,
            FADE
        }
        public RevealStyle revealStyle;
        public Vector2 direction = new Vector2(0, 1);
        public float baseButtonSpacing = 5f;
        public int buttonNumOffset = 0;

        [HideInInspector]
        public float buttonSpacing = 5f;

        public void FitSpacingToScreenSize(Vector2 refScreenSize)
        {
            float refScreenFloat = (refScreenSize.x + refScreenSize.y) / 2;
            float screenFloat = (Screen.width + Screen.height) / 2;
            buttonSpacing = (baseButtonSpacing * screenFloat) / refScreenFloat;
        }
    }

    public class circularSpawner
    {
    }

    public GameObject[] buttonRefs;

    public List<GameObject> buttons;

    public enum ScaleMode
    {
        MATCH_WIDTH_HEIGHT,
        INDEPENDENT_WIDTH_HEIGHT
    }
    public ScaleMode mode;
    public Vector2 referenceButtonSize;
    public Vector2 referenceScreenSize;

    ButtonScaler buttonScaler = new ButtonScaler();
    public RevealSettings revealSettings = new RevealSettings();
    public LinearSpawner linSpawner = new LinearSpawner();

    float lastScreenWidth = 0;
    float lastScreenHeight = 0;
	// Use this for initialization
	void Start () {
        buttons = new List<GameObject>();
        buttonScaler = new ButtonScaler();
        lastScreenWidth = Screen.width;
        lastScreenHeight = Screen.height;
        buttonScaler.Initialize(referenceButtonSize, referenceScreenSize, (int)mode);
        linSpawner.FitSpacingToScreenSize(buttonScaler.referenceScreenSize);
        if (revealSettings.revealOnStart)
        {
            spawnButtons();
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (Screen.width != lastScreenWidth || Screen.height != lastScreenHeight)
        {
            lastScreenWidth = Screen.width;
            lastScreenHeight = Screen.height;
            buttonScaler.Initialize(referenceButtonSize, referenceScreenSize, (int)mode);
            linSpawner.FitSpacingToScreenSize(buttonScaler.referenceScreenSize);
            spawnButtons();
        }
        if (revealSettings.opening)
        {
            if (!revealSettings.spawned)
            {
                spawnButtons();
            }
            switch (revealSettings.option)
            {
                case RevealSettings.RevealOption.LINEAR:
                    switch (linSpawner.revealStyle)
                    {
                        case LinearSpawner.RevealStyle.SLIDE:
                            revealLinearyNormal();
                            break;
                        case LinearSpawner.RevealStyle.FADE:
                            revealLinearlyFade();
                            break;
                    }
                    break;
            }
        }
        for (int i = 0; i < buttons.Count; i++)
        {
            Debug.Log(buttons[i].GetComponent<RectTransform>().transform.rotation.ToString());
        }
	}
    public void revealLinearyNormal()
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            Vector3 targetPos;
            RectTransform buttonRect = buttons[i].GetComponent<RectTransform>();
            buttonRect.sizeDelta = new Vector2(buttonScaler.newButtonSize.x, buttonScaler.newButtonSize.y);
            targetPos.x = linSpawner.direction.x * ((i + linSpawner.buttonNumOffset) * (buttonRect.sizeDelta.x + linSpawner.buttonSpacing)) + transform.position.x;
            targetPos.y = linSpawner.direction.y * ((i + linSpawner.buttonNumOffset) * (buttonRect.sizeDelta.y + linSpawner.buttonSpacing)) + transform.position.y;
            targetPos.z = 0;
            buttonRect.position = Vector3.Lerp(buttonRect.position, targetPos, revealSettings.translateSmooth * Time.deltaTime);
            Quaternion rotation = Quaternion.identity;
            rotation.eulerAngles = new Vector3(0,0,0);
            buttonRect.transform.rotation = rotation;
        }
    }
    public void revealLinearlyFade()
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            Vector3 targetPos;
            RectTransform buttonRect = buttons[i].GetComponent<RectTransform>();
            buttonRect.sizeDelta = new Vector2(buttonScaler.newButtonSize.x, buttonScaler.newButtonSize.y);
            targetPos.x = 0;//linSpawner.direction.x * ((i + linSpawner.buttonNumOffset) * (buttonRect.sizeDelta.x + linSpawner.buttonSpacing)) + transform.position.x;
            targetPos.y = 0;// linSpawner.direction.y * ((i + linSpawner.buttonNumOffset) * (buttonRect.sizeDelta.y + linSpawner.buttonSpacing)) + transform.position.y;
            targetPos.z = 0;

            buttonFader previousButtonFader;
            if (i > 0)
            {
                previousButtonFader = buttons[i - 1].GetComponent<buttonFader>();
            }
            else
            {
                previousButtonFader = null;
            }
            buttonFader bF = buttons[i].GetComponent<buttonFader>();

            if (previousButtonFader)
            {
                if (previousButtonFader.faded)
                {
                    buttons[i].transform.position = targetPos;
                    if (bF)
                    {
                        bF.fade(revealSettings.fadeSmooth);
                    }
                    else
                    {
                        Debug.LogError("No ButtonFader script");
                    }
                }
            }
            else
            {
                buttons[i].transform.position = targetPos;
                if (bF)
                {
                    bF.fade(revealSettings.fadeSmooth);
                }
                else
                {
                    Debug.LogError("No ButtonFadeScript");
                }
            }
        }
    }
    public void spawnButtons()
    {
        revealSettings.opening = true;
        revealSettings.spawned = true;
        for (int i = buttons.Count - 1; i >= 0; i--)
        {
            Destroy(buttons[i]);
        }
        buttons.Clear();

        clearCommonButtonBranchers();

        for (int i = 0; i < buttonRefs.Length; i++)
        {
            GameObject b = Instantiate(buttonRefs[i] as GameObject);
            b.transform.SetParent(transform);
            b.transform.position = transform.position;
            b.transform.localScale = new Vector3(1, 1, 1);
            if (linSpawner.revealStyle == LinearSpawner.RevealStyle.FADE)
            {
                Color c = b.GetComponent<Image>().color;
                c.a = 0;
                b.GetComponent<Image>().color = c;
                if (b.GetComponentInChildren<Text>())
                {
                    c = b.GetComponentInChildren<Text>().color;
                    c.a = 0;
                    b.GetComponentInChildren<Text>().color = c;
                }
                buttons.Add(b);
            }
        }
    }
    void clearCommonButtonBranchers()
    {
        GameObject[] branchers = GameObject.FindGameObjectsWithTag("ButtonBrancher");
        foreach (GameObject brancher in branchers)
        {
            if (brancher.transform.parent == transform.parent)
            {
                buttonBrancher bb = brancher.GetComponent<buttonBrancher>();
                for (int i = bb.buttons.Count-1; i >= 0; i--)
                {
                    Destroy(bb.buttons[i]);
                }
                bb.buttons.Clear();
            }
        }
    }
}
