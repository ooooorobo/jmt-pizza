// 게임 매니저를 도와 게임 클리어, 오버 등을 체크하는 클래스
// 이름 짓기 힘들다

using UnityEngine;

public abstract class GameChecker: MonoBehaviour
{
    protected IGameManager gameManager;
    protected StageLoader stageLoader;
    public IDefaultSpawnerStrategy spawnerFactory;
    
    public GameObject clearPanelPrefab;
    public Transform clearPanel;
    
    public Transform canvas;
    
    public abstract void InitGame();
    public abstract void CheckGameClear();
    public abstract void CheckGameOver();
    public abstract void SetGameClearPanelUi();
    public abstract void ChangeGauge(Topping t);
}