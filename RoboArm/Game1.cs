using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Security.Cryptography;

namespace RoboArm
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private BasicEffect basicEffect;

        private Texture2D metal1Texture; // Tekstura metalu1
        private Texture2D metal2Texture; // Tekstura metalu2
        private Texture2D metal3Texture; // Tekstura metalu3

        private Matrix world, view, proj;

        private float zoom = 1.0f;
        private float zoomSpeed = 0.02f;

        private float time = 0; // Czas w grze
        private float angleX = 0.0f, angleY = 0.0f;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            this.Window.AllowUserResizing = true;
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = 1200;
            _graphics.PreferredBackBufferHeight = 1000;
            _graphics.ApplyChanges();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            metal1Texture = Content.Load<Texture2D>("metal1");
            metal2Texture = Content.Load<Texture2D>("metal2");
            metal3Texture = Content.Load<Texture2D>("metal3");

            basicEffect = new BasicEffect(GraphicsDevice);
            basicEffect.VertexColorEnabled = true;
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (keyboardState.IsKeyDown(Keys.Right)) angleY += 0.02f;
            if (keyboardState.IsKeyDown(Keys.Left)) angleY -= 0.02f;

            if (keyboardState.IsKeyDown(Keys.Up)) angleX += 0.02f;
            if (keyboardState.IsKeyDown(Keys.Down)) angleX -= 0.02f;

            if (keyboardState.IsKeyDown(Keys.Q)) zoom += zoomSpeed;

            world = Matrix.Identity;
            view = Matrix.CreateLookAt(new Vector3(0.0f, 0.0f, 6.0f / zoom), Vector3.Zero, Vector3.Up);
            view = Matrix.CreateRotationX(angleX) * Matrix.CreateRotationY(angleY) * view;
            proj = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(50), GraphicsDevice.Viewport.AspectRatio, 0.01f, 1000.0f);

            basicEffect.World = world;
            basicEffect.View = view;
            basicEffect.Projection = proj;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}