using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts;
using System.Linq;
using System.Collections;

/// <summary>
/// Managing player possible events
/// </summary>
/// 
public class Player : MonoBehaviour
{
    #region Data Members

    // Refrence to my rigidbody player
    private Rigidbody2D myPlayer;

    // x input
    private float horizontal;

    // If facing right or no
    private bool isFacingRight;

    public GameObject moveJoystick; 

    // The ground layer ref
    private LayerMask ground;

    // The player current type
    private PossibleType currentPlayer;

    private PossibleType currentEnemy;

    private CameraShaker cameraShakerScripts;

    private GhostEffect ghostEffect;

    private bool isInvincble = false;

    private Animator playerAnim;

    // 1==Rock || 2==Paper || 3=Scissors
    private int randomType;

    private IEnumerable<PossibleType> possibleTypeList = System.Enum.GetValues(typeof(PossibleType)).Cast<PossibleType>();

    // Player sprite rendered refrence
    private SpriteRenderer playerRendered;

    // Holding the rock/ paper/ scissors pictures
    public Sprite[] pictureTypeList = new Sprite[3];

    // Holding the rock/ paper/ scissors pictures for different game modes
    public Sprite[] pictureTypeListText = new Sprite[3];
    public Sprite[] pictureTypeListNumber = new Sprite[3];
    public Sprite[] pictureTypeListColor = new Sprite[3];

    // Holding refrence to the dash effect
    public GameObject dashEffect;
    
    public DeathMenu deathMenu;
    public Text scoreTxt;
    public Text timeText;

    private int currentModeRef; // 0 = NormalGame, 10 = HellGame, 1 = TextGame, 2 = NumberGame, 3 = ColorGame, 4 = HandSignGame ** 
    private int startCurrentModeRef;

    protected Joystick Joystick;
    protected DashButton DashJoybutton;

    private float direction;
    public Consts.DashState dashState;


    #endregion

    #region Functions

    private void Awake()
    {
        // Just for the first time the game runs it makes an highscore file 
        if (!PlayerPrefs.HasKey(Consts.HIGH_SCORE))
        {
            // And sets it to zero
            PlayerPrefs.SetFloat(Consts.HIGH_SCORE, 0);
        }
    }

    // Right after the Awake
    void Start()
    {
        Joystick = FindObjectOfType<Joystick>();
        DashJoybutton = FindObjectOfType<DashButton>();

        cameraShakerScripts = Camera.main.GetComponent<CameraShaker>();

        ghostEffect = gameObject.GetComponent<GhostEffect>();

        playerAnim = GetComponent<Animator>();
        

        // At the start the player is alive
        GameManager.Instance().Restart();

        playerRendered = GetComponent<SpriteRenderer>();

        // Picking a starting charecter 
        randomType = UnityEngine.Random.Range(0, 3);

        // CurrentPlayer equals to the PlayerType element at the random index
        currentPlayer = possibleTypeList.ElementAt(randomType);

        // Picture equals to the RPS at the random index
        playerRendered.sprite = pictureTypeList[randomType];

        // Gets refrebce to player's body
        myPlayer = GetComponent<Rigidbody2D>();

        // At the start the player facing right
        isFacingRight = true;

        // Get refrence to Ground Layer (like tagging)
        ground = LayerMask.GetMask(Consts.GROUND);

        startCurrentModeRef = GameManager.Instance().Mode;
    }

    private void Update()
    {
        Dash();
    }

    /// <summary>
    /// Called a fixed amount of times on a fixed span life
    /// </summary>
    void FixedUpdate()
    {

        // As long as the player is not dead, update score and enable movement
        if (!GameManager.Instance().IsDead)
        {
            // Controls movement
            Movements();

            currentModeRef = GameManager.Instance().Mode;

            if (currentModeRef != startCurrentModeRef)
            {
                startCurrentModeRef = currentModeRef;
                playerRendered.sprite = GetModeSprites(currentModeRef)[randomType];
                myPlayer.gameObject.transform.localScale = new Vector3(1, 1, 1);
            }
        }

        // Else dont display hearts
        else
        {
            foreach (GameObject heart in GameManager.Instance().HeartPictures)
            {
                heart.GetComponent<Image>().enabled = false;
            }

            moveJoystick.gameObject.SetActive(false);
        }

        // Displays hearts
        DisplayLives();

        // If the player broke the high score, update the high score
        if (GameManager.Instance().Score > PlayerPrefs.GetFloat(Consts.HIGH_SCORE))
        {
            PlayerPrefs.SetFloat(Consts.HIGH_SCORE, GameManager.Instance().Score);
        }

        scoreTxt.text = Consts.TIME_SURVIVED + (int)GameManager.Instance().Score;
        timeText.text = Consts.TIME + (int)GameManager.Instance().Score;
    }

    private Sprite[] GetModeSprites(int modeNum)
    {
        switch (modeNum)
        {
            case 0:
                return pictureTypeList;

            case 1:
                return pictureTypeListText;

            case 2:
                return pictureTypeListNumber;

            case 3:
                return pictureTypeListColor;

            default:
                return pictureTypeList;
        }

    }

