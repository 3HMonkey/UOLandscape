using ImGuiNET;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using UOLandscape.Configuration;
using UOLandscape.UI.Enums;
using Num = System.Numerics;

namespace UOLandscape.UI.Components
{
    class InfoOverlayComponent
    {
        static Position _position = Position.TopRight;
        public static bool IsActive = true;

        public static bool Show()
        {
            const float DISTANCE = 10.0f;
            
            var io = ImGuiNET.ImGui.GetIO();
            if( _position != Position.Custom )
            {
                var viewport = ImGui.GetMainViewport();
                Num.Vector2 work_area_pos = viewport.GetWorkPos();   
                Num.Vector2 work_area_size = viewport.GetWorkSize();
                Num.Vector2 window_pos = new Num.Vector2(((_position & Position.TopRight) == Position.TopRight) ? (work_area_pos.X + work_area_size.X - DISTANCE) : (work_area_pos.X + DISTANCE), ((_position & Position.BottomLeft) == Position.BottomLeft) ? (work_area_pos.Y + work_area_size.Y - DISTANCE) : (work_area_pos.Y + DISTANCE));
                Num.Vector2 window_pos_pivot = new Num.Vector2(((_position & Position.TopRight) == Position.TopRight) ? 1.0f : 0.0f, ((_position & Position.BottomLeft) == Position.BottomLeft) ? 1.0f : 0.0f);
                ImGui.SetNextWindowPos(window_pos, ImGuiCond.Always, window_pos_pivot);
                ImGui.SetNextWindowViewport(viewport.ID);
            }
            ImGui.SetNextWindowBgAlpha(0.35f); // Transparent background
            ImGuiWindowFlags window_flags = ImGuiWindowFlags.NoDecoration | ImGuiWindowFlags.NoDocking | ImGuiWindowFlags.AlwaysAutoResize | ImGuiWindowFlags.NoSavedSettings | ImGuiWindowFlags.NoFocusOnAppearing | ImGuiWindowFlags.NoNav;
            if( _position != Position.Custom )
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
                    if( ImGui.MenuItem("Custom", null, _position == Position.Custom) ) _position = Position.Custom;
                    if( ImGui.MenuItem("Top-left", null, _position == Position.TopLeft) ) _position = Position.TopLeft;
                    if( ImGui.MenuItem("Top-right", null, _position == Position.TopRight) ) _position = Position.TopRight;
                    if( ImGui.MenuItem("Bottom-left", null, _position == Position.BottomLeft) ) _position = Position.BottomLeft;
                    if( ImGui.MenuItem("Bottom-right", null, _position == Position.BottomRight) ) _position = Position.BottomRight;
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
