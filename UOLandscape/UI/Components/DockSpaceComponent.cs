using ImGuiNET;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using UOLandscape.Configuration;
using Num = System.Numerics;

namespace UOLandscape.UI.Components
{
    class DockSpaceComponent
    {
        public static bool IsActive = true;
        static bool opt_fullscreen = true;
        static bool opt_padding = false;
        static ImGuiDockNodeFlags dockspace_flags = ImGuiDockNodeFlags.None;

        public static void Show()
        {
            ImGuiWindowFlags window_flags =  ImGuiWindowFlags.NoDocking;
            if( opt_fullscreen )
            {
                ImGuiViewportPtr viewport = ImGui.GetMainViewport();
                ImGui.SetNextWindowPos(viewport.GetWorkPos());
                ImGui.SetNextWindowSize(viewport.GetWorkSize());
                ImGui.SetNextWindowViewport(viewport.ID);
                ImGui.PushStyleVar(ImGuiStyleVar.WindowRounding, 0.0f);
                ImGui.PushStyleVar(ImGuiStyleVar.WindowBorderSize, 0.0f);
                window_flags |= ImGuiWindowFlags.NoTitleBar | ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoMove;
                window_flags |= ImGuiWindowFlags.NoBringToFrontOnFocus | ImGuiWindowFlags.NoNavFocus;
            }
            else
            {
                dockspace_flags &= ~ImGuiDockNodeFlags.PassthruCentralNode;
            }

            // When using ImGuiDockNodeFlags_PassthruCentralNode, DockSpace() will render our background
            // and handle the pass-thru hole, so we ask Begin() to not render a background.
            if( (dockspace_flags & ImGuiDockNodeFlags.PassthruCentralNode) == ImGuiDockNodeFlags.PassthruCentralNode )
                window_flags |= ImGuiWindowFlags.NoBackground;
            // Important: note that we proceed even if Begin() returns false (aka window is collapsed).
            // This is because we want to keep our DockSpace() active. If a DockSpace() is inactive,
            // all active windows docked into it will lose their parent and become undocked.
            // We cannot preserve the docking relationship between an active window and an inactive docking, otherwise
            // any change of dockspace/settings would lead to windows being stuck in limbo and never being visible.
            if( !opt_padding )
                ImGui.PushStyleVar(ImGuiStyleVar.WindowPadding, new Num.Vector2(0.0f, 0.0f));
            ImGui.Begin("DockSpace", ref IsActive, window_flags);
            if( !opt_padding )
                ImGui.PopStyleVar();
            if( opt_fullscreen )
                ImGui.PopStyleVar(2);
            // DockSpace
            ImGuiIOPtr io = ImGui.GetIO();
            if( (io.ConfigFlags & ImGuiConfigFlags.DockingEnable) == ImGuiConfigFlags.DockingEnable )
            {
                uint dockspace_id = ImGui.GetID("MyDockSpace");
                ImGui.DockSpace(dockspace_id, new Num.Vector2(0.0f, 0.0f), dockspace_flags);
            }



        }
    }
}
