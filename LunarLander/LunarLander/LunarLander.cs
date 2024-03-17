using CS5410;
using CS5410.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace LunarLander
{
    public class LunarLander : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private IGameState m_currentState;
        private Dictionary<GameStateEnum, IGameState> _states;

        private Rectangle m_background;
        private Texture2D m_backgroundImage;
        private Lander lander;

        // The Keyboard will be handeled here, but it's input will be handled in the game view??
        private CS5410.Input.KeyboardInput m_inputKeyboard;

        public LunarLander()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
           
            _graphics.PreferredBackBufferWidth = 1920;
            _graphics.PreferredBackBufferHeight = 1080;
            _graphics.ApplyChanges();

            m_inputKeyboard = new KeyboardInput();

            _states = new Dictionary<GameStateEnum, IGameState>
            {
                { GameStateEnum.MainMenu, new MainMenuView() },
                { GameStateEnum.GamePlay, new GamePlayView(m_inputKeyboard) },
                { GameStateEnum.HighScores, new HighScoresView() },
                { GameStateEnum.Help, new HelpView() },
                { GameStateEnum.Controls, new ControlsView(m_inputKeyboard) },
                { GameStateEnum.About, new AboutView() }
            };

            // Give all game states a chance to initialize, other than constructor
            foreach (var item in _states)
            {
                item.Value.initialize(this.GraphicsDevice, _graphics);
            }

            // We are starting with the main menu
            m_currentState = _states[GameStateEnum.MainMenu];


            //Get the baclkground ready
            m_background = new Rectangle(0, 0, 1980, 1080);

            


            //TODO change this to load saved controls ????

            base.Initialize();
            
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            // Give all game states a chance to load their content
            foreach (var item in _states)
            {
                item.Value.loadContent(this.Content);
            }

            m_backgroundImage = this.Content.Load<Texture2D>("Images/LunarBackground");

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            GameStateEnum nextStateEnum = m_currentState.processInput(gameTime);

            // Special case for exiting the game
            if (nextStateEnum == GameStateEnum.Exit)
            {
                Exit();
            }
            else
            {
                m_currentState.update(gameTime); // TODO update game input there? 
                m_currentState = _states[nextStateEnum];
            }

            

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            DrawBackground();
            m_currentState.render(gameTime); // Draw the current game state including gameplay 

            // TODO: Add your drawing code here
            base.Draw(gameTime);
        }

        private void DrawBackground()
        {
            _spriteBatch.Begin(SpriteSortMode.Deferred);
            _spriteBatch.Draw(m_backgroundImage, m_background, Color.White);
            _spriteBatch.End();
        }





        


    }
}
