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

    // The ground layer ref
    private LayerMask ground;

    // The player current type
    private PossibleType currentPlayer;

    private PossibleType currentEnemy;

    // 1==Rock || 2==Paper || 3=Scissors
    private int randomType;

    private IEnumerable<PossibleType> possibleTypeList = System.Enum.GetValues(typeof(PossibleType)).Cast<PossibleType>();

    // Player sprite rendered refrence
    private SpriteRenderer playerRendered;

    // Holding the rock/ paper/ scissors pictures
    public Sprite[] pictureTypeList = new Sprite[3];

    // Holding refrence to the dash effect
    public GameObject dashEffect;
    
    public bool isDead;
    public DeathMenu deathMenu;
    public float score;
    public Text scoreTxt;
    public Text timeText;
    private int lives = 3;

    public Image[] heartPictures = new Image[3];

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
        // At the start the player is alive
        isDead = false;

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
        if (!isDead)
        {
            // Updates score
            score += Time.fixedDeltaTime;

            // Controls movement
            Movements();
        }

        // Else dont display hearts
        else
        {
            for (int index = 2; index > -1; index--)
            {
                heartPictures[index].gameObject.SetActive(false);
            }
        }

        // Displays hearts
        DisplayLives();

        // If the player broke the high score, update the high score
        if (score > PlayerPrefs.GetFloat(Consts.HIGH_SCORE))
        {
            PlayerPrefs.SetFloat(Consts.HIGH_SCORE, score);
        }

        scoreTxt.text = Consts.TIME_SURVIVED + (int)score;
        timeText.text = Consts.TIME + (int)score;
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
        if (currentPlayer == currentEnemy - 1)
        {
            Death();
        }
        // If the are equal --lives
        else if (currentPlayer == currentEnemy)
        {
            lives--;
            if (lives == 0)
            {
                isDead = true;
            }
        }
        else if (currentPlayer == PossibleType.Scissors && currentEnemy == PossibleType.Rock)
        {
            Death();
        }

        if (!isDead)
        {
            Destroy(collisionObject.gameObject);
        }
    }

    /// <summary>
    /// Displays lives
    /// </summary>
    private void DisplayLives()
    {
        switch (lives)
        {
            case 0:
                heartPictures[0].gameObject.SetActive(false);
                Death();
                break;
            case 1:
                heartPictures[1].gameObject.SetActive(false);
                break;
            case 2:
                heartPictures[2].gameObject.SetActive(false);
                break;
        }
    }

    private void Movements()
    {

        // Getting input for horizontal 
        horizontal = Input.GetAxis(Consts.HORIZONTAL);

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
                var isDashKeyDown = Input.GetKeyDown(KeyCode.F);

                // If the F was pressed
                if (isDashKeyDown)
                {
                    // Checks the direction the player is facing
                    direction = isFacingRight ? 1f : -1f;

                    // Moves the player
                    myPlayer.AddForce(new Vector2((Consts.dashSpeed * direction), myPlayer.velocity.y));

                    dashState = Consts.DashState.Dashing;

                    // Starts a cooldown timer
                    StartCoroutine(DashCooldown());
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

    private void Death()
    {
        isDead = true;
        deathMenu.ToggleEndMenu();
    }

    #endregion
}