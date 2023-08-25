using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using UnityEngine;

public static class TrocaTela
{
    public static void TrocarParaMemoria()
    {
        var start = SceneManager.GetActiveScene();
        var sceneLoad = SceneManager.LoadSceneAsync("CenaMemoria");
        sceneLoad.allowSceneActivation = true;
        SceneManager.UnloadSceneAsync(start);
    }

    public static void TrocarParaRaciocinio()
    {
        var start = SceneManager.GetActiveScene();
        var sceneLoad = SceneManager.LoadSceneAsync("CenaRacioc");
        sceneLoad.allowSceneActivation = true;
        SceneManager.UnloadSceneAsync(start);
    }

    public static void TrocarParaMemoriaAud()
    {
        var start = SceneManager.GetActiveScene();
        var sceneLoad = SceneManager.LoadSceneAsync("CenaMemAud");
        sceneLoad.allowSceneActivation = true;
        SceneManager.UnloadSceneAsync(start);
    }

    public static void TrocarParaTemasMem()
    {
        var start = SceneManager.GetActiveScene();
        var sceneLoad = SceneManager.LoadSceneAsync("CenaTemasMem");
        sceneLoad.allowSceneActivation = true;
        SceneManager.UnloadSceneAsync(start);
    }

    public static void TrocarParaRegrasMem()
    {
        var start = SceneManager.GetActiveScene();
        var sceneLoad = SceneManager.LoadSceneAsync("CenaRegrasMemoria");
        sceneLoad.allowSceneActivation = true;
        SceneManager.UnloadSceneAsync(start);
    }

    public static void TrocarParaTelaInicial()
    {
        var start = SceneManager.GetActiveScene();
        var sceneLoad = SceneManager.LoadSceneAsync("CenaBuild");
        sceneLoad.allowSceneActivation = true;
        SceneManager.UnloadSceneAsync(start);
    }

    public static void TrocarParaTemasMemAud()
    {
        var start = SceneManager.GetActiveScene();
        var sceneLoad = SceneManager.LoadSceneAsync("CenaTemasMemAud");
        sceneLoad.allowSceneActivation = true;
        SceneManager.UnloadSceneAsync(start);
    }

    public static void TrocarParaRegrasMemAud()
    {
        var start = SceneManager.GetActiveScene();
        var sceneLoad = SceneManager.LoadSceneAsync("CenaRegrasMemAud");
        sceneLoad.allowSceneActivation = true;
        SceneManager.UnloadSceneAsync(start);
    }

    public static void TrocarParaCenaSudoku()
    {
        var start = SceneManager.GetActiveScene();
        var sceneLoad = SceneManager.LoadSceneAsync("CenaSudoku");
        sceneLoad.allowSceneActivation = true;
        SceneManager.UnloadSceneAsync(start);
    }

    public static void TrocarParaRegrasSudoku()
    {
        var start = SceneManager.GetActiveScene();
        var sceneLoad = SceneManager.LoadSceneAsync("CenaRegrasSudoku");
        sceneLoad.allowSceneActivation = true;
        SceneManager.UnloadSceneAsync(start);
    }

    public static void TrocarParaJogoSudoku()
    {
        var start = SceneManager.GetActiveScene();
        var sceneLoad = SceneManager.LoadSceneAsync("Template");
        sceneLoad.allowSceneActivation = true;
        SceneManager.UnloadSceneAsync(start);
    }

    public static void TrocarParaCenaJogoDaVelha()
    {
        var start = SceneManager.GetActiveScene();
        var sceneLoad = SceneManager.LoadSceneAsync("CenaJogodaVelha");
        sceneLoad.allowSceneActivation = true;
        SceneManager.UnloadSceneAsync(start);
    }

    public static void TrocarParaRegrasJogoDaVelha()
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