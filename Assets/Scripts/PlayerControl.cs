using System;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerControl : MonoBehaviour
{
    //Config
    [SerializeField] private  float movementSpeed;
    [SerializeField] private  float gravity;
    [SerializeField] private  float jumpStartSpeed;

    
    //Components
    private Animator _animator;
    private CharacterController _characterController;
    
    //Inputs
    private float _horizontalInput;
    [NonSerialized] public float beamInput;
    private float _jumpInput;
    
    
    
    //Util
    private float _verticalSpeed;
    
    void Start()
    {
        _animator = GetComponentInChildren<Animator>();
        _characterController = GetComponent<CharacterController>();
    }
    void Update()
    {
        Vector3 pos = transform.position;
        Camera.main.transform.position = new Vector3(pos.x,pos.y,-10);
        
        //RESTART
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SceneManager.LoadScene("SampleLevel");
        }
        
        GetInputs();
        Jump();
        Move();
        Rotate();
    }


    void GetInputs()
    {
        _horizontalInput = Input.GetAxis("Horizontal");
        beamInput = Input.GetAxis("Fire1");
        _jumpInput = Input.GetAxis("Jump");
        
        _animator.SetFloat("horizontal",Mathf.Abs(_horizontalInput));
        _animator.SetFloat("beam",Mathf.Abs(beamInput));
    }

    void Jump()
    {
        if (_characterController.isGrounded)
        {

            if (_jumpInput > 0.1f)
            {
                _verticalSpeed = jumpStartSpeed;
            }
            else if(_verticalSpeed < -0.1f)
            {
                _verticalSpeed = 0;
            }
        }
        else
        {
            _verticalSpeed -= Time.deltaTime * gravity;
        }
    }

    void Move()
    {

        Vector3 deltaHorizontal = Vector3.right * (Time.deltaTime * movementSpeed * _horizontalInput);
        _characterController.Move(deltaHorizontal);
        

        if (Mathf.Abs(_verticalSpeed) > 0.001f)
        {
            Vector3 deltaVertical = Vector3.up * (Time.deltaTime * _verticalSpeed);
            CollisionFlags collisionFlags = _characterController.Move(deltaVertical);
            if ((collisionFlags & CollisionFlags.Above) != 0)
            {
                _verticalSpeed = 0;
            }
        }
    }


    void Rotate()
    {
        if (_horizontalInput > 0.1f)
        {
            transform.eulerAngles = new Vector3(0, 90, 0);
        }
        else if (_horizontalInput < -0.1f)
        {
            transform.eulerAngles = new Vector3(0, 270, 0);
        }
    }


    public void TeleportToPosition(Vector3 position)
    {
        _characterController.enabled = false;
        transform.position = position;
        _characterController.enabled = true;
    }


    public void SetVerticalSpeed(float amount)
    {
        _verticalSpeed = amount;
    }
    

}
