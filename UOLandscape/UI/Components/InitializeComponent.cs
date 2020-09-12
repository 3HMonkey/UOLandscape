using ImGuiNET;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Text;
using UOLandscape.Configuration;

namespace UOLandscape.UI.Components
{
    class InitializeComponent 
    {
        
        public static bool IsActive = true;
        
        private static string _currentPath
        {
            get => ConfigurationSettings.GlobalSettings.UOPath;
            set => ConfigurationSettings.GlobalSettings.UOPath = value;
        }

               
        public static bool Show(uint dockspaceID)
        {
            ImGui.SetNextWindowSize(new System.Numerics.Vector2(500, 90));
            ImGui.SetNextWindowDockID(dockspaceID, ImGuiCond.FirstUseEver);
            if( ImGui.Begin("Initialize", ref IsActive) )
            {
                ImGui.TextUnformatted("Ultima Online Path");
                
                if( ConfigurationSettings.GlobalSettings.UOPath != null )
                {
                    ImGui.TextUnformatted($"{ConfigurationSettings.GlobalSettings.UOPath}");
                }
                else
                {
                    ImGui.Text("*");
                }

                if(ImGui.Button("Initialize"))
                {
                    IsActive = false;
                }

                ImGui.End();
                return true;
            }
            return false;
        }
    }

}

