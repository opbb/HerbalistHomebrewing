using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UIController : MonoBehaviour
{
    // The one instance of this class
    public static UIController Instance;

    // IGUIScreens to manage
    private PlayerInventory playerInventory;
    private QuestManager questManager;
    private RecipeManager recipeManager;

    // Input keys
    [SerializeField] private KeyCode closeUIKey = KeyCode.Escape;
    [SerializeField] private KeyCode inventoryKey = KeyCode.E;
    [SerializeField] private KeyCode recipeKey = KeyCode.T;

    // Tells if any GUI screen is currently active
    private bool isAnyGUIActive { get => activeScreen != null; }
    // Remember the current GUI Screen which is active
    private IGUIScreen activeScreen;


    // This awake method enforces the singleton design pattern.
    // i.e. there can only ever be one UIController
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(this);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        playerInventory = PlayerInventory.Instance;
        questManager = GetComponent<QuestManager>();
        recipeManager = GetComponent<RecipeManager>();
        if (playerInventory == null || questManager == null || recipeManager == null)
        {
            throw new InvalidOperationException("The UI Controller cant find all of the GUIScreens, " +
                "they are probably not attached to the same GameObject, which they should be.");
        }
        activeScreen = null;
    }

    // Update is called once per frame
    void Update()
    {
        if(isAnyGUIActive)
        {
            // If a UI is open, check if we should close it
            if(Input.GetKeyDown(closeUIKey))
            {
                // IF escape is pressed, close the UI no matter what
                DeactivatGUIScreen();
            } else if(playerInventory.isGUIActive() && Input.GetKeyDown(inventoryKey))
            {
                // If the inventory key is pressed while it is open, close it
                DeactivatePlayerInventory();
            }
        } else
        {
            if (Input.GetKeyDown(recipeKey) && InRange.isInRange)
            {
                ActivateRecipeManager();
            }
            else if (Input.GetKeyDown(inventoryKey))
            {
                ActivatePlayerInventory();
            }
        } 
    }

    // ======= UI Specific Helpers =======

    // Activates the QuestManager GUI if it is safe to do so, returning true if it does.
    public bool ActivateQuestManager()
    {
        return ActivateGUIScreen(questManager);
    }

    // Deactivates the QuestManager if it is the active screen.
    public void DeactivateQuestManager()
    {
        if(ReferenceEquals(activeScreen, questManager))
        {
            DeactivatGUIScreen();
        }
    }

    // Activates the RecipeManager GUI if it is safe to do so, returning true if it does.
    public bool ActivateRecipeManager()
    {
        return ActivateGUIScreen(recipeManager);
    }

    // Deactivates the RecipeManager if it is the active screen.
    public void DeactivateRecipeManager()
    {
        if (ReferenceEquals(activeScreen, recipeManager))
        {
            DeactivatGUIScreen();
        }
    }

    // Activates the PlayerInventory GUI if it is safe to do so, returning true if it does.
    public bool ActivatePlayerInventory()
    {
        return ActivateGUIScreen(playerInventory);
    }

    // Deactivates the PlayerInventory if it is the active screen.
    public void DeactivatePlayerInventory()
    {
        if (ReferenceEquals(activeScreen, playerInventory))
        {
            DeactivatGUIScreen();
        }
    }




    // ======= Generic Helpers =======


    private bool ActivateGUIScreen(IGUIScreen screenType)
    {
        // Ensure other GUIs aren't active
        if (isAnyGUIActive)
        {
            // If this GUIScreen is already active, return true, otherwise return false
            return ReferenceEquals(screenType, activeScreen);
        }

        // Set the new active screen
        activeScreen = screenType;

        // Set up generic UI stuff
        EnterUIMode();

        // Activate the UI
        activeScreen.activateGUI();

        // Return succeess.
        return true;
    }

    private void DeactivatGUIScreen()
    {
        activeScreen.deactivateGUI();
        activeScreen = null;
        ExitUIMode();
    }


    // Does the things that always acompany UI (activate mouse, deactivate movement, etc)
    private void EnterUIMode()
    {
        MouseLook.isUIActive = true;
        PlayerController.isUIActive = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    // Undoes the things that always acompany UI (deactivate mouse, activate movement, etc)
    private void ExitUIMode()
    {
        MouseLook.isUIActive = false;
        PlayerController.isUIActive = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
