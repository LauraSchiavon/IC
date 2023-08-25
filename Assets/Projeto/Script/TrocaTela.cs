using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using UnityEngine;

public static class TrocaTela
{
    public static void trocarParaMemoria()
    {
        var start = SceneManager.GetActiveScene();
        var sceneLoad = SceneManager.LoadSceneAsync("CenaMemoria");
        sceneLoad.allowSceneActivation = true;
        SceneManager.UnloadSceneAsync(start);
    }

    public static void trocarParaRaciocinio()
    {
        var start = SceneManager.GetActiveScene();
        var sceneLoad = SceneManager.LoadSceneAsync("CenaRacioc");
        sceneLoad.allowSceneActivation = true;
        SceneManager.UnloadSceneAsync(start);
    }

    public static void trocarParaMemoriaAud()
    {
        var start = SceneManager.GetActiveScene();
        var sceneLoad = SceneManager.LoadSceneAsync("CenaMemAud");
        sceneLoad.allowSceneActivation = true;
        SceneManager.UnloadSceneAsync(start);
    }

    public static void trocarParaTemasMem()
    {
        var start = SceneManager.GetActiveScene();
        var sceneLoad = SceneManager.LoadSceneAsync("CenaTemasMem");
        sceneLoad.allowSceneActivation = true;
        SceneManager.UnloadSceneAsync(start);
    }

    public static void trocarParaRegrasMem()
    {
        var start = SceneManager.GetActiveScene();
        var sceneLoad = SceneManager.LoadSceneAsync("CenaRegrasMemoria");
        sceneLoad.allowSceneActivation = true;
        SceneManager.UnloadSceneAsync(start);
    }

    public static void trocarParaTelaInicial()
    {
        var start = SceneManager.GetActiveScene();
        var sceneLoad = SceneManager.LoadSceneAsync("CenaBuild");
        sceneLoad.allowSceneActivation = true;
        SceneManager.UnloadSceneAsync(start);
    }

    public static void trocarParaTemasMemAud()
    {
        var start = SceneManager.GetActiveScene();
        var sceneLoad = SceneManager.LoadSceneAsync("CenaTemasMemAud");
        sceneLoad.allowSceneActivation = true;
        SceneManager.UnloadSceneAsync(start);
    }

    public static void trocarParaRegrasMemAud()
    {
        var start = SceneManager.GetActiveScene();
        var sceneLoad = SceneManager.LoadSceneAsync("CenaRegrasMemAud");
        sceneLoad.allowSceneActivation = true;
        SceneManager.UnloadSceneAsync(start);
    }

    public static void trocarParaCenaSudoku()
    {
        var start = SceneManager.GetActiveScene();
        var sceneLoad = SceneManager.LoadSceneAsync("CenaSudoku");
        sceneLoad.allowSceneActivation = true;
        SceneManager.UnloadSceneAsync(start);
    }

    public static void trocarParaRegrasSudoku()
    {
        var start = SceneManager.GetActiveScene();
        var sceneLoad = SceneManager.LoadSceneAsync("CenaRegrasSudoku");
        sceneLoad.allowSceneActivation = true;
        SceneManager.UnloadSceneAsync(start);
    }

    public static void trocarParaJogoSudoku()
    {
        var start = SceneManager.GetActiveScene();
        var sceneLoad = SceneManager.LoadSceneAsync("Template");
        sceneLoad.allowSceneActivation = true;
        SceneManager.UnloadSceneAsync(start);
    }

    public static void trocarParaCenaJogoDaVelha()
    {
        var start = SceneManager.GetActiveScene();
        var sceneLoad = SceneManager.LoadSceneAsync("CenaJogodaVelha");
        sceneLoad.allowSceneActivation = true;
        SceneManager.UnloadSceneAsync(start);
    }

    public static void trocarParaRegrasJogoDaVelha()
    {
        var start = SceneManager.GetActiveScene();
        var sceneLoad = SceneManager.LoadSceneAsync("CenaRegrasJogodaVelha");
        sceneLoad.allowSceneActivation = true;
        SceneManager.UnloadSceneAsync(start);
    }

    public static void trocarParaJogoJogoDaVelha()
    {
        var start = SceneManager.GetActiveScene();
        var sceneLoad = SceneManager.LoadSceneAsync("Board");
        sceneLoad.allowSceneActivation = true;
        SceneManager.UnloadSceneAsync(start);
    }
}