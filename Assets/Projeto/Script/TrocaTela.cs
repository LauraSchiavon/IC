using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using UnityEngine;

public class TrocaTela : MonoBehaviour
{
    public void trocarParaMemoria()
    {
        var start = SceneManager.GetActiveScene();
        var sceneLoad = SceneManager.LoadSceneAsync("CenaMemoria");
        sceneLoad.allowSceneActivation = true;
        SceneManager.UnloadSceneAsync(start);
    }

    public void trocarParaRaciocinio()
    {
        var start = SceneManager.GetActiveScene();
        var sceneLoad = SceneManager.LoadSceneAsync("CenaRacioc");
        sceneLoad.allowSceneActivation = true;
        SceneManager.UnloadSceneAsync(start);
    }

    public void trocarParaMemoriaAud()
    {
        var start = SceneManager.GetActiveScene();
        var sceneLoad = SceneManager.LoadSceneAsync("CenaMemAud");
        sceneLoad.allowSceneActivation = true;
        SceneManager.UnloadSceneAsync(start);
    }

    public void trocarParaTemasMem()
    {
        var start = SceneManager.GetActiveScene();
        var sceneLoad = SceneManager.LoadSceneAsync("CenaTemasMem");
        sceneLoad.allowSceneActivation = true;
        SceneManager.UnloadSceneAsync(start);
    }

    public void trocarParaRegrasMem()
    {
        var start = SceneManager.GetActiveScene();
        var sceneLoad = SceneManager.LoadSceneAsync("CenaRegrasMemoria");
        sceneLoad.allowSceneActivation = true;
        SceneManager.UnloadSceneAsync(start);
    }

    public void trocarParaTelaInicial()
    {
        var start = SceneManager.GetActiveScene();
        var sceneLoad = SceneManager.LoadSceneAsync("CenaBuild");
        sceneLoad.allowSceneActivation = true;
        SceneManager.UnloadSceneAsync(start);
    }

    public void trocarParaTemasMemAud()
    {
        var start = SceneManager.GetActiveScene();
        var sceneLoad = SceneManager.LoadSceneAsync("CenaTemasMemAud");
        sceneLoad.allowSceneActivation = true;
        SceneManager.UnloadSceneAsync(start);
    }

    public void trocarParaRegrasMemAud()
    {
        var start = SceneManager.GetActiveScene();
        var sceneLoad = SceneManager.LoadSceneAsync("CenaRegrasMemAud");
        sceneLoad.allowSceneActivation = true;
        SceneManager.UnloadSceneAsync(start);
    }

    public void trocarParaCenaSudoku()
    {
        var start = SceneManager.GetActiveScene();
        var sceneLoad = SceneManager.LoadSceneAsync("CenaSudoku");
        sceneLoad.allowSceneActivation = true;
        SceneManager.UnloadSceneAsync(start);
    }

    public void trocarParaRegrasSudoku()
    {
        var start = SceneManager.GetActiveScene();
        var sceneLoad = SceneManager.LoadSceneAsync("CenaRegrasSudoku");
        sceneLoad.allowSceneActivation = true;
        SceneManager.UnloadSceneAsync(start);
    }

    public void trocarParaJogoSudoku()
    {
        var start = SceneManager.GetActiveScene();
        var sceneLoad = SceneManager.LoadSceneAsync("Template");
        sceneLoad.allowSceneActivation = true;
        SceneManager.UnloadSceneAsync(start);
    }

    public void trocarParaCenaJogoDaVelha()
    {
        var start = SceneManager.GetActiveScene();
        var sceneLoad = SceneManager.LoadSceneAsync("CenaJogodaVelha");
        sceneLoad.allowSceneActivation = true;
        SceneManager.UnloadSceneAsync(start);
    }

    public void trocarParaRegrasJogoDaVelha()
    {
        var start = SceneManager.GetActiveScene();
        var sceneLoad = SceneManager.LoadSceneAsync("CenaRegrasJogodaVelha");
        sceneLoad.allowSceneActivation = true;
        SceneManager.UnloadSceneAsync(start);
    }

    public void trocarParaJogoJogoDaVelha()
    {
        var start = SceneManager.GetActiveScene();
        var sceneLoad = SceneManager.LoadSceneAsync("MainMenu");
        sceneLoad.allowSceneActivation = true;
        SceneManager.UnloadSceneAsync(start);
    }
    
    public void trocarParaCenaCampoMinado()
    {
        var start = SceneManager.GetActiveScene();
        var sceneLoad = SceneManager.LoadSceneAsync("CenaCampoMinado");
        sceneLoad.allowSceneActivation = true;
        SceneManager.UnloadSceneAsync(start);
    }
    
    public void trocarParaRegrasCampoMinado()
    {
        var start = SceneManager.GetActiveScene();
        var sceneLoad = SceneManager.LoadSceneAsync("CenaRegrasCampMin");
        sceneLoad.allowSceneActivation = true;
        SceneManager.UnloadSceneAsync(start);
    }
    
    public void trocarParaJogoCampoMinado()
    {
        var start = SceneManager.GetActiveScene();
        var sceneLoad = SceneManager.LoadSceneAsync("Hexagonal Minesweeper Vertical");
        sceneLoad.allowSceneActivation = true;
        SceneManager.UnloadSceneAsync(start);
    }
}