    private void OnCollisionEnter2D(Collision2D collisionObject)
    {
        switch (collisionObject.gameObject.tag)
        {
            case Consts.TELEPORT_WALL:
                OnWallCollision();
                break;

            case Consts.GROUND:
                break;

            default:
                // Converting gameObject.tag(string) to PlayerType
                PossibleType enemyType;
                System.Enum.TryParse(collisionObject.gameObject.tag, out enemyType);
                currentEnemy = possibleTypeList.FirstOrDefault(x => x.ToString().Equals(collisionObject.gameObject.tag));
                WinLoseTie(currentPlayer, currentEnemy, collisionObject);
                break;
        }
    }

    
    private void OnWallCollision()
    {
        // if the player hits the left wall
        if (gameObject.transform.position.x < 0)
        {
            gameObject.transform.position = new Vector3(9, gameObject.transform.position.y, gameObject.transform.position.z);
        } 
        else
        {
            gameObject.transform.position = new Vector3(-9, gameObject.transform.position.y, gameObject.transform.position.z);
        }
    }

    private void WinLoseTie(PossibleType currentPlayer, PossibleType currentEnemy, Collision2D collisionObject)
    {
        // If the enemy is bigger than the player by one, call death function
        if (currentPlayer == currentEnemy - 1 && !isInvincble)
        {
            Death();
        }
        // If the are equal --lives
        else if (currentPlayer == currentEnemy && !isInvincble)
        {
            GameManager.Instance().Lives--;
            cameraShakerScripts.CollisionCameraShake();

            isInvincble = true;
            playerAnim.SetBool(Consts.IS_INVINCBLE, true);
            StartCoroutine(InvincbleCooldown());

            if (GameManager.Instance().Lives == 0)
            {
                Death();
            }
        }
        else if (currentPlayer == PossibleType.Scissors && currentEnemy == PossibleType.Rock && !isInvincble)
        {
            Death();
        }

        if (!GameManager.Instance().IsDead)
        {
            Destroy(collisionObject.gameObject);
        }
    }

    /// <summary>
    /// Displays lives
    /// </summary>
    private void DisplayLives()
    {
        switch (GameManager.Instance().Lives)
        {
            case 0:
                GameManager.Instance().HeartPictures[0].GetComponent<Image>().enabled = false;
                Death();
                break;
            case 1:
                GameManager.Instance().HeartPictures[1].GetComponent<Image>().enabled = false;
                break;
            case 2:
                GameManager.Instance().HeartPictures[2].GetComponent<Image>().enabled = false;
                break;
        }
    }

    private void Movements()
    {

        // Getting input for horizontal 
        horizontal = Joystick.Horizontal;

        // Player going left/right by the inputs
        myPlayer.velocity = new Vector2(horizontal * Consts.speed, myPlayer.velocity.y);

        ChangeDirection();
    }

    /// <summary>
    /// Changes direction of player
    /// </summary>
    private void ChangeDirection()
    {
        // If you go right but facing left or the oppsite then switch
        if (horizontal > 0 && !isFacingRight || horizontal < 0 && isFacingRight)
        {
            // Flipping boolean value
            isFacingRight = !isFacingRight;

            // Getting refrence to X scale value
            Vector3 PlayerScale = transform.localScale;

            // Flipping the scale flips the direction
            PlayerScale.x *= -1;

            // Changing the real value
            transform.localScale = PlayerScale;
        }
    }

    /// <summary>
    /// Player dash function
    /// </summary>
    private void Dash()
    {
        // Gets a dashstate(ready/dashing/cooldown), which starts as ready
        switch (dashState)
        {
            // If state is ready
            case Consts.DashState.Ready:
                var isDashKeyDown = Input.GetKeyDown(KeyCode.F) || DashJoybutton.Pressed;

                // If the F was pressed
                if (isDashKeyDown)
                {
                    // Starts making ghosts
                    ghostEffect.makeGhost = true;

                    // Checks the direction the player is facing
                    direction = isFacingRight ? 1f : -1f;

                    // Moves the player
                    myPlayer.AddForce(new Vector2((Consts.dashSpeed * direction), myPlayer.velocity.y));

                    dashState = Consts.DashState.Dashing;

                    // Starts a cooldown timer
                    StartCoroutine(DashCooldown());

                    // Starts a ghost timer
                    StartCoroutine(GhostTime());
                }
                break;
        }
    }

    /// <summary>
    /// Responsible for dashing cooldown
    /// </summary>
    IEnumerator DashCooldown()
    {
        yield return new WaitForSeconds(Consts.dashCooldownTime);
        dashState = Consts.DashState.Ready;
    }

    /// <summary>
    /// Responsible for invincble cooldown
    /// </summary>
    IEnumerator InvincbleCooldown()
    {
        yield return new WaitForSeconds(Consts.invincbleTime);
        isInvincble = false;
        playerAnim.SetBool(Consts.IS_INVINCBLE, false);
    }

    /// <summary>
    /// Responsible for dashing ghosts time
    /// </summary>
    IEnumerator GhostTime()
    {
        yield return new WaitForSeconds(0.15f);

        // Stops making ghosts
        ghostEffect.makeGhost = false;
    }

    private void Death()
    {
        GameManager.Instance().IsDead = true;
        deathMenu.ToggleEndMenu();
    }

    #endregion
}