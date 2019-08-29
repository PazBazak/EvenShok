using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts;
using System.Linq;

/// <summary>
/// Managing player possible events
/// </summary>
public class Player : MonoBehaviour
{
    #region Data Members

    // Refrence to my rigidbody player
    private Rigidbody2D MyPlayer;

    // x input
    private float Horizontal;

    // Speed of player
    private float speed = 7.2f;

    // If facing right or no
    private bool Right;

    // The ground layer ref
    private LayerMask Ground;

    // The player current type
    public PossibleType currentPlayer;

    public PossibleType currentEnemy;

    // 1==Rock || 2==Paper || 3=Scissors
    private int RandomType;

    private IEnumerable<PossibleType> possibleTypeList = System.Enum.GetValues(typeof(PossibleType)).Cast<PossibleType>();

    // Player sprite rendered refrence
    private SpriteRenderer PlayerRendered;

    // Holding the rock/ paper/ scissors pictures
    public Sprite[] RPS = new Sprite[3];

    public GameObject dashEffect;

    public bool isDead;
    public DeathMenu DeathMenu;
    public float score;
    public Text ScoreTxt;
    public Text TimeText;
    private int lives = 3;

    // [SerializeField] private AudioSource Hit;
    [SerializeField] private Image Heart1;
    [SerializeField] private Image Heart2;
    [SerializeField] private Image Heart3;
    private float direction;

    #endregion

    #region Functions

    // Right after the Awake
    void Start()
    {
        // Just for the first time the game runs it makes an highscore file 
        if (!PlayerPrefs.HasKey(Consts.HIGH_SCORE))
        {
            // And sets it to zero
            PlayerPrefs.SetFloat(Consts.HIGH_SCORE, 0);
        }

        // At the start the player is alive
        isDead = false;

        PlayerRendered = GetComponent<SpriteRenderer>();

        // Picking a starting charecter 
        RandomType = Random.Range(0, 3);

        // CurrentPlayer equals to the PlayerType element at the random index
        currentPlayer = possibleTypeList.ElementAt(RandomType);

        // Picture equals to the RPS at the random index
        PlayerRendered.sprite = RPS[RandomType];

        // Gets refrebce to player's body
        MyPlayer = GetComponent<Rigidbody2D>();

        // At the start the player facing right
        Right = true;

        // Get refrence to Ground Layer (like tagging)
        Ground = LayerMask.GetMask(Consts.GROUND);
    }

    private void OnCollisionEnter2D(Collision2D collisionObject)
    {
        if (!collisionObject.gameObject.tag.Equals(Consts.GROUND))
        {
            // Converting gameObject.tag(string) to PlayerType
            PossibleType enemyType;
            System.Enum.TryParse(collisionObject.gameObject.tag, out enemyType);
            currentEnemy = possibleTypeList.FirstOrDefault(x => x.ToString().Equals(collisionObject.gameObject.tag));
            WinLoseTie(currentPlayer, currentEnemy, collisionObject);
        }
    }

    /// <summary>
    /// Manages the outcome of collision with enemy
    /// </summary>
    /// <param name="currentPlayer"></param>
    /// <param name="currentEnemy"></param>
    /// <param name="collisionObject"></param>
    private void WinLoseTie(PossibleType currentPlayer, PossibleType currentEnemy, Collision2D collisionObject)
    {
        // If the enemy is bigger than the player by one, call death function
        if (currentPlayer  == currentEnemy - 1)
        {
            Death();
        }
        // If the are equal --lives
        else if (currentPlayer == currentEnemy)
        {
            lives--;
        }
        else if (currentPlayer == PossibleType.Scissors && currentEnemy == PossibleType.Rock)
        {
            Death();
        }

        Destroy(collisionObject.gameObject);
    }

    private void Update()
    {
        DisplayLives();

        if (!isDead)
        {
            score += Time.deltaTime;
        }
        else
        {
            Heart3.gameObject.SetActive(false);
            Heart2.gameObject.SetActive(false);
            Heart1.gameObject.SetActive(false);
        }

        if (score > PlayerPrefs.GetFloat(Consts.HIGH_SCORE))
        {
            // If the score is higher than highscore then its now the highscore
            PlayerPrefs.SetFloat(Consts.HIGH_SCORE, score);
        }

        ScoreTxt.text = Consts.TIME_SURVIVED + (int)score;
        TimeText.text = Consts.TIME + (int)score;
    }


    /// <summary>
    /// Displays lives
    /// </summary>
    private void DisplayLives()
    {
        switch (lives)
        {
            case 0:
                Heart1.gameObject.SetActive(false);
                Death();
                break;

            case 1:
                Heart2.gameObject.SetActive(false);
                break;

            case 2:
                Heart3.gameObject.SetActive(false);
                break;
        }
    }

    // Called a fixed amount of times on a fixed span life
    void FixedUpdate()
    {
        if (!isDead)
        {
            // Getting input for horizontal 
            Horizontal = Input.GetAxis(Consts.HORIZONTAL);

            if (Input.GetMouseButton(0))
            {
                // Move right
                if (Input.mousePosition.x > Screen.width / 2)
                {
                    Horizontal = 1;
                }
                else
                {
                    Horizontal = -1;
                }
            }

            Movements(Horizontal);
            ChangeDirection(Horizontal);
        }
    }

    // Chaning direction of player
    private void ChangeDirection(float x)
    {
        // If you go right but facing left or the oppsite then switch
        if (x > 0 && !Right || x < 0 && Right)
        {
            // Flipping boolean value
            Right = !Right;

            // Getting refrence to X scale value
            Vector3 PlayerScale = transform.localScale;

            // Flipping the scale flips the direction
            PlayerScale.x *= -1;

            // Changing the real value
            transform.localScale = PlayerScale;
        }
    }

    private void Movements(float x)
    {
        // Player going left/right by the inputs
        MyPlayer.velocity = new Vector2(x * speed, MyPlayer.velocity.y);
        Dash();

    }
    private void Dash()
    {
        if (Right)
        {
            direction = 1f;
        }
        else
        {
            direction = -1f;
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            //Instantiate(dashEffect, MyPlayer.transform.position, Quaternion.identity);
            MyPlayer.velocity = new Vector2(speed * 15 * direction, MyPlayer.velocity.y);                          
        }
        
    }

    private void Death()
    {
        isDead = true;
        DeathMenu.ToggleEndMenu();
    }

    #endregion
}