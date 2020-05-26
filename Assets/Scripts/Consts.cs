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
        public const float easyDifficulity = 0.14f;
        public const float mediumDifficulity = 0.10f;
        public const float hardDifficulity = 0.075f;
        public const float hellDifficulity = 0.045f;

        // Speed of player
        public const float speed = 7.2f;

        // Jumpheight
        public const float JumpForce = 7f;

        #endregion
    }
}