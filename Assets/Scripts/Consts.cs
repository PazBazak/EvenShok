using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    /// <summary>
    /// Consts
    /// </summary>
    public class Consts
    {
        #region Consts
        
        public const string SPAWN_LOCATION = "SpawnLocation";
        public const string SPAWN_LOCATION_ASTROIDS = "AstroidLocation";
        public const string ASTROID_TAG = "Astroid";
        public const string PLAYER = "Player";
        public const string ROCK = "Rock";
        public const string PAPER = "Paper";
        public const string SCISSORS = "Scissors";
        public const string SOUND = "Sound";
        public const string GROUND = "Ground";
        public const string MENU = "Menu";
        public const string HIGH_SCORE = "HighScore ";
        public const string TIME_SURVIVED = "Time survived : ";
        public const string TIME = "Time : ";
        public const string HORIZONTAL = "Horizontal";
        public const string NEVER = "NEVER";
        public const string PLAY_SCENE = "Falling rock paper scissors";
        public const string OPTIONS = "OptionMenu";
        public const string EASY_DIFF = "Easy";
        public const string MEDIUM_DIFF = "Medium";
        public const string HARD_DIFF = "Hard";
        public const string HELL_DIFF = "HELL";

        // Game difficulities values
        public const float easyDifficulity = 0.13f;
        public const float mediumDifficulity = 0.097f;
        public const float hardDifficulity = 0.074f;
        public const float hellDifficulity = 0.05f;

        // Speed of player
        public const float speed = 7.2f;

        // Jumpheight
        public const float JumpForce = 7f;

        // Camera Shake
        public const float durationEasyShake = 0.35f;
        public const float xPowerEasyShake = 0.38f;
        public const float yPowerEasyShake = 0.18f;

        public const float durationMediumShake = 0.53f;
        public const float xPowerMediumShake = 0.44f;
        public const float yPowerMediumShake = 0.24f;

        public const float durationHardShake = 0.73f;
        public const float xPowerHardShake = 0.48f;
        public const float yPowerHardShake = 0.28f;

        public const float durationHellShake = 10f;
        public const float xPowerHellShake = 0.41f;
        public const float yPowerHellShake = 0.23f;

        public const float durationCollisionShake = 0.53f;
        public const float xPowerCollisionShake = 0.44f;
        public const float yPowerCollisionShake = 0.24f;

        public const float durationAstroidShake =1.3f;
        public const float xPowerAstroidShake = 0.55f;
        public const float yPowerAstroidShake = 0.31f;


        // Dash related
        public enum DashState
        {
            Ready,
            Dashing,
            Cooldown
        }

        public const float maxDash = 20f;
        public const float dashSpeed = speed * 300;
        public const float dashCooldownTime = 1;

        #endregion
    }
}