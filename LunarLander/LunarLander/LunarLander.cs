using CS5410;
using CS5410.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;


using System.IO;
using System.IO.IsolatedStorage;
using System.Runtime.Serialization.Json;
using Microsoft.Xna.Framework.Audio;


namespace LunarLander
{
    public class LunarLander : Game
    {
        private GraphicsDeviceManager _graphics;
        private BasicEffect _basicEffect;

        private SpriteBatch _spriteBatch;
        private IGameState m_currentState;
        private Dictionary<GameStateEnum, IGameState> _states;

        private Rectangle m_background;
        private Texture2D m_backgroundImage;
        private Lander lander;
        private Texture2D landerSprite;

        private SoundEffect m_rocketBoost;

        
        private Song song;
        private bool playingSong = false;

        private bool saving = false;
        private bool loading = false;

        // The Keyboard will be handeled here, but it's input will be handled in the game view??
        private KeyboardInput m_inputKeyboard;

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
            lander = new Lander();

            _graphics.GraphicsDevice.RasterizerState = new RasterizerState
            {
                FillMode = FillMode.Solid,
                CullMode = CullMode.CullCounterClockwiseFace,   // CullMode.None If you want to not worry about triangle winding order
                MultiSampleAntiAlias = true,
            };

            _basicEffect = new BasicEffect(_graphics.GraphicsDevice)
            {
                VertexColorEnabled = true,
                View = Matrix.CreateLookAt(new Vector3(0, 0, 1), Vector3.Zero, Vector3.Up),

                Projection = Matrix.CreateOrthographicOffCenter(
                    0, _graphics.GraphicsDevice.Viewport.Width,
                    _graphics.GraphicsDevice.Viewport.Height, 0,   // doing this to get it to match the default of upper left of (0, 0)
                    0.1f, 2)
            };

            loadKeyboard(); // Loads the correct keyboard with controls
            if(m_inputKeyboard == null)
            { //Load Defualt controls 
                m_inputKeyboard = new KeyboardInput();
                m_inputKeyboard.registerCommand(Keys.W, false, new IInputDevice.CommandDelegate(onMoveUp), "boost");
                m_inputKeyboard.registerCommand(Keys.A, false, new IInputDevice.CommandDelegate(onRotateLeft), "rotateLeft");
                m_inputKeyboard.registerCommand(Keys.D, false, new IInputDevice.CommandDelegate(onRotateRight), "rotateRight");
            }

            _states = new Dictionary<GameStateEnum, IGameState>
            {
                { GameStateEnum.MainMenu, new MainMenuView() },
                { GameStateEnum.GamePlay, new GamePlayView(m_inputKeyboard, _graphics, _basicEffect, lander) },
                { GameStateEnum.HighScores, new HighScoresView() },
                { GameStateEnum.Help, new HelpView() },
                { GameStateEnum.Controls, new ControlsView(m_inputKeyboard , lander) },
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
            landerSprite = this.Content.Load<Texture2D>("Images/landerSprite");
            lander.texture = landerSprite;

            m_backgroundImage = this.Content.Load<Texture2D>("Images/LunarBackground");
            song = this.Content.Load<Song>("Audio/music");

            m_rocketBoost = this.Content.Load<SoundEffect>("Audio/mixkit-rocket-ignition-flames-1725");
            lander.boost = m_rocketBoost;

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

            if (!playingSong)
            {
                MediaPlayer.Play(song);
                MediaPlayer.IsRepeating = true; // TODO maybe remove this
                playingSong = true;
            }

            m_inputKeyboard.Update(gameTime);

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

        private void loadKeyboard()
        {
            lock (this)
            {
                if (!this.loading)
                {
                    this.loading = true;
                    // Yes, I know the result is not being saved, I dont' need it
                    var result = finalizeLoadAsync();
                    result.Wait();
                    

                }
            }
        }
        private async Task finalizeLoadAsync()
        {
            await Task.Run(() =>
            {
                using (IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    try
                    {
                        if (storage.FileExists("Controls.json"))
                        {
                            using (IsolatedStorageFileStream fs = storage.OpenFile("Controls.json", FileMode.Open))
                            {
                                if (fs != null)
                                {
                                    DataContractJsonSerializer mySerializer = new DataContractJsonSerializer(typeof(KeyboardInput));
                                    m_inputKeyboard = (KeyboardInput)mySerializer.ReadObject(fs);
                                }
                            }
                            m_inputKeyboard.registerCommand(m_inputKeyboard.controlList["boost"], false, new IInputDevice.CommandDelegate(onMoveUp), "boost");
                            m_inputKeyboard.registerCommand(m_inputKeyboard.controlList["rotateLeft"], false, new IInputDevice.CommandDelegate(onRotateLeft), "rotateLeft");
                            m_inputKeyboard.registerCommand(m_inputKeyboard.controlList["rotateRight"], false, new IInputDevice.CommandDelegate(onRotateRight), "rotateRight");

                        }
                        else
                        {
                            m_inputKeyboard = null; // Make the defualt just in case
                        }
                    }
                    catch (IsolatedStorageException)
                    {
                        // Ideally show something to the user, but this is demo code :)
                    }
                }

                this.loading = false;
            });
        }


        #region Input Handlers
        private void onMoveUp(GameTime gameTime, float scale)
        {
            lander.moveUP();
        }

        private void onRotateLeft(GameTime gameTime, float scale)
        {
            lander.rotateLeft();
        }

        private void onRotateRight(GameTime gameTime, float scale)
        {
            lander.rotateRight();
        }

        #endregion









    }
}
