using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO.IsolatedStorage;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;

using System.IO;
using LunarLander;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;
using System;
using CS5410.Input;


namespace CS5410
{
    public class ControlsView : GameStateView
    {
        private SpriteFont m_font;

        private const string MESSAGE = "Changing control, press desired Key";
        private SpriteFont m_fontMenu;
        private SpriteFont m_fontMenuSelect;



        private enum Control
        {
            Boost,
            RotateLeft,
            RotateRight
        }





        // Keyboard needed to change controls 
        // TODO adjust a dictionary of controls, then register the new key in the old keys place 
        // TODO de-register old key 
        private Input.KeyboardInput keyboard;
        private bool m_waitForKeyRelease = true;
        private Control _currentSelection = Control.Boost;

        private bool saving = false;
        private bool loading = false;
        private Lander lander;

        private bool updatingBoost;
        private bool updatingRL;
        private bool updatingRR;
        private bool lockNav;
     

        public ControlsView(Input.KeyboardInput keys, Lander lander)
        {
            keyboard = keys;
            updatingBoost = false;
            updatingRL = false;
            updatingRR = false;
        }

        public override void loadContent(ContentManager contentManager)
        {
            m_font = contentManager.Load<SpriteFont>("Fonts/menuStandard");
            m_fontMenu = contentManager.Load<SpriteFont>("Fonts/menuStandard");
            m_fontMenuSelect = contentManager.Load<SpriteFont>("Fonts/menuSelected");

        }

        public override GameStateEnum processInput(GameTime gameTime)
        {
            


            if (!m_waitForKeyRelease)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Down) && !lockNav)
                {
                    if (_currentSelection != Control.RotateRight)
                    {
                        _currentSelection = _currentSelection + 1;
                    }
                    m_waitForKeyRelease = true;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.Up) && !lockNav)
                {
                    if (_currentSelection != Control.Boost)
                    {
                        _currentSelection = _currentSelection - 1;
                    }
                    m_waitForKeyRelease = true;
                }

                if (updatingBoost && Keyboard.GetState().GetPressedKeyCount() > 0)
                {
                    Keys newKey;

                    newKey = Keyboard.GetState().GetPressedKeys()[0];

                    keyboard.ChangeKey(newKey, false, new IInputDevice.CommandDelegate(onMoveUp), "boost");
                    updatingBoost = false;
                    lockNav = false;
                    saveControls();


                }
                if (updatingRL && Keyboard.GetState().GetPressedKeyCount() > 0)
                {
                    Keys newKey;

                    newKey = Keyboard.GetState().GetPressedKeys()[0];

                    keyboard.ChangeKey(newKey, false, new IInputDevice.CommandDelegate(onRotateLeft), "rotateLeft");
                    updatingRL = false;
                    lockNav = false;
                    saveControls();

                }
                if (updatingRR && Keyboard.GetState().GetPressedKeyCount() > 0)
                {
                    Keys newKey;

                    newKey = Keyboard.GetState().GetPressedKeys()[0];

                    keyboard.ChangeKey(newKey, false, new IInputDevice.CommandDelegate(onRotateRight), "rotateRight");
                    updatingRR = false;
                    lockNav = false;
                    saveControls();
                }

                // Arrow keys to navigate the menu
                

                // If enter is pressed, return the appropriate new state
                if (Keyboard.GetState().IsKeyDown(Keys.Enter) && _currentSelection == Control.Boost)
                {
                    // Find the key associated with Boosting and replace it 
                    updatingBoost = true;
                    lockNav = true;

                }
                if (Keyboard.GetState().IsKeyDown(Keys.Enter) && _currentSelection == Control.RotateLeft)
                {
                    updatingRL = true;
                    lockNav = true;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.Enter) && _currentSelection == Control.RotateRight)
                {
                    updatingRR = true;
                    lockNav = true;
                }

              

               
            }
            else if (Keyboard.GetState().IsKeyUp(Keys.Down) && Keyboard.GetState().IsKeyUp(Keys.Up) && Keyboard.GetState().IsKeyUp(Keys.Enter))
            {
                m_waitForKeyRelease = false;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                return GameStateEnum.MainMenu;
            }






            return GameStateEnum.Controls;
        }
        public override void update(GameTime gameTime)
        {
        }

        public override void render(GameTime gameTime)
        {


            m_spriteBatch.Begin();

            // I split the first one's parameters on separate lines to help you see them better
            float bottom = drawMenuItem(
                _currentSelection == Control.Boost ? m_fontMenuSelect : m_fontMenu,
                "Boost " + keyboard.controlList["boost"].ToString(),
                200,
                _currentSelection == Control.Boost ? Color.Yellow : Color.Blue);
            bottom = drawMenuItem(_currentSelection == Control.RotateLeft ? m_fontMenuSelect : m_fontMenu, "Rotate Left " + keyboard.controlList["rotateLeft"].ToString(),
                bottom, _currentSelection == Control.RotateLeft ? Color.Yellow : Color.Blue);
            drawMenuItem(_currentSelection == Control.RotateRight ? m_fontMenuSelect : m_fontMenu, "Rotate Right " + keyboard.controlList["rotateRight"].ToString(), bottom,
                _currentSelection == Control.RotateRight ? Color.Yellow : Color.Blue);

            if (updatingBoost || updatingRL || updatingRR)
            {
                Vector2 stringSize = m_font.MeasureString(MESSAGE);
                m_spriteBatch.DrawString(m_font, MESSAGE,
                    new Vector2(m_graphics.PreferredBackBufferWidth / 2 - stringSize.X / 2, m_graphics.PreferredBackBufferHeight / 2 - stringSize.Y), Color.Yellow);
            }

            m_spriteBatch.End();
        }

        private float drawMenuItem(SpriteFont font, string text, float y, Color color)
        {
            Vector2 stringSize = font.MeasureString(text);
            m_spriteBatch.DrawString(
                font,
                text,
                new Vector2(m_graphics.PreferredBackBufferWidth / 2 - stringSize.X / 2, y),
                color);

            return y + stringSize.Y;
        }


        private void saveControls()
        {
            lock (this)
            {
                if (!this.saving)
                {
                    this.saving = true;

                   

                    // Yes, I know the result is not being saved, I dont' need it
                    finalizeSaveAsync(keyboard);
                }
            }
        }

        private async Task finalizeSaveAsync(KeyboardInput state)
        {
            await Task.Run(() =>
            {
                using (IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    try
                    {
                        using (IsolatedStorageFileStream fs = storage.OpenFile("Controls.json", FileMode.Create))
                        {
                            if (fs != null)
                            {
                                DataContractJsonSerializer mySerializer = new DataContractJsonSerializer(typeof(KeyboardInput));
                                mySerializer.WriteObject(fs, state);
                            }
                        }
                    }
                    catch (IsolatedStorageException)
                    {
                        // Ideally show something to the user, but this is demo code :)
                    }
                }

                this.saving = false;
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
