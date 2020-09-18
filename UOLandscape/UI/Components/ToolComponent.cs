using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Text;
using UOLandscape.Configuration;

namespace UOLandscape.UI.Components
{
    class ToolComponent
    {
        public static bool IsActive = true;

        public static bool Show()
        {
            ImGui.SetNextWindowSize(new System.Numerics.Vector2(100, 450));
            
            
            
            if( ImGui.Begin("Tools", ref IsActive, ImGuiWindowFlags.NoResize))
            {
                ImGui.End();
                return true;
            }
            return false;


        }
    }
}
