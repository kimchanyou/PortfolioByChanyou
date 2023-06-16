using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    private UIManager canvas_UI;
    private FloatingText floatingText;

    public float attackTime = 0.5f;     // 공격 쿨타임

    public Vector2 inputVec;
    public Vector2 dirVec;              // 바라보는 방향

    public static Vector2 attackVec;
    public static string attackSpriteName;

    [SerializeField]
    private float moveSpeed = 5f;       // 이동 속도
    public float ditectionRange = 10f;   // 가이드라인 생성 조건 거리

    public Rigidbody2D rigid;
    public SpriteRenderer spriter;
    public AudioSource audioSource;
    public AudioClip attackClip;
    public AudioClip gemAttackClip;
    public Camera mainCam;
    private Texture2D basicCursor;

    public GameObject targetGem = null;
    public GameObject gemItemInven;
    public GameObject guideLine;

    public GemInven[] attackable;

    public bool isAttack = false;

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
        canvas_UI = FindObjectOfType<UIManager>();
        floatingText = canvas_UI.transform.GetChild(4).GetComponent<FloatingText>();
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        attackClip = Resources.Load<AudioClip>("Sounds/Attack");
        gemAttackClip = Resources.Load<AudioClip>("Sounds/GemAttack");
        basicCursor = Resources.Load<Texture2D>("Textures/Cursor/Basic");
        mainCam = Camera.main;
        Managers.Input.KeyAction -= OnKeyborard;
        Managers.Input.KeyAction += OnKeyborard;
        //Managers.Input.MounseAction -= OnMouseEvent;
        //Managers.Input.MounseAction += OnMouseEvent;
        gemItemInven = Resources.Load<GameObject>("Prefabs/02GemItem"); // 인벤토리에 들어갈 보석 프리팹 로드
        guideLine = transform.GetChild(0).gameObject;
        Cursor.SetCursor(basicCursor, new Vector2(basicCursor.width / 4, basicCursor.height / 4), CursorMode.Auto);
    }

    private void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;  // 인벤토리에서 마우스로 움직일시 다른 이벤트 받지 않도록
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
        MouseAttack();
        attackable = GameObject.Find("Attack").GetComponentsInChildren<GemInven>();
    }
    
    void LateUpdate()
    {
        // 좌우 이동시에 이미지 방향전환
        if (inputVec.x != 0)
            spriter.flipX = inputVec.x < 0;

        // 바라보는 방향 RayCastHit방식에서 사용
        //dirVec.x = spriter.flipX ? -1 : 1;

    }
    private void OnKeyborard()
    {
        // GetAxisRaw : 키 입력 받을 때 딱 떨어지도록
        inputVec.x = Input.GetAxisRaw("Horizontal");
        inputVec.y = Input.GetAxisRaw("Vertical");

        Vector2 nextVec = inputVec.normalized * moveSpeed * Time.deltaTime;
        rigid.MovePosition(rigid.position + nextVec);
    }
    private void OnMouseEvent(Define.MouseEvent evt)
    {
        switch (State)
        {
            case Define.PlayerState.IDLE:
                break;
            case Define.PlayerState.WALK:
                break;
            case Define.PlayerState.ATTACK:
                break;
        }
    }

    public void UpdateDie()
    {
        // 부활 할건지 물어보는 팝업 창 뜨게?
    }
    public void UpdateIdle()
    {
        if (!isAttack && Input.GetMouseButtonDown(0))
            State = Define.PlayerState.ATTACK;
        if (inputVec.magnitude != 0)
            State = Define.PlayerState.WALK;
    }
    public void UpdateWalk()
    {
        if (!isAttack && Input.GetMouseButtonDown(0))
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
        audioSource.clip = attackClip;
        audioSource.Play();
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
            Managers.Pool.ReturnObject(targetGem, Managers.Pool.itemRoot, Managers.Pool.itemPool);
            floatingText.ShowText("레벨 " + (id + 1).ToString() + "보석 획득");
        }

    }
    #region RaycastHit 방식 TargetFind
    //private void TargetFind()
    //{
    //    RaycastHit2D hit = Physics2D.Raycast(transform.position, dirVec.normalized, attackRange, LayerMask.GetMask("Gem"));
    //    if (hit.collider != null)
    //    {
    //        targetGem = hit.collider.gameObject;
    //        GemItem gem = targetGem.GetComponent<GemItem>();
    //        GemInfo gemInfo = gem.dicGem[gem.gemId];
    //        id = gemInfo.id;
    //        attack = gemInfo.attack;
    //        gemName = gemInfo.gemName;
    //        spriteName = gemInfo.spriteName;
    //    }
    //    else
    //    {
    //        id = 0;
    //        attack = 0;
    //        gemName = null;
    //        spriteName = null;
    //        targetGem = null;
    //    }
    //}
    #endregion
    private void TargetFind()
    {
        targetGem = FindTarget.targetGem;
        if (targetGem != null)
        {
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
        }
    }

    IEnumerator AttackTime(float attackTime)
    {
        isAttack = true;
        yield return new WaitForSeconds(attackTime);
        isAttack = false;
    }

    private Transform GetItemInven()
    {
        for (int i = 0; i < UIManager.instance.itemInvenLists.Length; i++)
        {
            if (UIManager.instance.itemInvenLists[i].transform.childCount == 0)
            {
                return UIManager.instance.itemInvenLists[i].transform;
            }
        }
        return null;
    }

    private void MouseAttack()
    {
        Vector2 mousePos = Input.mousePosition;
        // 현재 마우스의 위치를 게임내의 Position 값으로 변환
        mousePos = mainCam.ScreenToWorldPoint(mousePos);

        Vector3 playerPos = transform.position;

        Vector2 distanceVec = mousePos - (Vector2)playerPos;

        guideLine.SetActive(distanceVec.magnitude < ditectionRange ? true : false);

        guideLine.transform.right = distanceVec.normalized;

        if (Input.GetMouseButtonDown(0))
        {
            spriter.flipX = distanceVec.x > 0 ? false : true;
            if (targetGem != null) return;
            for (int i = 0; i < attackable.Length; i++)
            {
                if (attackable[i].isCool == false)
                {
                    attackable[i].isCool = true;
                    attackSpriteName = attackable[i].spriteName;
                    attackVec = distanceVec;
                    GameObject attackGem = Managers.Pool.GetObject(Managers.Pool.attackPool);
                    if (attackGem == null) return;
                    audioSource.clip = gemAttackClip;
                    audioSource.Play();
                    attackGem.transform.position = transform.position + (Vector3)distanceVec.normalized;
                    attackGem.GetComponent<Rigidbody2D>().AddForce(distanceVec.normalized * 1000f);
                    break;
                }
            }
            
        }
    }
    //private void UpdateMouseCursor()
    //{
    //    Vector2 mousePos = Input.mousePosition;
    //    // 현재 마우스의 위치를 게임내의 Position 값으로 변환
    //    mousePos = mainCam.ScreenToWorldPoint(mousePos);

    //    Ray2D ray2D = new Ray2D(mousePos, Vector2.zero);
    //    RaycastHit2D hit2D = Physics2D.Raycast(ray2D.origin, ray2D.direction);
        
    //    if (hit2D.collider.gameObject.layer == LayerMask.GetMask("Gem"))
    //    {
    //        Cursor.SetCursor(attackCursor, new Vector2(attackCursor.width / 4, attackCursor.height / 4), CursorMode.Auto);
    //    }
    //    else if (hit2D.collider.gameObject.layer == LayerMask.GetMask("Weapon"))
    //    {
    //        Cursor.SetCursor(mergeCursor, new Vector2(mergeCursor.width / 4, mergeCursor.height / 4), CursorMode.Auto);
    //    }
    //    else
    //    {
    //        Cursor.SetCursor(basicCursor, new Vector2(basicCursor.width / 4, basicCursor.height / 4), CursorMode.Auto);
    //    }
    //}
}
