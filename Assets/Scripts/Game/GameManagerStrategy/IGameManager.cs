using UnityEngine;

interface IGameManager
{
    // 게임 기본 세팅 - 타일/스폰/플레이어/조이스틱 등 준비
    void InitGame();

    // O/X 토핑 획득 시 - 점수 변경
    void ChangeScore(int score);

    // 코인 획득 시 - 플레이어 코인 변경
    void GetCoin(int coin);
    
    // 게임 일시정지 시
    void StopGame();

    // 게임 재개 시
    void ResumeGame();
    
    // 오븐 오픈 조건 확인
    void CheckGameClear();

    // 게임 클리어 후 로직
    void GameClear();

    // 게임 오버 후 로직
    void GameOver();
}