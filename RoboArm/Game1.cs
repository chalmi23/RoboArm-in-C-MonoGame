using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RoboArm
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private BasicEffect basicEffect, basicEffect2, basicEffect3, basicEffect4;        //Każde ramię osobne
        private Texture2D metal1Texture, metal2Texture, metal3Texture;        //Tekstury ramion

        private VertexPositionTexture[] vertices; // Wierzchołki pierwszego graniastosłupa
        private VertexPositionTexture[] vertices2; // Wierzchołki drugiego graniastosłupa
        private VertexPositionTexture[] vertices3; // Wierzchołki drugiego graniastosłupa
        private VertexPositionTexture[] vertices4; // Wierzchołki drugiego graniastosłupa

        private Matrix translationMatrix = Matrix.CreateTranslation(new Vector3(4.0f, 0.0f, 0.0f)); // Przesunięcie na pozycję drugiego graniastosłupa
        private Matrix world, view, proj;       //Każde ramię osobny wymiar
        private Matrix world2, view2, proj2;
        private Matrix world3, view3, proj3;
        private Matrix world4, view4, proj4;

        private int[] indices; // Indeksy wierzchołków graniastosłupów

        private float angleX = 0.0f, angleY = 0.0f; //Zmienne do kamery
        private float zoom = 1.0f;
        private float zoomSpeed = 0.02f;

        private float firstArmUpDown = 0.0f;        //Zmienna pierwszego ramienia

        private float secondArmRotation = 0.0f;     //Zmienne drugiego ramienia
        private float secondArmUpDown = 0.0f;

        private float thirdArmUpDown = 0.05f;       //Zmienna szczypiec

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            this.Window.AllowUserResizing = true;
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = 1400;
            _graphics.PreferredBackBufferHeight = 1000;
            _graphics.ApplyChanges();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            metal1Texture = Content.Load<Texture2D>("metal1");      //Załadowanie tekstur
            metal2Texture = Content.Load<Texture2D>("metal2");
            metal3Texture = Content.Load<Texture2D>("metal3");

            indices = new int[]   // Indeksy wierzchołków graniastosłupów
            {
            // Podstawa
            0, 1, 2,
            1, 3, 2,

            // Czworokąt górny
            4, 5, 6,
            5, 7, 6,

            // Boki
            0, 4, 1,
            1, 4, 5,
            1, 5, 3,
            3, 5, 7,
            3, 7, 2,
            2, 7, 6,
            2, 6, 0,
            0, 6, 4,
            };

            vertices = new VertexPositionTexture[]      //Pierwsze ramię
            {
            // Podstawa pierwszego graniastosłupa
            new VertexPositionTexture(new Vector3(0.0f, -0.25f, -0.25f), new Vector2(0, 1)),
            new VertexPositionTexture(new Vector3(4.0f, -0.25f, -0.25f), new Vector2(1, 1)),
            new VertexPositionTexture(new Vector3(0.0f, -0.25f, 0.25f), new Vector2(0, 0)),
            new VertexPositionTexture(new Vector3(4.0f, -0.25f, 0.25f), new Vector2(1, 0)),

            // Czworokąt górny pierwszego graniastosłupa
            new VertexPositionTexture(new Vector3(0.0f, 0.25f, -0.25f), new Vector2(0, 1)),
            new VertexPositionTexture(new Vector3(4.0f, 0.25f, -0.25f), new Vector2(1, 1)),
            new VertexPositionTexture(new Vector3(0.0f, 0.25f, 0.25f), new Vector2(0, 0)),
            new VertexPositionTexture(new Vector3(4.0f, 0.25f, 0.25f), new Vector2(1, 0)),

            // Boki - Wydłużone ściany pierwszego graniastosłupa
            new VertexPositionTexture(new Vector3(0.0f, -0.25f, -0.25f), new Vector2(0, 1)),
            new VertexPositionTexture(new Vector3(4.0f, -0.25f, -0.25f), new Vector2(1, 1)),
            new VertexPositionTexture(new Vector3(0.0f, 0.25f, -0.25f), new Vector2(0, 0)),
            new VertexPositionTexture(new Vector3(4.0f, 0.25f, 0.25f), new Vector2(1, 0)),
            };
            basicEffect = new BasicEffect(GraphicsDevice);
            basicEffect.TextureEnabled = true;
            basicEffect.Texture = metal1Texture;

            vertices2 = new VertexPositionTexture[] //Drugie ramię
            {
            new VertexPositionTexture(new Vector3(0.0f, -0.125f , -0.125f), new Vector2(0, 1)),
            new VertexPositionTexture(new Vector3(4.0f, -0.125f, -0.125f), new Vector2(1, 1)),
            new VertexPositionTexture(new Vector3(0.0f, -0.125f, 0.125f), new Vector2(0, 0)),
            new VertexPositionTexture(new Vector3(4.0f, -0.125f, 0.125f), new Vector2(1, 0)),

            new VertexPositionTexture(new Vector3(0.0f, 0.125f, -0.125f), new Vector2(0, 1)),
            new VertexPositionTexture(new Vector3(4.0f, 0.125f, -0.125f), new Vector2(1, 1)),
            new VertexPositionTexture(new Vector3(0.0f, 0.125f, 0.125f), new Vector2(0, 0)),
            new VertexPositionTexture(new Vector3(4.0f, 0.125f, 0.125f), new Vector2(1, 0)),

            new VertexPositionTexture(new Vector3(0.0f, -0.125f, -0.125f), new Vector2(0, 1)),
            new VertexPositionTexture(new Vector3(4.0f, -0.125f, -0.125f), new Vector2(1, 1)),
            new VertexPositionTexture(new Vector3(0.0f, 0.125f, -0.25f), new Vector2(0, 0)),
            new VertexPositionTexture(new Vector3(4.0f, 0.125f, 0.25f), new Vector2(1, 0)),
            };
            basicEffect2 = new BasicEffect(GraphicsDevice);
            basicEffect2.TextureEnabled = true;
            basicEffect2.Texture = metal2Texture;

            vertices3 = new VertexPositionTexture[]     //Pierwsze ramię szczypiec
            {
            new VertexPositionTexture(new Vector3(0.0f, -0.0200f, -0.0625f), new Vector2(0, 1)),
            new VertexPositionTexture(new Vector3(1.0f, -0.0625f, -0.0625f), new Vector2(1, 1)),
            new VertexPositionTexture(new Vector3(0.0f, -0.0625f, 0.0625f), new Vector2(0, 0)),
            new VertexPositionTexture(new Vector3(1.0f, -0.0625f, 0.0625f), new Vector2(1, 0)),

            new VertexPositionTexture(new Vector3(0.0f, 0.0625f, -0.0625f), new Vector2(0, 1)),
            new VertexPositionTexture(new Vector3(1.0f, 0.0625f, -0.0625f), new Vector2(1, 1)),
            new VertexPositionTexture(new Vector3(0.0f, 0.0625f, 0.0625f), new Vector2(0, 0)),
            new VertexPositionTexture(new Vector3(1.0f, 0.0625f, 0.0625f), new Vector2(1, 0)),

            new VertexPositionTexture(new Vector3(0.0f, -0.0625f, -0.0625f), new Vector2(0, 1)),
            new VertexPositionTexture(new Vector3(1.0f, -0.0625f, -0.0625f), new Vector2(1, 1)),
            new VertexPositionTexture(new Vector3(0.0f, 0.0625f, -0.0625f), new Vector2(0, 0)),
            new VertexPositionTexture(new Vector3(1.0f, 0.0625f, 0.0625f), new Vector2(1, 0)),
            };
            basicEffect3 = new BasicEffect(GraphicsDevice);
            basicEffect3.TextureEnabled = true;
            basicEffect3.Texture = metal3Texture;

            vertices4 = new VertexPositionTexture[]     //Drugie ramię szczypiec
            {
            new VertexPositionTexture(new Vector3(0.0f, -0.0625f, -0.0625f), new Vector2(0, 1)),
            new VertexPositionTexture(new Vector3(1.0f, -0.0625f, -0.0625f), new Vector2(1, 1)),
            new VertexPositionTexture(new Vector3(0.0f, -0.0625f, 0.0625f), new Vector2(0, 0)),
            new VertexPositionTexture(new Vector3(1.0f, -0.0625f, 0.0625f), new Vector2(1, 0)),

            new VertexPositionTexture(new Vector3(0.0f, 0.0625f, -0.0625f), new Vector2(0, 1)),
            new VertexPositionTexture(new Vector3(1.0f, 0.0625f, -0.0625f), new Vector2(1, 1)),
            new VertexPositionTexture(new Vector3(0.0f, 0.0625f, 0.0625f), new Vector2(0, 0)),
            new VertexPositionTexture(new Vector3(1.0f, 0.0625f, 0.0625f), new Vector2(1, 0)),

            new VertexPositionTexture(new Vector3(0.0f, -0.0625f, -0.0625f), new Vector2(0, 1)),
            new VertexPositionTexture(new Vector3(1.0f, -0.0625f, -0.0625f), new Vector2(1, 1)),
            new VertexPositionTexture(new Vector3(0.0f, 0.0625f, -0.0625f), new Vector2(0, 0)),
            new VertexPositionTexture(new Vector3(1.0f, 0.0625f, 0.0625f), new Vector2(1, 0)),
            };
            basicEffect4 = new BasicEffect(GraphicsDevice);
            basicEffect4.TextureEnabled = true;
            basicEffect4.Texture = metal3Texture;
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Right) || keyboardState.IsKeyDown(Keys.W)) angleY += 0.02f;        //Opcje kamery
            if (keyboardState.IsKeyDown(Keys.Left) || keyboardState.IsKeyDown(Keys.S)) angleY -= 0.02f;

            if (keyboardState.IsKeyDown(Keys.Up)) angleX += 0.02f;
            if (keyboardState.IsKeyDown(Keys.Down)) angleX -= 0.02f;

            if (keyboardState.IsKeyDown(Keys.Q)) zoom += zoomSpeed;

            if (keyboardState.IsKeyDown(Keys.A))
            {
                zoom -= zoomSpeed;
                if (zoom < 0.1f) zoom = 0.1f;
            }

            if (keyboardState.IsKeyDown(Keys.E)) firstArmUpDown += 0.02f;
            if (keyboardState.IsKeyDown(Keys.D)) firstArmUpDown -= 0.02f;

            if (keyboardState.IsKeyDown(Keys.R)) secondArmRotation += 0.02f;
            if (keyboardState.IsKeyDown(Keys.F)) secondArmRotation -= 0.02f;

            if (keyboardState.IsKeyDown(Keys.T) && secondArmUpDown < 1.55) secondArmUpDown += 0.02f;
            if (keyboardState.IsKeyDown(Keys.G) && secondArmUpDown > -1.55) secondArmUpDown -= 0.02f;

            if (keyboardState.IsKeyDown(Keys.Y) && thirdArmUpDown < 1.55 ) thirdArmUpDown += 0.02f;
            if (keyboardState.IsKeyDown(Keys.H) && thirdArmUpDown > 0.05) thirdArmUpDown -= 0.02f;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            world = Matrix.CreateTranslation(new Vector3(0.0f, 0.0f, 0.0f)); 
            world2 = Matrix.CreateTranslation(new Vector3(0.0f, 0.0f, 0.0f)); 
            world3 = Matrix.CreateTranslation(new Vector3(0.0f, 0.0f, 0.0f)); 
            world4 = Matrix.CreateTranslation(new Vector3(0.0f, 0.0f, 0.0f)); 

            view = Matrix.CreateLookAt(new Vector3(0.0f, 0.0f, 25.0f / zoom), Vector3.Zero, Vector3.Up);
            view2 = Matrix.CreateLookAt(new Vector3(0.0f, 0.0f, 25.0f / zoom), Vector3.Zero, Vector3.Up);
            view3 = Matrix.CreateLookAt(new Vector3(0.0f, 0.0f, 25.0f / zoom), Vector3.Zero, Vector3.Up);
            view4 = Matrix.CreateLookAt(new Vector3(0.0f, 0.0f, 25.0f / zoom), Vector3.Zero, Vector3.Up);

            view = Matrix.CreateRotationX(angleX) * Matrix.CreateRotationY(angleY) * view;
            view2 = Matrix.CreateRotationX(angleX) * Matrix.CreateRotationY(angleY) * view2;
            view3 = Matrix.CreateRotationX(angleX) * Matrix.CreateRotationY(angleY) * view3;
            view4 = Matrix.CreateRotationX(angleX) * Matrix.CreateRotationY(angleY) * view4;

            proj = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(50), GraphicsDevice.Viewport.AspectRatio, 0.01f, 1000.0f);
            proj2 = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(50), GraphicsDevice.Viewport.AspectRatio, 0.01f, 1000.0f);
            proj3 = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(50), GraphicsDevice.Viewport.AspectRatio, 0.01f, 1000.0f);
            proj4 = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(50), GraphicsDevice.Viewport.AspectRatio, 0.01f, 1000.0f);

            //Ustaw macierze dla E i D
            view = Matrix.CreateRotationZ(firstArmUpDown) * view; 
            view2 = Matrix.CreateRotationZ(firstArmUpDown) * view2; 
            view3 = Matrix.CreateRotationZ(firstArmUpDown) * view3; 
            view4 = Matrix.CreateRotationZ(firstArmUpDown) * view4;

            //Ustaw macierze dla R i F
            view2 = Matrix.CreateRotationX(secondArmRotation) * view2;
            view3 = Matrix.CreateRotationX(secondArmRotation) * view3;
            view4 = Matrix.CreateRotationX(secondArmRotation) * view4;

            //Ustaw macierze dla T i G
            view2 = Matrix.CreateRotationZ(secondArmUpDown) * translationMatrix * view2;
            view3 = Matrix.CreateRotationZ(secondArmUpDown) * translationMatrix * view3;
            view4 = Matrix.CreateRotationZ(secondArmUpDown) * translationMatrix * view4;

            //Ustaw macierze dla Y i H
            view3 = Matrix.CreateRotationY(thirdArmUpDown) * translationMatrix * view3;
            view4 = Matrix.CreateRotationY(-thirdArmUpDown) * translationMatrix * view4;

            DrawPrism(vertices, indices, basicEffect, view, proj, world);
            DrawPrism(vertices2, indices, basicEffect2, view2, proj2, world2);
            DrawPrism(vertices3, indices, basicEffect3, view3, proj3, world3); 
            DrawPrism(vertices4, indices, basicEffect4, view4, proj4, world4);

            RasterizerState rs = new RasterizerState();
            rs.CullMode = CullMode.None; 
            GraphicsDevice.RasterizerState = rs;

            base.Draw(gameTime);
        }

        private void DrawPrism(VertexPositionTexture[] vertices, int[] indices, BasicEffect effect, Matrix viewMatrix, Matrix projMatrix, Matrix world)
        {
            effect.World = world;
            effect.View = viewMatrix;
            effect.Projection = projMatrix;

            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                GraphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, vertices, 0, vertices.Length, indices, 0, indices.Length / 3);
            }
        }
    }
}
