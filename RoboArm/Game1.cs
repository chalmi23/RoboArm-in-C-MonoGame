using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RoboArm
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private BasicEffect basicEffect;
        private BasicEffect basicEffect2;
        private BasicEffect basicEffect3;
        private BasicEffect basicEffect4;

        private int[] indices; // Indeksy wierzchołków pierwszego graniastosłupa
        private Texture2D metal1Texture;
        private Texture2D metal2Texture;
        private Texture2D metal3Texture;

        private VertexPositionTexture[] vertices; // Wierzchołki pierwszego graniastosłupa
        private VertexPositionTexture[] vertices2; // Wierzchołki drugiego graniastosłupa
        private VertexPositionTexture[] vertices3; // Wierzchołki drugiego graniastosłupa
        private VertexPositionTexture[] vertices4; // Wierzchołki drugiego graniastosłupa

        private Matrix world, view, proj;
        private Matrix world2, view2, proj2;
        private Matrix world3, view3, proj3;
        private Matrix world4, view4, proj4;

        bool isTG = false;

        private float angleX = 0.0f, angleY = 0.0f;

        private float zoom = 1.0f;
        private float zoomSpeed = 0.02f;

        private float firstArmUpDown = 0.0f;

        private float secondArmRotation = 0.0f;
        private float secondArmUpDown = 0.0f;

        private float thirdArmUpDown = 0.05f;

        private float scaleFactor = 0.5f; // Wartość skali dla drugiego graniastosłupa

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
            basicEffect.TextureEnabled = true;
            basicEffect.Texture = metal1Texture;

            // Definiuj indeksy wierzchołków pierwszego graniastosłupa
            indices = new int[]
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

            vertices = new VertexPositionTexture[]
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

            basicEffect2 = new BasicEffect(GraphicsDevice);
            basicEffect2.TextureEnabled = true;
            basicEffect2.Texture = metal2Texture;

            vertices2 = new VertexPositionTexture[]
            {
            // Podstawa drugiego graniastosłupa (zaczyna się na końcu pierwszego)
            new VertexPositionTexture(new Vector3(0.0f, -0.25f * scaleFactor, -0.25f* scaleFactor), new Vector2(0, 1)),
            new VertexPositionTexture(new Vector3(4.0f, -0.25f* scaleFactor, -0.25f* scaleFactor), new Vector2(1, 1)),
            new VertexPositionTexture(new Vector3(0.0f, -0.25f* scaleFactor, 0.25f* scaleFactor), new Vector2(0, 0)),
            new VertexPositionTexture(new Vector3(4.0f, -0.25f* scaleFactor, 0.25f* scaleFactor), new Vector2(1, 0)),

            // Czworokąt górny drugiego graniastosłupa (zaczyna się na końcu pierwszego)
            new VertexPositionTexture(new Vector3(0.0f, 0.25f * scaleFactor, -0.25f* scaleFactor), new Vector2(0, 1)),
            new VertexPositionTexture(new Vector3(4.0f, 0.25f * scaleFactor, -0.25f* scaleFactor), new Vector2(1, 1)),
            new VertexPositionTexture(new Vector3(0.0f, 0.25f * scaleFactor, 0.25f* scaleFactor), new Vector2(0, 0)),
            new VertexPositionTexture(new Vector3(4.0f, 0.25f * scaleFactor, 0.25f* scaleFactor), new Vector2(1, 0)),

            // Boki - Wydłużone ściany drugiego graniastosłupa (zaczyna się na końcu pierwszego)
            new VertexPositionTexture(new Vector3(0.0f, -0.25f, -0.25f), new Vector2(0, 1)),
            new VertexPositionTexture(new Vector3(4.0f, -0.25f, -0.25f), new Vector2(1, 1)),
            new VertexPositionTexture(new Vector3(0.0f, 0.25f * scaleFactor, -0.25f), new Vector2(0, 0)),
            new VertexPositionTexture(new Vector3(4.0f, 0.25f * scaleFactor, 0.25f), new Vector2(1, 0)),
            };

            basicEffect3 = new BasicEffect(GraphicsDevice);
            basicEffect3.TextureEnabled = true;
            basicEffect3.Texture = metal3Texture;

            // Wierzchołki pierwszej połówki graniastosłupa
            vertices3 = new VertexPositionTexture[]
            {
            // Podstawa pierwszej połówki graniastosłupa
            new VertexPositionTexture(new Vector3(0.0f, -0.0200f, -0.0625f), new Vector2(0, 1)),
            new VertexPositionTexture(new Vector3(1.0f, -0.0625f, -0.0625f), new Vector2(1, 1)),
            new VertexPositionTexture(new Vector3(0.0f, -0.0625f, 0.0625f), new Vector2(0, 0)),
            new VertexPositionTexture(new Vector3(1.0f, -0.0625f, 0.0625f), new Vector2(1, 0)),

            // Czworokąt górny pierwszej połówki graniastosłupa
            new VertexPositionTexture(new Vector3(0.0f, 0.0625f, -0.0625f), new Vector2(0, 1)),
            new VertexPositionTexture(new Vector3(1.0f, 0.0625f, -0.0625f), new Vector2(1, 1)),
            new VertexPositionTexture(new Vector3(0.0f, 0.0625f, 0.0625f), new Vector2(0, 0)),
            new VertexPositionTexture(new Vector3(1.0f, 0.0625f, 0.0625f), new Vector2(1, 0)),

            // Boki - Wydłużone ściany pierwszej połówki graniastosłupa
            new VertexPositionTexture(new Vector3(0.0f, -0.0625f, -0.0625f), new Vector2(0, 1)),
            new VertexPositionTexture(new Vector3(1.0f, -0.0625f, -0.0625f), new Vector2(1, 1)),
            new VertexPositionTexture(new Vector3(0.0f, 0.0625f, -0.0625f), new Vector2(0, 0)),
            new VertexPositionTexture(new Vector3(1.0f, 0.0625f, 0.0625f), new Vector2(1, 0)),
            };

            // Wierzchołki drugiej połówki graniastosłupa
            basicEffect4 = new BasicEffect(GraphicsDevice);
            basicEffect4.TextureEnabled = true;
            basicEffect4.Texture = metal3Texture;

            // Wierzchołki drugiej połówki graniastosłupa
            vertices4 = new VertexPositionTexture[]
            {
            // Podstawa pierwszej połówki graniastosłupa
            new VertexPositionTexture(new Vector3(0.0f, -0.0625f, -0.0625f), new Vector2(0, 1)),
            new VertexPositionTexture(new Vector3(1.0f, -0.0625f, -0.0625f), new Vector2(1, 1)),
            new VertexPositionTexture(new Vector3(0.0f, -0.0625f, 0.0625f), new Vector2(0, 0)),
            new VertexPositionTexture(new Vector3(1.0f, -0.0625f, 0.0625f), new Vector2(1, 0)),

            // Czworokąt górny pierwszej połówki graniastosłupa
            new VertexPositionTexture(new Vector3(0.0f, 0.0625f, -0.0625f), new Vector2(0, 1)),
            new VertexPositionTexture(new Vector3(1.0f, 0.0625f, -0.0625f), new Vector2(1, 1)),
            new VertexPositionTexture(new Vector3(0.0f, 0.0625f, 0.0625f), new Vector2(0, 0)),
            new VertexPositionTexture(new Vector3(1.0f, 0.0625f, 0.0625f), new Vector2(1, 0)),

            // Boki - Wydłużone ściany pierwszej połówki graniastosłupa
            new VertexPositionTexture(new Vector3(0.0f, -0.0625f, -0.0625f), new Vector2(0, 1)),
            new VertexPositionTexture(new Vector3(1.0f, -0.0625f, -0.0625f), new Vector2(1, 1)),
            new VertexPositionTexture(new Vector3(0.0f, 0.0625f, -0.0625f), new Vector2(0, 0)),
            new VertexPositionTexture(new Vector3(1.0f, 0.0625f, 0.0625f), new Vector2(1, 0)),
            };
        }

        protected override void Update(GameTime gameTime)
        {
            // Obracaj graniastosłupy
            KeyboardState keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Right) || keyboardState.IsKeyDown(Keys.W)) angleY += 0.02f;
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



            // Ustaw macierze World, View i Projection dla strzałek
            // Ustaw macierze World, View i Projection dla strzałek
            world = Matrix.CreateTranslation(new Vector3(0.0f, 0.0f, 0.0f)); // Domyślna pozycja w punkcie (0, 0, 0)
            world2 = Matrix.CreateTranslation(new Vector3(0.0f, 0.0f, 0.0f)); // Przesunięcie o 4.0f wzdłuż osi X
            world3 = Matrix.CreateTranslation(new Vector3(0.0f, 0.0f, 0.0f)); // Domyślna pozycja w punkcie (0, 0, 0)
            world4 = Matrix.CreateTranslation(new Vector3(0.0f, 0.0f, 0.0f)); // Domyślna pozycja w punkcie (0, 0, 0)


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


            Matrix translationMatrix = Matrix.CreateTranslation(new Vector3(4.0f, 0.0f, 0.0f)); // Przesunięcie na pozycję drugiego graniastosłupa

            //Ustaw macierze dla T i G

            view2 = Matrix.CreateRotationZ(secondArmUpDown) * translationMatrix * view2;
            view3 = Matrix.CreateRotationZ(secondArmUpDown) * translationMatrix * view3;
            view4 = Matrix.CreateRotationZ(secondArmUpDown) * translationMatrix * view4;

            //Ustaw macierze dla Y i H
            view3 = Matrix.CreateRotationY(thirdArmUpDown) * translationMatrix * view3;
            view4 = Matrix.CreateRotationY(-thirdArmUpDown) * translationMatrix * view4;


            DrawPrism(vertices, indices, basicEffect, view, proj, world);
            DrawPrism(vertices2, indices, basicEffect2, view2, proj2, world2);
            DrawPrism(vertices3, indices, basicEffect3, view3,proj3, world3); // Rysuj trzeci graniastosłup z basicEffect3
            DrawPrism(vertices4, indices, basicEffect4, view4,proj4, world4);

            RasterizerState rs = new RasterizerState();
            rs.CullMode = CullMode.None; // Ustawienie cull mode na None, aby było widać całe sześciany
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
