using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public Vector2 direction;
    public float moveSpeed = 3.5f;
    Rigidbody2D rb;
    public Player.ControlState PlayerState;
    public LayerMask InteractMask;

    public static GameObject playerGO;

    // Use this for initialization
    void Start () {
	if (playerGO == null)
	    playerGO = gameObject;
	else if(playerGO != this.gameObject){
	    Destroy(this.gameObject);
	    return;
	}

	DontDestroyOnLoad(transform.gameObject);
	rb = GetComponent<Rigidbody2D>();
    }

    void Update () {
	switch (PlayerState) {
	    case Player.ControlState.Normal:
		float h = Input.GetAxisRaw("Horizontal");
		float v = Input.GetAxisRaw("Vertical");
		rb.velocity = ( h * Vector3.right + v * Vector3.up ) * moveSpeed;

		if(Input.GetKeyDown(KeyCode.Q)){
		    Debug.Log("interact!!!");
		    Interact();
		}
		break;
	    case Player.ControlState.Talking:
		if (Input.GetKeyDown(KeyCode.Q))
		    DialogueEngine.waitingConfirm = false;
		break;
	    default:
		break;
	}
    }

    public static void SetPlayerPosition(Vector2 newPosition){
	playerGO.transform.position = newPosition;
    }

    void Interact(){
	Vector2 size = direction.x != 0? new Vector2(1,0.5f) : new Vector2(0.5f,1);
	RaycastHit2D hitInfo = Physics2D.BoxCast((Vector2)transform.position+direction, size, 0, direction, 0, InteractMask);
	if (hitInfo.collider != null){
	    var t = hitInfo.collider.gameObject.GetComponent<IInteractable>();
	    if(t != null)
		t.InteractWithPlayer();
	}
    }
}
