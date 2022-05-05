using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerControler : MonoBehaviour
{
    public delegate void PlayerDelegate();
    public static event PlayerDelegate OnPlayerDied;
    public static event PlayerDelegate OnPlayerScored;

    public Vector3 startPos;
    public float tSmooth = 5;
    public float tapPower = 10;
    private Animator anim;

    public AudioSource tapAudio;
    public AudioSource scoreAudio;
    public AudioSource dieAudio;
   

    Rigidbody2D rigidbody;
    Quaternion forwardRotation;
    Quaternion downRotation;
    GameManager game;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        forwardRotation = Quaternion.Euler(0, 0, 25);
        downRotation = Quaternion.Euler(0, 0, -70);
        game = GameManager.Instance;
        rigidbody.simulated = false;

    }

    void OnEnable()
    {
        GameManager.OnGameStarted += OnGameStarted;
        GameManager.OnGameOverConfirmed += OnGameOverConfirmed;
    }

    void OnDisable()
    {
        GameManager.OnGameStarted -= OnGameStarted;
        GameManager.OnGameOverConfirmed -= OnGameOverConfirmed;

    }

    void OnGameStarted()
    {
        rigidbody.velocity = Vector3.zero;
        rigidbody.simulated = true;
    }

    void OnGameOverConfirmed()
    {
        transform.localPosition = startPos;
        transform.rotation = Quaternion.identity;
    }

    void Update()
    {
        if (game.GameOver) return;

        if(Input.GetMouseButtonDown(0))
        {
            tapAudio.Play();
            transform.rotation = forwardRotation;
            rigidbody.velocity = Vector3.zero;
            rigidbody.AddForce(Vector2.up * tapPower, ForceMode2D.Force);
        }
        transform.rotation = Quaternion.Lerp(transform.rotation, downRotation, tSmooth * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "ScoreArea")
        {
            OnPlayerScored();
            scoreAudio.Play();
        }

        if(col.gameObject.tag == "DeadZone")
        {
            rigidbody.simulated = false;

            OnPlayerDied();
            dieAudio.Play();
        }
    }

    
}
