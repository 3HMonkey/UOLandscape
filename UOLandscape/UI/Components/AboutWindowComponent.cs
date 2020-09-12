using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Text;
using UOLandscape.Configuration;

namespace UOLandscape.UI.Components
{
    class AboutWindowComponent 
    {
        public static bool IsActive = false;
        
        public static bool Show(uint dockspaceID)
        {
            ImGui.SetNextWindowDockID(dockspaceID, ImGuiCond.FirstUseEver);
            if( ImGui.Begin("About", ref IsActive) )
            {
                ImGui.TextUnformatted("Test");
                ImGui.TextUnformatted(UOLandscapeEnvironment.Version.ToString());
                ImGui.End();
                return true;
            }
            return false;
        }
    }
}
