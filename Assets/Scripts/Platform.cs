using UnityEngine;

// 발판으로서 필요한 동작을 담은 스크립트
public class Platform : MonoBehaviour 
{
    public GameObject[] obstacles; // 장애물 오브젝트들
    private bool stepped = false; // 플레이어 캐릭터가 밟았었는가

    private void OnEnable() {
        stepped = false;

        for(int i = 0; i < obstacles.Length; i++) {
            if (Random.Range(0,3) == 0) {
                obstacles[i].SetActive(true);
            }
            else {
                obstacles[i].SetActive(false);
            }
        }
        
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.collider.tag == "Player" && !stepped) {
            stepped = true;
            GameManager.instance.AddScore(1);
        }
        
    }



}
