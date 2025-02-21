using UnityEngine;

public static class ServiceLocator
{
    public static GameManager GameManager { get; private set; }
    public static BoxManager BoxManager { get; private set; }
    public static UIManager UIManager { get; private set; }

    public static void Register(GameManager gameManager, BoxManager boxManager, UIManager uiManager)
    {
        GameManager = gameManager;
        BoxManager = boxManager;
        UIManager = uiManager;

        Debug.Log("✅ ServiceLocator: Registered UIManager = " + (UIManager != null));
    }
}
