using UnityEngine;

// 발판을 생성하고 주기적으로 재배치하는 스크립트
public class PlatformSpawner : MonoBehaviour 
{
    public GameObject platformPrefab; // 생성할 발판의 원본 프리팹
    public int count = 3; // 생성할 발판의 개수

    public float timeBetSpawnMin = 1.25f; // 다음 배치까지의 시간 간격 최솟값
    public float timeBetSpawnMax = 2.25f; // 다음 배치까지의 시간 간격 최댓값
    private float timeBetSpawn; // 다음 배치까지의 시간 간격

    public float yMin = -3.5f; // 배치할 위치의 최소 y값
    public float yMax = 1.5f; // 배치할 위치의 최대 y값
    private float xPos = 20f; // 배치할 위치의 x 값

    private GameObject[] platforms; // 미리 생성한 발판들
    private int currentIndex = 0; // 사용할 현재 순번의 발판

    private Vector2 poolPosition = new Vector2(0, -20); // 초반에 생성된 발판들을 화면 밖에 숨겨둘 위치
    private float lastSpawnTime; // 마지막 배치 시점


    void Start() {
        // 변수들을 초기화하고 사용할 발판들을 미리 생성

        platforms = new GameObject[count]; // 발판 배열 생성

        for (int i = 0; i < count; i++) {
            platforms[i] = Instantiate(platformPrefab, poolPosition, Quaternion.identity); // 새 발판을 복제 생성함
        }

        lastSpawnTime = 0f; // 마지막 배치 시점 초기화
        timeBetSpawn = 0f; // 다음번 배치까지의 시간 간격 초기화

    }

    void Update() 
    {
        // 순서를 돌아가며 주기적으로 발판을 배치

        if (GameManager.instance.isGameover) 
        { // 게임오버 상태에서 더이상 진행되지 않도록 함
            return;
        }

        if (Time.time >= lastSpawnTime + timeBetSpawn) 
        { // 발판을 재배치한 가장 최근 시점에서 지금까지 시간이 얼마나 지났는지 확인
            lastSpawnTime = Time.time; // Time.time은 현재시점을 의미, 가장 최근 배치 시점을 갱신
            timeBetSpawn = Random.Range(timeBetSpawnMin, timeBetSpawnMax); // 다음번 발판 배치 시간을 랜덤 값으로 변경
            float yPos = Random.Range(yMin, yMax); // 발판을 재배치할 y좌표 랜점으로 설정
            platforms[currentIndex].SetActive(false);
            platforms[currentIndex].SetActive(true); // 발판 오브젝트를 껏다 켜서 게임 오브젝트의 상태를 리셋함 -> OnEnable 메서드 실행하기 위함
            platforms[currentIndex].transform.position = new Vector2(xPos, yPos); // 리셋한 후 위치를 변경하고 순번을 넘긴다.
            currentIndex++;

            if (currentIndex >= count) 
            {
                currentIndex = 0; // 순번의 마지막에 도달하면 순번을 리셋한다.
            }
        }
    }
}