using UnityEngine;

public class Environment
{
    /* For Backend */
    public const string InfiniteRankUuid = "b0a52b30-235c-11eb-87fa-6f54f412f5f8";
    public const string InfiniteTableName = "infiniteModeScoreTable";
    
    /* 게임 공통 */
    // ⬇️⬇️⬇️⬇️ 이 방향 줄 개수
    public const int BoardColumnCount = 13;
    // ➡️➡️➡️➡️ 이 방향 줄 개수
    public const int BoardRowCount = 17;


    /* 무한 모드 */
    
    // 초기 점수
    public const int InfiniteInitialScore = 1000;

    // 타겟 토핑의 토핑 ID
    public const int InfiniteTargetToppingId = 0;
    
    // 타겟 토핑(치즈)의 클리어 최소 조건
    public const int InfiniteTargetToppingGoalMin = 4;

    public const int InfiniteToppingTotalCount = 10;

    // 플레이어 최초 시작 속도 - 한 칸 움직이는 데 걸리는 시간(초) 
    public const float InfinitePlayerInitialSpeed = 0.2f;
    
    // 토핑 하나를 먹을 때마다, 플레이어 속도가 빨라지는 정도 - 원래 속도에서 단순 빼기 연산
    public const float InfinitePlayerAccelerateSpeed = 0.02f;

    // O 토핑 획득 시 점수 변화량
    public const int InfiniteOToppingScore = 400;
    
    // X 토핑 획득 시 점수 변화량
    public const int InfiniteXToppingScore = -200;

    // X 토핑 총 개수
    public const int InfiniteXToppingCount = 8;
    
    // 토핑 스폰 딜레이 (하나의 토핑이 생성된 후, n초 뒤에 다음 토핑 생성)
    public const float InfiniteToppingSpawnDelay = 2f;
    
    // 토핑 표시 시간 (토핑이 생성된 후, n초 뒤에 사라짐)
    public const float InfiniteToppingDestroyDelay = 3f;
    
    // 게임 오버 한도 점수 (이 점수 이하면 게임 오버 됩니다)
    public const int InfiniteScoreMinimum = 0;

    public static Color ColorOToppingScore()
    {
        return new Color(28, 129, 204);
    }    
    public static Color ColorXToppingScore()
    {
        return new Color(255, 99, 0);
    } 
    
    
    /* 이 아래는 수정하지 말 것 !! */
    public static readonly string[] ToppingNameList = {
        "치즈", "파인애플", "불고기", "버섯", "올리브", "양파", "피망", "새우", "베이컨", "옥수수"
    };

    // Enum
    public enum GameMode
    {
        LOBBY,
        INFINITE,
        STORY,
        MULTI
    }

    public enum StageMode
    {
        ORIGINAL,
        AVOID,
        CLEAN_DUST
    }
}
