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
            ImGui.SetNextWindowDockID(ImGui.GetID("MyDockSpace"), ImGuiCond.FirstUseEver);
            ImGui.SetNextWindowSize(new System.Numerics.Vector2(560, 450));
            if( ImGui.Begin("About", ref IsActive) )
            {
                ImGui.Text($@"
UOLandscaper {UOLandscapeEnvironment.Version.ToString()}

UOLandscaper is a modern landscape editor and creator tool for Ultima Online. 
This editor helps you to easily develop maps and content for your game.

AWARE: THIS IS ALPHA STAGE => NOT YET RUNNABLE AND / OR STABLE 

This tool is inspired and uses machanisms of:  
ClassicUO https://github.com/andreakarasho/ClassicUO  
ModernUO https://github.com/modernuo/ModernUO  
OpenUO / UltimaSDK https://github.com/jeffboulanger/OpenUO  


License Information
Copyright(C) 2020 - 3HMonkey

This program is free software: you can redistribute it and / or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.If not, see < https://www.gnu.org/licenses/>."
                                );
                ImGui.End();
                return true;
            }
            return false;
        }
    }
}
