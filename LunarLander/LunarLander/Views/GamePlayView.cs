using CS5410.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

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

        public GamePlayView(Input.KeyboardInput keyboard) {

            m_inputKeyboard = keyboard;

            //Use defualt controls for now 
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
            m_spriteBatch.Begin();

            //Do the game stuff here


            m_spriteBatch.End();
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
