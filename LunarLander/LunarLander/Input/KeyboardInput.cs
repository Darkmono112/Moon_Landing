using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using System.Diagnostics;
using System.Collections;


using static System.Text.Json.JsonSerializer; //todo remove
using System.Linq; //TODO remove 

namespace CS5410.Input
{
    /// <summary>
    /// Derived input device for the PC Keyboard
    /// </summary>
    public class KeyboardInput : IInputDevice
    {
        private Dictionary<Keys, CommandEntry> m_commandEntries = new Dictionary<Keys, CommandEntry>();
        public Dictionary<String, Keys> controlList = new Dictionary<String, Keys>();

        public void registerCommand(Keys key, bool keyPressOnly, IInputDevice.CommandDelegate callback, String commandName)
        {
            //
            // If already registered, remove it!
            if (m_commandEntries.ContainsKey(key))
            {
                m_commandEntries.Remove(key);
            }
            m_commandEntries.Add(key, new CommandEntry(key, keyPressOnly, callback));
            if (!controlList.ContainsKey(commandName))
            {
                controlList.Add(commandName, key);
            }
        }

        public void ChangeKey(Keys newKey,bool keyPressOnly, IInputDevice.CommandDelegate callback, String commandName)
        {
            

            if (m_commandEntries.ContainsKey(newKey))
            {
                return;
            }
            m_commandEntries.Add(newKey, new CommandEntry(newKey, keyPressOnly, callback));

            m_commandEntries.Remove(controlList[commandName]); // Removed the old controls 
            controlList.Remove(commandName); // removed saved control
            
            controlList.Add(commandName, newKey); // adds new control to the list 
        }
 
        private struct CommandEntry
        {
            public CommandEntry(Keys key, bool keyPressOnly, IInputDevice.CommandDelegate callback)
            {
                this.key = key;
                this.keyPressOnly = keyPressOnly;
                this.callback = callback;
            }

            public Keys key;
            public bool keyPressOnly;
            public IInputDevice.CommandDelegate callback;
        }

        /// <summary>
        /// Goes through all the registered commands and invokes the callbacks if they
        /// are active.
        /// </summary>
        public void Update(GameTime gameTime)
        {
            KeyboardState state = Keyboard.GetState();
            foreach (CommandEntry entry in this.m_commandEntries.Values)
            {
                if (entry.keyPressOnly && keyPressed(entry.key))
                {
                    entry.callback(gameTime, 1.0f);
                }
                else if (!entry.keyPressOnly && state.IsKeyDown(entry.key))
                {
                    entry.callback(gameTime, 1.0f);
                }
            }

            //
            // Move the current state to the previous state for the next time around
            m_statePrevious = state;
        }

        private KeyboardState m_statePrevious;

        /// <summary>
        /// Checks to see if a key was newly pressed
        /// </summary>
        private bool keyPressed(Keys key)
        {
            return (Keyboard.GetState().IsKeyDown(key) && !m_statePrevious.IsKeyDown(key));
        }
    }
}
