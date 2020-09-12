using ImGuiNET;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Text;
using UOLandscape.Configuration;

namespace UOLandscape.UI.Components
{
    class NewProjectComponent 
    {
        
        public static bool IsActive = false;
        private static string _currentPath
        {
            get => ConfigurationSettings.GlobalSettings.UOPath;
            set => ConfigurationSettings.GlobalSettings.UOPath = value;
        }

        private static string t = _currentPath;
       
        public static bool Show(uint dockspaceID)
        {
            ImGui.SetNextWindowDockID(dockspaceID, ImGuiCond.FirstUseEver);
            if( ImGui.Begin("Settings", ref IsActive) )
            {
                ImGui.TextUnformatted("Ultima Online Path");
                ImGui.InputText("##PathBox", ref t, 100);


                if( ConfigurationSettings.GlobalSettings.UOPath != null )
                {
                    ImGui.TextUnformatted($"{ConfigurationSettings.GlobalSettings.UOPath}");
                }
                else
                {
                    ImGui.Text("*");
                }

                ImGui.End();
                return true;
            }
            return false;
        }
    }

}

