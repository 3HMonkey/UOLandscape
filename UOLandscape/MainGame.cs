using ImGuiNET;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Microsoft.Extensions.Logging;
using UOLandscape.Configuration;
using UOLandscape.UI;
using UOLandscape.UI.Components;
using Num = System.Numerics;

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
            if (_uiService.SettingsWindow.IsActive)
            {
                _uiService.SettingsWindow.Show(0);
            }

            if (!_uiService.SettingsWindow.IsActive)
            {
                // Menu
                if (ImGui.BeginMainMenuBar())
                {
                    if (ImGui.BeginMenu("Menu"))
                    {
                        //if( ImGui.MenuItem("New", "Ctrl+N", false, true) ) NewProjectWindow.IsActive = !NewProjectWindow.IsActive;
                        ImGui.EndMenu();
                    }

                    if (ImGui.BeginMenu("Options"))
                    {
                        if (ImGui.MenuItem("Settings", null, false, true))
                        {
                            _uiService.SettingsWindow.ToggleActive();

                        }
                        //ImGui.MenuItem("New", "Ctrl+N", true, show_test_window);
                        //ImGui.MenuItem("New", "Ctrl+N", true, show_test_window);
                        //ImGui.MenuItem("New", "Ctrl+N", true, show_test_window);
                        //ImGui.MenuItem("New", "Ctrl+N", true, show_test_window);
                        //ImGui.MenuItem("New", "Ctrl+N", true, show_test_window);
                        //ImGui.MenuItem("New", "Ctrl+N", true, show_test_window);

                        ImGui.EndMenu();
                    }

                    if (ImGui.BeginMenu("Help"))
                    {
                        if (ImGui.MenuItem("About", null, false, true))
                        {
                            _uiService.AboutWindow.ToggleActive();
                        }
                        //ImGui.MenuItem("New", "Ctrl+N", true, show_test_window);
                        //ImGui.MenuItem("New", "Ctrl+N", true, show_test_window);
                        //ImGui.MenuItem("New", "Ctrl+N", true, show_test_window);
                        ImGui.EndMenu();
                    }

                    if (ImGui.BeginMenu("View"))
                    {
                        if (ImGui.MenuItem("Info Box", null, _uiService.InfoOverlayWindow.IsActive, true))
                        {
                           _uiService.InfoOverlayWindow.ToggleActive();
                        }
                        if (ImGui.MenuItem("Tools", null, _uiService.ToolsWindow.IsActive, true))
                        {
                            _uiService.ToolsWindow.ToggleActive();
                        }
                        ImGui.EndMenu();
                    }

                    ImGui.EndMainMenuBar();
                }

                if (_uiService.DockSpaceWindow.IsActive)
                {
                    _uiService.DockSpaceWindow.Show(MainDockspaceID);
                }

                if (_uiService.InfoOverlayWindow.IsActive)
                {
                    _uiService.InfoOverlayWindow.Show(0);
                }

                if (_uiService.ToolsWindow.IsActive)
                {
                    _uiService.ToolsWindow.Show(0);
                }

                if (_uiService.NewProjectWindow.IsActive)
                {
                    _uiService.NewProjectWindow.Show(MainDockspaceID);
                }

                if (_uiService.AboutWindow.IsActive)
                {
                    _uiService.AboutWindow.Show(MainDockspaceID);
                }
            }


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
            for (var pixel = 0; pixel < textureData.Length; pixel++)
            {
                textureData[pixel] = paint(pixel);
            }

            texture.SetData(textureData);
            return texture;
        }
    }
}