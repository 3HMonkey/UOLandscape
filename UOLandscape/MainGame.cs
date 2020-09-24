using ImGuiNET;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Microsoft.Extensions.Logging;
using UOLandscape.Configuration;
using UOLandscape.UI;
using Num = System.Numerics;
using UOLandscape.Artwork;
using System.Drawing.Imaging;

namespace UOLandscape
{
    internal sealed class MainGame : Game
    {
        private readonly ILogger<MainGame> _logger;
        private readonly IAppSettingsProvider _appSettingsProvider;
        private readonly IUIService _uiService;
        public GraphicsDeviceManager _graphics;
        private ImGuiRenderer _imGuiRenderer;
        private Texture2D _xnaTexture;
        private Texture2D _testTexture;
        private IntPtr _imGuiTexture;
        public static uint MainDockspaceID = 0;
        private readonly Texture2D[] _hues_sampler = new Texture2D[2];
        private Num.Vector3 _clearColor = new Num.Vector3(114f / 255f, 144f / 255f, 154f / 255f);

        public MainGame(ILogger<MainGame> logger, IAppSettingsProvider appSettingsProvider, IUIService uiService)
        {
            _logger = logger;
            _appSettingsProvider = appSettingsProvider;
            _uiService = uiService;

            _appSettingsProvider.Load();

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
            _logger.LogInformation("Initializing...");
            _imGuiRenderer = new ImGuiRenderer(this).Initialize().RebuildFontAtlas();
            ImGui.GetIO().ConfigFlags = ImGuiConfigFlags.DockingEnable;

            _logger.LogInformation("Initializing...Done");
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _logger.LogInformation("Loading Content...");
            _xnaTexture = CreateTexture(GraphicsDevice, 300, 150, pixel =>
            {
                var red = (pixel % 300) / 2;
                return new Color(red, 1, 1);
            });
            
             _imGuiTexture = _imGuiRenderer.BindTexture(_xnaTexture);
            
            base.LoadContent();
            _imGuiRenderer.UnbindTexture(_imGuiTexture);
            _logger.LogInformation("Loading Content...Done");
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(_clearColor.X, _clearColor.Y, _clearColor.Z));

            //spriteBatch.Begin();
            //Your regular Game draw calls
            // spriteBatch.End();

            base.Draw(gameTime);

            _imGuiRenderer.BeginLayout(gameTime);
            ImGuiLayout();
            _imGuiRenderer.EndLayout();
        }

        private void ImGuiLayout()
        {
            if( _uiService.SettingsWindow.IsVisible )
            {
                _uiService.SettingsWindow.Show(0);
            }

            if( !_uiService.SettingsWindow.IsVisible )
            {
                // Menu
                if( ImGui.BeginMainMenuBar() )
                {
                    if( ImGui.BeginMenu("Menu") )
                    {
                        //if( ImGui.MenuItem("New", "Ctrl+N", false, true) ) NewProjectWindow.IsVisible = !NewProjectWindow.IsVisible;
                        ImGui.EndMenu();
                    }

                    if( ImGui.BeginMenu("Options") )
                    {
                        if( ImGui.MenuItem("Settings", null, false, true) )
                        {
                            _uiService.SettingsWindow.ToggleVisibility();

                        }
                        //ImGui.MenuItem("New", "Ctrl+N", true, show_test_window);
                        //ImGui.MenuItem("New", "Ctrl+N", true, show_test_window);
                        //ImGui.MenuItem("New", "Ctrl+N", true, show_test_window);
                        //ImGui.MenuItem("New", "Ctrl+N", true, show_test_window);
                        //ImGui.MenuItem("New", "Ctrl+N", true, show_test_window);
                        //ImGui.MenuItem("New", "Ctrl+N", true, show_test_window);

                        ImGui.EndMenu();
                    }

                    if( ImGui.BeginMenu("Help") )
                    {
                        if( ImGui.MenuItem("About", null, false, true) )
                        {
                            _uiService.AboutWindow.ToggleVisibility();
                        }
                        //ImGui.MenuItem("New", "Ctrl+N", true, show_test_window);
                        //ImGui.MenuItem("New", "Ctrl+N", true, show_test_window);
                        //ImGui.MenuItem("New", "Ctrl+N", true, show_test_window);
                        ImGui.EndMenu();
                    }

                    if( ImGui.BeginMenu("View") )
                    {
                        if( ImGui.MenuItem("Info Box", null, _uiService.InfoOverlayWindow.IsVisible, true) )
                        {
                            _uiService.InfoOverlayWindow.ToggleVisibility();
                        }
                        if( ImGui.MenuItem("Tools", null, _uiService.ToolsWindow.IsVisible, true) )
                        {
                            _uiService.ToolsWindow.ToggleVisibility();
                        }
                        ImGui.EndMenu();
                    }

                    ImGui.EndMainMenuBar();
                }

                if( _uiService.DockSpaceWindow.IsVisible )
                {
                    _uiService.DockSpaceWindow.Show(MainDockspaceID);
                }

                if( _uiService.InfoOverlayWindow.IsVisible )
                {
                    _uiService.InfoOverlayWindow.Show(0);
                }

                if( _uiService.ToolsWindow.IsVisible )
                {
                    _uiService.ToolsWindow.Show(0);
                }

                if( _uiService.DebugWindow.IsVisible )
                {
                    _uiService.DebugWindow.Show(0);
                }

                if( _uiService.NewProjectWindow.IsVisible )
                {
                    _uiService.NewProjectWindow.Show(MainDockspaceID);
                }

                if( _uiService.AboutWindow.IsVisible )
                {
                    _uiService.AboutWindow.Show(MainDockspaceID);
                }
            }

            //##############################################################
            //##############################################################
            //TESTCASE CALLING TEST ARTWORKPROVIDER CLASS
            if( !_uiService.SettingsWindow.IsVisible )
            {
                if( ArtworkProvider.Length > 0 )
                {
                    if( ImGui.Begin("GameWindow") )
                    {
                        ImGui.BeginChild("GameRender");
                                               

                        
                        if( _testTexture == null )
                        {
                            var texture = ArtworkProvider.GetStatic(GraphicsDevice, 9);
                            _testTexture = texture;
                            var textureToRender = _imGuiRenderer.BindTexture(texture);
                            ImGui.Image(textureToRender, new Num.Vector2(44, 44));
                        }
                        else
                        {
                            var textureToRender = _imGuiRenderer.BindTexture(_testTexture);
                            ImGui.Image(textureToRender, new Num.Vector2(44, 44));
                        }
                        
                        
                        

                        ImGui.EndChild();
                        ImGui.End();
                    }
                }
            }
            //##############################################################
            //##############################################################



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

        private static Texture2D CreateTexture(GraphicsDevice device, int width, int height, Func<int, Color> paint)
        {
            var texture = new Texture2D(device, width, height);
            var textureData = new Color[width * height];
            for( var pixel = 0; pixel < textureData.Length; pixel++ )
            {
                textureData[pixel] = paint(pixel);
            }

            texture.SetData(textureData);
            return texture;
        }
    }
}