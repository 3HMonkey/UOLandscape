using ImGuiNET;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Text;
using UOLandscape.Configuration;

namespace UOLandscape.UI.Components
{
    class SettingsComponent 
    {
        
        public static bool IsActive = true;
        private static string _currentPath
        {
            get => ConfigurationSettings.GlobalSettings.UOPath;
            set => ConfigurationSettings.GlobalSettings.UOPath = value;
        }

        private static string _inputText = _currentPath;
       
        public static bool Show(uint dockspaceID)
        {
            ImGui.SetNextWindowDockID(dockspaceID, ImGuiCond.FirstUseEver);
            if( ImGui.Begin("Settings", ref IsActive) )
            {
                ImGui.TextUnformatted("Ultima Online Path");
                ImGui.InputText("##PathBox", ref _inputText, 100);


                if( ConfigurationSettings.GlobalSettings.UOPath != null )
                {
                    ImGui.TextUnformatted($"{ConfigurationSettings.GlobalSettings.UOPath}");
                }
                else
                {
                    ImGui.Text("*");
                }

                if(ImGui.Button("Save"))
                {
                    ConfigurationSettings.GlobalSettings.UOPath = _inputText;
                    ConfigurationSettings.GlobalSettings.Save();
                    
                }
                ImGui.SameLine();
                if( ImGui.Button("Initialize") )
                {
                    UOLandscape.Client.Client.Load();
                    IsActive = false;
                }

                ImGui.End();
                return true;
            }
            return false;
        }
    }

}

