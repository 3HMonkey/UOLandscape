using System.Diagnostics;
using System.IO;

namespace UOLandscape.Client
{
    internal static class ClientVersionHelper
    {
        public static bool TryParseFromFile(string clientPath, out string version)
        {
            if (File.Exists(clientPath))
            {
                var fileInfo = new FileInfo(clientPath);
                var dirInfo = new DirectoryInfo(fileInfo.DirectoryName);
                if (dirInfo.Exists)
                {
                    foreach (var clientInfo in dirInfo.GetFiles("client.exe", SearchOption.TopDirectoryOnly))
                    {
                        var versionInfo = FileVersionInfo.GetVersionInfo(clientInfo.FullName);
                        if (!string.IsNullOrEmpty(versionInfo.FileVersion))
                        {
                            version = versionInfo.FileVersion.Replace(",", ".").Replace(" ", "").ToLower();
                            return true;
                        }
                    }
                }
            }

            version = null;
            return false;
        }

        public static bool IsClientVersionValid(string versionText, out ClientVersion version)
        {
            version = 0;

            if (!string.IsNullOrEmpty(versionText))
            {
                versionText = versionText.ToLower();

                var buff = versionText.ToLower().Split('.');
                if (buff.Length <= 2 || buff.Length > 4)
                {
                    return false;
                }

                if (int.TryParse(buff[0], out var major) && major >= byte.MinValue && major <= byte.MaxValue)
                {
                    var extra = 0;

                    if (int.TryParse(buff[1], out var minor) && minor >= byte.MinValue && minor <= byte.MaxValue)
                    {
                        var extraIndex = 2;
                        var build = 0;

                        if (buff.Length == 4)
                        {
                            if (!(int.TryParse(buff[extraIndex], out build) && build >= byte.MinValue && build <= byte.MaxValue))
                            {
                                return false;
                            }

                            extraIndex++;
                        }

                        var i = 0;

                        for (; i < buff[extraIndex].Length; i++)
                        {
                            var c = buff[extraIndex][i];

                            if (char.IsLetter(c))
                            {
                                extra = (byte) c;
                                break;
                            }
                        }

                        if (extra != 0)
                        {
                            if (buff[extraIndex].Length - i > 1)
                            {
                                return false;
                            }
                        }
                        else if (i <= 0)
                        {
                            return false;
                        }

                        if (!(int.TryParse(buff[extraIndex].Substring(0, i), out var numExtra) &&
                              numExtra >= byte.MinValue && numExtra <= byte.MaxValue))
                        {
                            return false;
                        }

                        if (extra != 0)
                        {
                            var start = 'a';
                            var index = 0;
                            while (start != extra && start <= 'z')
                            {
                                start++;
                                index++;
                            }

                            extra = index;
                        }

                        if (extraIndex == 2)
                        {
                            build = numExtra;
                            numExtra = extra;
                        }

                        version = (ClientVersion) (((major & 0xFF) << 24) | ((minor & 0xFF) << 16) |
                                                   ((build & 0xFF) << 8) | (numExtra & 0xFF));

                        return true;
                    }
                }
            }

            return false;
        }
    }
}