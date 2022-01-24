using UnityEngine;

// PlayerController는 플레이어 캐릭터로서 Player 게임 오브젝트를 제어한다.
public class PlayerController : MonoBehaviour 
{
   public AudioClip deathClip; // 사망시 재생할 오디오 클립
   public float jumpForce = 700f; // 점프 힘

   private int jumpCount = 0; // 누적 점프 횟수
   private bool isGrounded = false; // 바닥에 닿았는지 나타냄
   private bool isDead = false; // 사망 상태

   private Rigidbody2D playerRigidbody; // 사용할 리지드바디 컴포넌트
   private Animator animator; // 사용할 애니메이터 컴포넌트
   private AudioSource playerAudio; // 사용할 오디오 소스 컴포넌트

   private void Start() 
   {
       // 초기화
       playerRigidbody = GetComponent<Rigidbody2D>();
       animator = GetComponent<Animator>();
       playerAudio = GetComponent<AudioSource>();
   }

   private void Update() 
   {
       // 사용자 입력을 감지하고 점프하는 처리

       if (isDead) 
       {
           return; //플레이어가 죽은 경우 아래 if문으로 가지못하도록 함
       }

       if (Input.GetMouseButtonDown(0) && jumpCount < 2) 
       { //누적점프가 2회를 넘은 경우에는 점프를 못함 -> 3단 점프 못하도록! 0은 마우스 왼쪽버튼을 의미함 Down은 버튼을 누른 순간을 의미
           jumpCount++;
           playerRigidbody.velocity = Vector2.zero; // 점프 직전에 속도를 (0,0)으로 만듦 == new Vector2(0,0) 
           playerRigidbody.AddForce(new Vector2(0,jumpForce)); // 위쪽 방향으로 jumpForce만큼 힘을 준다 AddForce의 입력은 Vector2(x,y) 형식임 
           playerAudio.Play(); // 오디오소스 컴포넌트가 소리를 재생하도록 함 jump클립 재생
       }

       else if (Input.GetMouseButtonUp(0) && playerRigidbody.velocity.y > 0) 
       { // 마우스 왼쪽 버튼에서 손을 뗀 경우
           playerRigidbody.velocity = playerRigidbody.velocity * 0.5f; // 플레이어는 버튼을 오래 누를수록 높이 점프함
       }

       animator.SetBool("Grounded", isGrounded);
   }

   private void Die() 
   {
       // 사망 처리
       animator.SetTrigger("Die"); // 애니메이터의 Die 트리거 파라미터를 셋
       playerAudio.clip = deathClip; // 오디오 소스에 할당된 오디오 클립을 deathClip으로 변경
       playerAudio.Play(); // 사망 효과음 재생
       playerRigidbody.velocity = Vector2.zero; // 속도를 (0,0)으로 변경
       isDead = true; // 사망 상태를 true로 변경

       GameManager.instance.OnPlayerDead(); // 게임 매니저의 게임오버 처리 실행
   }

   private void OnTriggerEnter2D(Collider2D other) 
   {
       // 트리거 콜라이더를 가진 장애물과의 충돌을 감지

       if (other.tag == "Dead" && !isDead) 
       { // 충돌한 상대방의 태그가 Dead이고 아직 사망하지 않았다면 Die 실행
           Die();
       }
   }

   private void OnCollisionEnter2D(Collision2D collision) 
   {
       // 바닥에 닿았음을 감지하는 처리
       if (collision.contacts[0].normal.y > 0.7f) 
       {
           isGrounded = true;
           jumpCount = 0;
       }
   }

   private void OnCollisionExit2D(Collision2D collision) 
   {
       // 바닥에서 벗어났음을 감지하는 처리
        isGrounded = false;
   }
}