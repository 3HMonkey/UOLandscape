using System.Diagnostics;
using System.IO;

namespace UOLandscape.Client
{
    static class ClientVersionHelper
    {
        public static bool TryParseFromFile(string clientpath, out string version)
        {
            if( File.Exists(clientpath) )
            {
                FileInfo fileInfo = new FileInfo(clientpath);

                DirectoryInfo dirInfo = new DirectoryInfo(fileInfo.DirectoryName);
                if( dirInfo.Exists )
                {
                    foreach( var clientInfo in dirInfo.GetFiles("client.exe", SearchOption.TopDirectoryOnly) )
                    {
                        FileVersionInfo versInfo = FileVersionInfo.GetVersionInfo(clientInfo.FullName);
                        if( versInfo != null && !string.IsNullOrEmpty(versInfo.FileVersion) )
                        {
                            version = versInfo.FileVersion.Replace(",", ".").Replace(" ", "").ToLower();
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

            if( !string.IsNullOrEmpty(versionText) )
            {
                versionText = versionText.ToLower();

                string[] buff = versionText.ToLower().Split(new char[1] { '.' });

                if( buff.Length <= 2 || buff.Length > 4 )
                    return false;

                if( int.TryParse(buff[0], out var major) && major >= byte.MinValue && major <= byte.MaxValue )
                {
                    int extra = 0;

                    if( int.TryParse(buff[1], out var minor) && minor >= byte.MinValue && minor <= byte.MaxValue )
                    {
                        int extra_index = 2;
                        int build = 0;

                        if( buff.Length == 4 )
                        {
                            if( !(int.TryParse(buff[extra_index], out build) && build >= byte.MinValue && build <= byte.MaxValue) )
                            {
                                return false;
                            }

                            extra_index++;
                        }

                        int i = 0;

                        for( ; i < buff[extra_index].Length; i++ )
                        {
                            char c = buff[extra_index][i];

                            if( char.IsLetter(c) )
                            {
                                extra = (byte) c;
                                break;
                            }
                        }

                        if( extra != 0 )
                        {
                            if( buff[extra_index].Length - i > 1 )
                                return false;
                        }
                        else if( i <= 0 )
                            return false;

                        if( !(int.TryParse(buff[extra_index].Substring(0, i), out int num_extra) && num_extra >= byte.MinValue && num_extra <= byte.MaxValue) )
                        {
                            return false;
                        }

                        if( extra != 0 )
                        {
                            char start = 'a';
                            int index = 0;
                            while( start != extra && start <= 'z' )
                            {
                                start++;
                                index++;
                            }

                            extra = index;
                        }

                        if( extra_index == 2 )
                        {
                            build = num_extra;
                            num_extra = extra;
                        }

                        version = (ClientVersion) (((major & 0xFF) << 24) | ((minor & 0xFF) << 16) | ((build & 0xFF) << 8) | (num_extra & 0xFF));

                        return true;
                    }
                }

            }

            return false;
        }
    }
}
