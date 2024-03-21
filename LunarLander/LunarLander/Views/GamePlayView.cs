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
        private const string MESSAGE = "Are Safe";
        private const string MESSAGE2 = "Crash";
        // Keyboard for controls while in gameview
        private KeyboardInput m_inputKeyboard;
        private GraphicsDeviceManager _graphics;


        private BasicEffect _basicEffect;

        private Terrain terrain;
        private Lander lander;

        private bool crash;
        private bool crash2;
        public GamePlayView(KeyboardInput keyboard, GraphicsDeviceManager graphics, BasicEffect basicEffect, Lander lander)
        {

            m_inputKeyboard = keyboard;
            _graphics = graphics;
            _basicEffect = basicEffect;
            this.lander = lander;


            //Use defualt controls for now 
            // Replace with a loaded Dictionary, itterate through it for input




        }

        public override void loadContent(ContentManager contentManager)
        {
            m_font = contentManager.Load<SpriteFont>("Fonts/menuStandard");
            // load the sprites and stuff here. 
            terrain = new Terrain(1920, 1080);



        }


        public override GameStateEnum processInput(GameTime gameTime)
        {
            
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                resetTerrain();
                crash = false;
                crash2 = false;
                lander.resetPositon();
                return GameStateEnum.MainMenu;
            }

            m_inputKeyboard.Update(gameTime);

            return GameStateEnum.GamePlay;
        }


        private void resetTerrain()
        {
            terrain = new Terrain(1920, 1080);
        }

        public override void update(GameTime gameTime)
        {
            // Game stuff here 
            //move position to be thrust - gravity or something

            m_inputKeyboard.Update(gameTime);
            if(!crash && !crash2)
            {
                lander.applyGrav();
            }
            
            crash = lander.colision(terrain.displace.displaceList, terrain.displace.displaceListSafe);
            crash2 = lander.colision2(terrain.displace.displaceList, terrain.displace.displaceListSafe);


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
                    _indexStrip, 0, _indexStrip.Length -2);
            }

            m_spriteBatch.Begin();
            m_spriteBatch.Draw(lander.texture, lander.hitbox, Color.White);

            if (crash)
            {
                Vector2 stringSize = m_font.MeasureString(MESSAGE);
                m_spriteBatch.DrawString(m_font, MESSAGE,
                    new Vector2(m_graphics.PreferredBackBufferWidth / 2 - stringSize.X / 2, m_graphics.PreferredBackBufferHeight / 2 - stringSize.Y), Color.Yellow);
            }
            if (crash2)
            {
                Vector2 stringSize = m_font.MeasureString(MESSAGE2);
                m_spriteBatch.DrawString(m_font, MESSAGE2,
                    new Vector2(m_graphics.PreferredBackBufferWidth / 2 - stringSize.X / 2, m_graphics.PreferredBackBufferHeight / 2 - stringSize.Y), Color.Yellow);
            }


            m_spriteBatch.End();



        }





        //TODO FIGURE THIS OUT 
    

    }
}
