using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float attackTime = 0.5f; // 공격 쿨타임
    public float attackRange = 1.5f;// 공격 사정거리

    public Vector2 inputVec;
    public Vector2 dirVec;          // 바라보는 방향
    [SerializeField]
    private float moveSpeed = 5f;   // 이동 속도

    public Rigidbody2D rigid;
    public SpriteRenderer spriter;

    public GameObject targetGem = null;

    public bool isAttack = false;

    public GameObject gemItemInven;

    [SerializeField]
    private Define.PlayerState state = Define.PlayerState.IDLE;

    [Header("GemInfo")]
    public int id;
    public float attack;
    public string gemName;
    public string spriteName;
    public Define.PlayerState State
    {
        get { return state; }
        set
        {
            state = value;

            Animator anim = GetComponent<Animator>();
            switch (state)
            {
                case Define.PlayerState.DIE:
                    anim.CrossFade("DIE", 0.1f);
                    break;
                case Define.PlayerState.IDLE:
                    anim.CrossFade("IDLE", 0.1f);
                    break;
                case Define.PlayerState.WALK:
                    anim.CrossFade("WALK", 0.1f);
                    break;
                case Define.PlayerState.ATTACK:
                    anim.CrossFade("ATTACK", 0.1f);
                    break;
            }
        }
    }
    private void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        Managers.Input.KeyAction -= OnKeyborard;
        Managers.Input.KeyAction += OnKeyborard;
        gemItemInven = Resources.Load<GameObject>("Prefabs/02GemItem"); // 인벤토리에 들어갈 보석 프리팹 로드
    }

    private void Update()
    {
        switch (State)
        {
            case Define.PlayerState.DIE:
                UpdateDie();
                break;
            case Define.PlayerState.IDLE:
                UpdateIdle();
                break;
            case Define.PlayerState.WALK:
                UpdateWalk();
                break;
            case Define.PlayerState.ATTACK:
                UpdateAttack();
                break;
        }
        TargetFind();
    }
    
    void LateUpdate()
    {
        // 좌우 이동시에 이미지 방향전환
        if (inputVec.x != 0)
            spriter.flipX = inputVec.x < 0;

        // 바라보는 방향
        dirVec.x = spriter.flipX ? -1 : 1;
    }
    private void OnKeyborard()
    {
        // GetAxisRaw : 키 입력 받을 때 딱 떨어지도록
        inputVec.x = Input.GetAxisRaw("Horizontal");
        inputVec.y = Input.GetAxisRaw("Vertical");

        Vector2 nextVec = inputVec.normalized * moveSpeed * Time.deltaTime;
        rigid.MovePosition(rigid.position + nextVec);

        Debug.DrawRay(transform.position, dirVec * attackRange, Color.green);
    }
    
    public void UpdateDie()
    {
        // 부활 할건지 물어보는 팝업 창 뜨게?

    }
    public void UpdateIdle()
    {
        if (!isAttack && Input.GetKeyDown(KeyCode.Space))
            State = Define.PlayerState.ATTACK;
        if (inputVec.magnitude != 0)
            State = Define.PlayerState.WALK;
    }
    public void UpdateWalk()
    {
        if (!isAttack && Input.GetKeyDown(KeyCode.Space))
            State = Define.PlayerState.ATTACK;
        if (Input.GetButton("Horizontal") == false && Input.GetButton("Vertical") == false)
        {
            inputVec = Vector2.zero;
            State = Define.PlayerState.IDLE;
        }
    }
    public void UpdateAttack()
    {
        StartCoroutine(AttackTime(attackTime)); // 공격 가능 쿨타임 설정
    }

    // 공격 애니메이션 끝나면 호출 되는 함수
    public void AttackEndEvent()
    {
        State = Define.PlayerState.IDLE;
    }
    public void OnHitEvent()
    {
        if (targetGem == null) return;
        Slider hpbar = targetGem.GetComponentInChildren<Slider>();
        hpbar.value -= 0.2f;
        if (hpbar.value <= 0.001f)
        {
            GameObject clone = Instantiate(gemItemInven, GetItemInven());
            GemInven gemInven = clone.GetComponent<GemInven>();
            gemInven.id = id;
            gemInven.attack = attack;
            gemInven.gemName = gemName;
            gemInven.spriteName = spriteName;
            Managers.Pool.ReturnObject(targetGem);
        }
        
    }
    public void TargetFind()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dirVec.normalized, attackRange, LayerMask.GetMask("Gem"));
        if (hit.collider != null)
        {
            targetGem = hit.collider.gameObject;
            GemItem gem = targetGem.GetComponent<GemItem>();
            GemInfo gemInfo = gem.dicGem[gem.gemId];
            id = gemInfo.id;
            attack = gemInfo.attack;
            gemName = gemInfo.gemName;
            spriteName = gemInfo.spriteName;
        }
        else
        {
            id = 0;
            attack = 0;
            gemName = null;
            spriteName = null;
            targetGem = null;
            //sprite = null;
        }
    }
    IEnumerator AttackTime(float attackTime)
    {
        isAttack = true;
        yield return new WaitForSeconds(attackTime);
        isAttack = false;
    }

    public Transform GetItemInven()
    {
        for (int i = 0; i < UIManager.instance.itemLists.Length; i++)
        {
            if (UIManager.instance.itemLists[i].transform.childCount == 0)
            {
                return UIManager.instance.itemLists[i].transform;
            }
        }
        return null;
    }
}
