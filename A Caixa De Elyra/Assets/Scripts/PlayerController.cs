using UnityEngine;

public class PlayerController:MonoBehaviour{
    [Header("Movimento")]
    public float moveSpeed = 5f;
    public float jumpForce = 12f;

    [Header("Ataque")]
    public Animator animator;

    private Rigidbody2D rb;
    private bool isGrounded;
    private int extraJumps;
    public int extraJumpValue = 1; 

    private void Start(){
        rb= GetComponent<Rigidbody2D>();
        extraJumps = extraJumpValue;
    }

    private void Update(){
        Move();
        Jump();
        Attack();
        Flip();
    }

    void Move(){
        float moveInput=
        Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        if(animator !=null)
        animator.SetFloat("Speed", Mathf.Abs(moveInput));
    }

    void Jump(){
        if(isGrounded)
        extraJumps = extraJumpValue;

        if(Input.GetKeyDown(KeyCode.Space)){
            if(isGrounded || extraJumps >0){
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);

                if(!isGrounded)
                extraJumps--;

                if(animator !=null)
                animator.SetTrigger("Jump");
            }
        }
    }
    void Attack(){
        if (Input.GetKeyDown(KeyCode.M)){
            if(animator!=null)
            animator.SetTrigger("Attack");
        }
    }
    void Flip(){
        float moveInput = Input.GetAxisRaw("Horizontal");
        if(moveInput !=0)
        transform.localScale = new Vector3(Mathf.Sign(moveInput)*Mathf.Abs(transform.localScale.x), transform.localScale.y, 1);
    }
    private void OnCollisionEnter2D(Collision2D collision){
        if(collision.gameObject.CompareTag("Ground"))
        isGrounded = true;
    }
    private void OnCollisionExit2D(Collision2D collision){
        if(collision.gameObject.CompareTag("Ground"))
        isGrounded = false;
    }
}
