using ImGuiNET;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using UOLandscape.UI;
using UOLandscape.UI.Components;
using Num = System.Numerics;

namespace UOLandscape
{
    public class Main : Game
    {
        public GraphicsDeviceManager _graphics;
        private ImGUIRenderer _imGuiRenderer;
        private Texture2D _xnaTexture;
        private IntPtr _imGuiTexture;

        
        public Main()
        {

            Content.RootDirectory = "Content";
            Window.AllowUserResizing = true;
            //Window.IsBorderless = true;

            _graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = 1200,
                PreferredBackBufferHeight = 768,
                PreferMultiSampling = true,
                IsFullScreen = false,
                SupportedOrientations = DisplayOrientation.LandscapeLeft | DisplayOrientation.LandscapeRight,
                GraphicsProfile = GraphicsProfile.HiDef,


            };


            IsMouseVisible = true;

        }

        protected override void Initialize()
        {
            _imGuiRenderer = new ImGUIRenderer(this).Initialize().RebuildFontAtlas();
            ImGui.GetIO().ConfigFlags = ImGuiConfigFlags.DockingEnable;


            base.Initialize();

        }

        private readonly Texture2D[] _hues_sampler = new Texture2D[2];
        protected override void LoadContent()
        {
            // Texture loading example

            // First, load the texture as a Texture2D (can also be done using the XNA/FNA content pipeline)
            _xnaTexture = CreateTexture(GraphicsDevice, 300, 150, pixel =>
            {
                var red = (pixel % 300) / 2;
                return new Color(red, 1, 1);
            });

            // Then, bind it to an ImGui-friendly pointer, that we can use during regular ImGui.** calls (see below)
            _imGuiTexture = _imGuiRenderer.BindTexture(_xnaTexture);

            base.LoadContent();


        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(clear_color.X, clear_color.Y, clear_color.Z));



            //spriteBatch.Begin();
            //Your regular Game draw calls
            // spriteBatch.End();

            base.Draw(gameTime);

            _imGuiRenderer.BeginLayout(gameTime);
            ImGuiLayout();
            _imGuiRenderer.EndLayout();


        }


        uint dockspaceID = 0;
        private bool show_test_window = false;
        private Num.Vector3 clear_color = new Num.Vector3(114f / 255f, 144f / 255f, 154f / 255f);

        

        protected virtual void ImGuiLayout()
        {
            if( !SettingsComponent.IsActive)
            {
                // Menu
                if( ImGui.BeginMainMenuBar() )
                {
                    if( ImGui.BeginMenu("Menu") )
                    {
                        if( ImGui.MenuItem("New", "Ctrl+N", false, true) ) NewProjectComponent.IsActive = !NewProjectComponent.IsActive;
                        ImGui.EndMenu();
                    }
                    if( ImGui.BeginMenu("Options") )
                    {
                        if( ImGui.MenuItem("Settings", null, false, true) ) SettingsComponent.IsActive = !SettingsComponent.IsActive;
                        ImGui.MenuItem("New", "Ctrl+N", true, show_test_window);
                        ImGui.MenuItem("New", "Ctrl+N", true, show_test_window);
                        ImGui.MenuItem("New", "Ctrl+N", true, show_test_window);
                        ImGui.MenuItem("New", "Ctrl+N", true, show_test_window);
                        ImGui.MenuItem("New", "Ctrl+N", true, show_test_window);
                        ImGui.MenuItem("New", "Ctrl+N", true, show_test_window);

                        ImGui.EndMenu();
                    }
                    if( ImGui.BeginMenu("Help") )
                    {
                        if( ImGui.MenuItem("About", null, false, true) ) AboutWindowComponent.IsActive = !AboutWindowComponent.IsActive;
                        ImGui.MenuItem("New", "Ctrl+N", true, show_test_window);
                        ImGui.MenuItem("New", "Ctrl+N", true, show_test_window);
                        ImGui.MenuItem("New", "Ctrl+N", true, show_test_window);
                        ImGui.EndMenu();
                    }
                    ImGui.EndMainMenuBar();
                }
            }
            
            if( NewProjectComponent.IsActive ) NewProjectComponent.Show(dockspaceID);
            if( SettingsComponent.IsActive ) SettingsComponent.Show(dockspaceID);
            if( AboutWindowComponent.IsActive ) AboutWindowComponent.Show(dockspaceID);


            //if( ImGui.Begin("GameWindow") )
            //{
            //    // Using a Child allow to fill all the space of the window.
            //    // It also alows customization
            //    ImGui.BeginChild("GameRender");
            //    // Get the size of the child (i.e. the whole draw size of the windows).
            //    Num.Vector2 wsize = ImGui.GetWindowSize();
            //    // Because I use the texture from OpenGL, I need to invert the V from the UV.
            //    ImGui.Image(_imGuiTexture, wsize);
            //    ImGui.EndChild();
            //    ImGui.End();
            //}




        }


        public static Texture2D CreateTexture(GraphicsDevice device, int width, int height, Func<int, Color> paint)
        {
            //initialize a texture
            var texture = new Texture2D(device, width, height);

            //the array holds the color for each pixel in the texture
            Color[] data = new Color[width * height];
            for( var pixel = 0; pixel < data.Length; pixel++ )
            {
                //the function applies the color according to the specified pixel
                data[pixel] = paint(pixel);
            }

            //set the color
            texture.SetData(data);

            return texture;
        }


    }
}
