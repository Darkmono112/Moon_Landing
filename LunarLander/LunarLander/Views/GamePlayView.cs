using CS5410.Input;
using LunarLander;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;

//NOTES TO SELF: 
// 1. Add in high scores
// 2. Add in controls
// 3. Add in lunar lander 
// 4. Add in hitbox 
// Make Terrain 
// Make terrain Generation 
// Make terrain draw 
// Add safe zone for moon landing

namespace CS5410
{
    public class GamePlayView : GameStateView
    {
        private SpriteFont m_font;
        // Keyboard for controls while in gameview
        private Input.KeyboardInput m_inputKeyboard;
        private Texture2D m_landerTexture;
        private GraphicsDeviceManager _graphics;

        private BasicEffect _basicEffect;

        private Terrain terrain;

        public GamePlayView(Input.KeyboardInput keyboard, GraphicsDeviceManager graphics, BasicEffect basicEffect) {

            m_inputKeyboard = keyboard;
            _graphics = graphics;
            _basicEffect = basicEffect;
            //Use defualt controls for now 
            // Replace with a loaded Dictionary, itterate through it for input
            m_inputKeyboard.registerCommand(Keys.W, false, new IInputDevice.CommandDelegate(onMoveUp));
            m_inputKeyboard.registerCommand(Keys.S, false, new IInputDevice.CommandDelegate(onMoveDown));
            m_inputKeyboard.registerCommand(Keys.A, false, new IInputDevice.CommandDelegate(onRotateLeft));
            m_inputKeyboard.registerCommand(Keys.D, false, new IInputDevice.CommandDelegate(onRotateRight));

            

        }

        public override void loadContent(ContentManager contentManager)
        {
            m_font = contentManager.Load<SpriteFont>("Fonts/menuStandard");
            // load the sprites and stuff here. 
            m_landerTexture = contentManager.Load<Texture2D>("Images/LanderSprite");

            terrain = new Terrain(1920, 1080);


        }


        public override GameStateEnum processInput(GameTime gameTime)
        {
            
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                return GameStateEnum.MainMenu;
            }

            m_inputKeyboard.Update(gameTime);

            return GameStateEnum.GamePlay;
        }

        public override void update(GameTime gameTime)
        {
            // Game stuff here 
            //move position to be thrust - gravity or something


        }

        public override void render(GameTime gameTime)
        {
            VertexPositionColor[] _strip = terrain.getStrip();
           
            
            int[] _indexStrip = terrain.getIndex();

           

            //Do the game stuff here
            foreach (EffectPass pass in _basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                m_graphics.GraphicsDevice.DrawUserIndexedPrimitives(
                    PrimitiveType.TriangleStrip,
                    _strip, 0, _strip.Length,
                    _indexStrip, 0, 3);
            }



        }

      



        //TODO FIGURE THIS OUT 
        #region Input Handlers
        /// <summary>
        /// The various moveX methods subtract half of the height/width because the rendering is performed
        /// from the center of the rectangle because it can rotate
        /// </summary>
        private void onMoveUp(GameTime gameTime, float scale)
        {
           
        }

        private void onMoveDown(GameTime gameTime, float scale)
        {
           
        }

        private void onRotateLeft(GameTime gameTime, float scale)
        {
            
        }

        private void onRotateRight(GameTime gameTime, float scale)
        {
            
        }

        #endregion


    }
}
