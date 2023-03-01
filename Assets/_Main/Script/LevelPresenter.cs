
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
using MoNo.Utility;

public class LevelPresenter : SingletonMonoBehaviour<LevelPresenter>
{
    protected override bool DontDestroy { get; } = false;
    private LevelProgressStateReactiveProperty _levelProgressState;
    private Counter _goalCounter =  new(); 
    
    [SerializeField] private int _goalCount = 3;
    
    public LevelProgressStateReactiveProperty LevelProgressState => _levelProgressState;
    public Counter GoalCounter => _goalCounter;
    public int GoalCount => _goalCount;

    protected override void Awake()
    {
        base.Awake();
        _levelProgressState = new LevelProgressStateReactiveProperty(StateType.StartView);
        _levelProgressState.AddTransition(StateType.StartView, StateType.Ingame, TriggerType.ToIngame);
        _levelProgressState.AddTransition(StateType.Ingame, StateType.Success, TriggerType.ToSuccess);
        _levelProgressState.AddTransition(StateType.Ingame, StateType.Fail, TriggerType.ToFail);
        _levelProgressState.AddTransition(StateType.Success, StateType.Result, TriggerType.ToResult);
        _levelProgressState.AddTransition(StateType.Fail, StateType.Result, TriggerType.ToResult);
        
        _goalCounter.Count
            .Where(count => count >= _goalCount)
            .Subscribe(_ => _levelProgressState.ExecuteTrigger(TriggerType.ToSuccess)).AddTo(this);
        
        
        TinySauce.OnGameStarted($"Scene {SceneManager.GetActiveScene().buildIndex}");
        _levelProgressState.Where(state => state == StateType.Success)
            .Subscribe(_ =>
            {
                TinySauce.OnGameFinished(true, _goalCounter.GetCount(),
                    $"Scene {SceneManager.GetActiveScene().buildIndex}");

            });
        _levelProgressState.Where(state => state == StateType.Fail)
            .Subscribe(_ =>
            {
                TinySauce.OnGameFinished(false, _goalCounter.GetCount(),
                    $"Scene {SceneManager.GetActiveScene().buildIndex}");
            });

    }
    
    public void GameStart()
    {
        _levelProgressState.ExecuteTrigger(TriggerType.ToIngame);
    }

    public void BackScene()
    {
        int prevScene;
        if (1 > SceneManager.GetActiveScene().buildIndex - 1)
        {
            prevScene = SceneManager.sceneCountInBuildSettings - 1; // last one
        }
        else
        {
            prevScene = SceneManager.GetActiveScene().buildIndex - 1;
        }
        SceneManager.LoadScene(prevScene);
    }
        
    public void RestartScene()
    {
        var currentScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentScene);
    }
        
    public void NextScene()
    {
        int nextScene;
        if (SceneManager.sceneCountInBuildSettings - 1 > SceneManager.GetActiveScene().buildIndex)
        {
            nextScene = SceneManager.GetActiveScene().buildIndex + 1;
        }
        else
        {
            nextScene = 1;
        }
        SceneManager.LoadScene(nextScene);
    }

    private void OnDestroy()
    {
        _levelProgressState.Dispose();
    }
}
