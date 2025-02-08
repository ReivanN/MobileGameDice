using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void Restart() 
    {
        SceneManager.LoadScene("Arena");
    }
}
