using System.Collections;
using UnityEngine;

public class CharacterBehaviour : MonoBehaviour
{


    Rigidbody _Rigidbody;

    //Movement
    [SerializeField] float _MovementSpeed = 1.0f;

    int _MovementDirection = 0;

    bool _LeftButtonPressed = false;
    bool _RightButtonPressed = false;

    
    //Jumping
    bool _Jumping = false;
    WaitForFixedUpdate _WaitForFixedUpdate = new WaitForFixedUpdate();
    WaitForSeconds _WaitForJumpFinish = new WaitForSeconds(1.0f);

    [SerializeField] float _JumpPower = 17.0f;

    const string _JumpingTag = "Jumping";
    const string _Non_JumpingTag = "Idle";
    const string _AlreadyDestroyed = "Destroyed";
    const string _NotDestroyed = "NotDestroyed";

    const int _FloorLayer = 14;

    enum JumpState
    {
        OnTheFloor = 0,
        JumpStarted = 1,
        JumpWillFinishOnceFloorTouched = 2
    }

    JumpState _JumpCurrentState = JumpState.OnTheFloor;

    private void Awake()
    {
        _Rigidbody = GetComponent<Rigidbody>();
        _RightButtonPressed = false;
        _LeftButtonPressed = false;
        _MovementDirection = 0;
        gameObject.tag = _Non_JumpingTag;
        _JumpCurrentState = JumpState.JumpWillFinishOnceFloorTouched;
    }


    private void OnCollisionStay(Collision collision)
    {

        if (_JumpCurrentState == JumpState.JumpWillFinishOnceFloorTouched && collision.gameObject.layer == _FloorLayer)
        {
            _JumpCurrentState = JumpState.OnTheFloor;
            _Jumping = false;
            gameObject.tag = _Non_JumpingTag;
            StopAllCoroutines();
        }

        if(_Jumping && collision.gameObject.CompareTag(_NotDestroyed))
        {
            collision.gameObject.GetComponent<ProceduralDestruction>().DestroyMesh();

        }

    }
    private void FixedUpdate()
    {
        MovementControl();

        _Rigidbody.AddForce(Vector3.right * _MovementSpeed * _MovementDirection, ForceMode.Force);


        Vector3 rigidPos = _Rigidbody.position;
        rigidPos.z = 0;
        if (_Rigidbody.position.z != 0.0f) _Rigidbody.MovePosition(rigidPos);

    }

    private void MovementControl()
    {
        if (Input.GetKey(KeyCode.Space)) Button_Jump_Pressed();

        if ((Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A) || _LeftButtonPressed) &&
          (!Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.D) && !_RightButtonPressed)) _MovementDirection = -1;

        else if ((Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D) || _RightButtonPressed) &&
          (!Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.A) && !_LeftButtonPressed)) _MovementDirection = 1;

        else _MovementDirection = 0;
    }

    public void Restart(Vector3 startingPosition)
    {
        _MovementDirection = 0;

        _LeftButtonPressed = false;
        _RightButtonPressed = false;

        transform.localScale = Vector3.one;
        _Rigidbody.MovePosition(startingPosition);
        _JumpCurrentState = JumpState.JumpWillFinishOnceFloorTouched;
    }

    public void Button_Left_Pressed()
    {
        _LeftButtonPressed = true;
    }
    public void Button_Right_Pressed()
    {
        _RightButtonPressed = true;
    }
    public void Button_Right_NotPressed()
    {
        _RightButtonPressed = false;
    }

    public void Button_Left_NotPressed()
    {
        _LeftButtonPressed = false;
    }
    public void Button_Jump_Pressed()
    {
        if(!_Jumping) StartCoroutine(Jump());
    }

    IEnumerator Jump()
    {
        _Jumping = true;
        _JumpCurrentState = JumpState.JumpStarted;
        gameObject.tag = _JumpingTag;
        yield return _WaitForFixedUpdate;
        _Rigidbody.AddForce(Vector3.up * _JumpPower, ForceMode.Impulse);
        yield return _WaitForJumpFinish;
        _JumpCurrentState = JumpState.JumpWillFinishOnceFloorTouched;

    }

}
