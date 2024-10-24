﻿using CryStal.Engine;
using CryStal.Engine.Models;
using CryStal.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using nkast.Aether.Physics2D.Collision.Shapes;
using nkast.Aether.Physics2D.Dynamics;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace CryStal
{
    public class Game1 : Game
    {
        //screen information
        public static int SCREENWIDTH = 1600;
        public static int SCREENHEIGHT = 900;

        //tile information
        public const int SCALE = 3;
        public const int TILESIZE = 16 * SCALE;
        public const float INVERCETILESIZE = 0.02083333333333333333f; // 1/tileSIze

        //Aether2d
        const float _timeStep = 1.0f / 60.0f;
        const int _subStepCount = 4;
        SolverIterations _solverIterations;
        World _world;

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Effect _paletteSwap;
        private Effect _lightingShader;

        public Player player;
        public Camera camera;
        public PointLight light1 = new PointLight(new Vector2(0,0), 1.0f, 0.0014f, 0.000007f, new Vector3(0.05f), new Vector3(0.8f), new Vector3(1f));

        List<GameObject> tempObj = new();

        //debug stuff
        Stopwatch simulationTimer = new();
        Stopwatch drawTimer = new();

        private long simulationTime = 1;

        //Default Font
        public static SpriteFont Arial;

        //delegates
        public static Action OnUpdate;
        public delegate void DrawAction(SpriteBatch spriteBatch, Camera camera);
        public static DrawAction OnDefaultDraw;
        public delegate void EffectDrawAction(SpriteBatch spriteBatch, Camera camera, Effect effect);
        public static EffectDrawAction OnPaletteDraw;
        public static EffectDrawAction OnLightingDraw;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferHeight = SCREENHEIGHT;
            _graphics.PreferredBackBufferWidth = SCREENWIDTH;
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            _solverIterations.VelocityIterations = 4;
            _solverIterations.PositionIterations = 4;
            _solverIterations.TOIPositionIterations = 4;
            _solverIterations.TOIVelocityIterations = 4;
        }

        protected override void Initialize()
        {
            _graphics.ApplyChanges();

            player = new Player(_world, new Vector2(TILESIZE, TILESIZE), 460);
            camera = new Camera(new Vector2(0, 0), new Vector2(SCREENWIDTH, SCREENHEIGHT));

            LevelHandler.InitializeLevel(_world, player, Services);

            OnPaletteDraw += player.Draw;
            OnDefaultDraw += LevelHandler.DrawLevel;
            OnLightingDraw += LevelHandler.DrawLevelLighting;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            Arial = Content.Load<SpriteFont>("Arial");
            player.Texture = Content.Load<Texture2D>("Player");
            player.Palette = Content.Load<Texture2D>("PlayerPalette");
            _paletteSwap = Content.Load<Effect>("Shaders\\PaletteSwap");
            _lightingShader = Content.Load<Effect>("Shaders\\LightingShader");
            _lightingShader.Parameters["WorldSize"].SetValue(new Vector2(SCREENWIDTH, SCREENHEIGHT));

            LevelHandler.LoadLevel("Demo");

            simulationTimer.Start();
            drawTimer.Start();
        }
        protected override void Update(GameTime gameTime)
        {
            checkGlobalKeys(Keyboard.GetState(), gameTime);

            simulationTimer.Restart();
            //Physics.Update(gameTime, _graphics.GraphicsDevice);
            _world.Step(_timeStep);
            simulationTime = simulationTimer.ElapsedMilliseconds;
            simulationTimer.Stop();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            //Lighting Draw Call
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp, effect: _lightingShader);

            LevelHandler.DrawBackground(_spriteBatch, camera);

            _lightingShader.Parameters["CameraPos"].SetValue(camera.getLocation());
            light1.Position = player.Position;
            light1.Use(_lightingShader);

            OnDefaultDraw(_spriteBatch, camera);
            OnLightingDraw(_spriteBatch, camera, _lightingShader);

            _spriteBatch.End();

            //Default draw call
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            //OnDefaultDraw(_spriteBatch, camera);

            DrawDebugTimer();
            drawTimer.Restart();
            player.DrawDebug(_spriteBatch);

            _spriteBatch.End();

            //Palette Draw Call
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp, effect: _paletteSwap);
            OnPaletteDraw(_spriteBatch, camera, _paletteSwap);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
        private void DrawDebugTimer()
        {
            //old engine
            _spriteBatch.DrawString(Arial, $"Simulation Time: {simulationTime}ms", new Vector2(4, 0), Color.WhiteSmoke);
            _spriteBatch.DrawString(Arial, $"Draw Time: {drawTimer.ElapsedMilliseconds}ms", new Vector2(4, 32), Color.WhiteSmoke);
        }

        bool spawned = false;
        bool stepped = false;
        private void checkGlobalKeys(KeyboardState keyboard, GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (keyboard.IsKeyDown(Keys.Enter) && !spawned)
            {
                player.Unload(_world);
                LevelHandler.LoadLevel("Demo2");
                spawned = true;
            }
            else if (keyboard.IsKeyUp(Keys.Enter) && spawned)
            {
                player.Load(_world);
                player.SetAlive();
                LevelHandler.LoadLevel("Demo");
                spawned = false;
            }
            if (keyboard.IsKeyDown(Keys.Up))
            {
                camera.Y -= 460 * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            if (keyboard.IsKeyDown(Keys.Down))
            {
                camera.Y += 460 * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            if (keyboard.IsKeyDown(Keys.Left))
            {
                camera.X -= 460 * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            if (keyboard.IsKeyDown(Keys.Right))
            {
                camera.X += 460 * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
        }
    }
}