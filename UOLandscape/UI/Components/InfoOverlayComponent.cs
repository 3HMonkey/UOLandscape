using ImGuiNET;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using UOLandscape.Configuration;
using Num = System.Numerics;

namespace UOLandscape.UI.Components
{
    class InfoOverlayComponent
    {
        static int corner = 1;
        public static bool IsActive = true;

        public static bool Show()
        {
            const float DISTANCE = 10.0f;
            
            var io = ImGuiNET.ImGui.GetIO();
            if( corner != -1 )
            {
                var viewport = ImGui.GetMainViewport();
                Num.Vector2 work_area_pos = viewport.GetWorkPos();   
                Num.Vector2 work_area_size = viewport.GetWorkSize();
                Num.Vector2 window_pos = new Num.Vector2(((corner & 1) == 1) ? (work_area_pos.X + work_area_size.X - DISTANCE) : (work_area_pos.X + DISTANCE), ((corner & 2) == 2) ? (work_area_pos.Y + work_area_size.Y - DISTANCE) : (work_area_pos.Y + DISTANCE));
                Num.Vector2 window_pos_pivot = new Num.Vector2(((corner & 1) == 1) ? 1.0f : 0.0f, ((corner & 2) == 2) ? 1.0f : 0.0f);
                ImGui.SetNextWindowPos(window_pos, ImGuiCond.Always, window_pos_pivot);
                ImGui.SetNextWindowViewport(viewport.ID);
            }
            ImGui.SetNextWindowBgAlpha(0.35f); // Transparent background
            ImGuiWindowFlags window_flags = ImGuiWindowFlags.NoDecoration | ImGuiWindowFlags.NoDocking | ImGuiWindowFlags.AlwaysAutoResize | ImGuiWindowFlags.NoSavedSettings | ImGuiWindowFlags.NoFocusOnAppearing | ImGuiWindowFlags.NoNav;
            if( corner != -1 )
                window_flags |= ImGuiWindowFlags.NoMove;
            if( ImGui.Begin("Example: Simple overlay", ref IsActive, window_flags) )
            {
                ImGui.Text(@"My example overlay");
                ImGui.Separator();
                if( ImGui.IsMousePosValid() )
                    ImGui.Text($"Mouse Position: ({io.MousePos.X},{io.MousePos.Y})");
                else
                    ImGui.Text("Mouse Position: <invalid>");
                if( ImGui.BeginPopupContextWindow() )
                {
                    if( ImGui.MenuItem("Custom", null, corner == -1) ) corner = -1;
                    if( ImGui.MenuItem("Top-left", null, corner == 0) ) corner = 0;
                    if( ImGui.MenuItem("Top-right", null, corner == 1) ) corner = 1;
                    if( ImGui.MenuItem("Bottom-left", null, corner == 2) ) corner = 2;
                    if( ImGui.MenuItem("Bottom-right", null, corner == 3) ) corner = 3;
                    if( IsActive && ImGui.MenuItem("Close") ) IsActive = false;
                    ImGui.EndPopup();
                }
                ImGui.End();
                return true;
               
            }
            return false;
            
        }
    }
}
