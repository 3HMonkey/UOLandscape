﻿using System;
using System.Collections.Generic;

namespace UOLandscape.Utility.Logging
{
    internal class Logger
    {
        private static readonly Dictionary<LogTypes, Tuple<ConsoleColor, string>> _logTypesInfo = new Dictionary<LogTypes, Tuple<ConsoleColor, string>>
        {
            {
                LogTypes.None, Tuple.Create(ConsoleColor.White, "")
            },
            {
                LogTypes.Info, Tuple.Create(ConsoleColor.Green, "  Info    ")
            },
            {
                LogTypes.Debug, Tuple.Create(ConsoleColor.DarkGreen, "  Debug   ")
            },
            {
                LogTypes.Trace, Tuple.Create(ConsoleColor.Green, "  Trace   ")
            },
            {
                LogTypes.Warning, Tuple.Create(ConsoleColor.Yellow, "  Warning ")
            },
            {
                LogTypes.Error, Tuple.Create(ConsoleColor.Red, "  Error   ")
            },
            {
                LogTypes.Panic, Tuple.Create(ConsoleColor.Red, "  Panic   ")
            }
        };

        private int _indent;

        private bool _isLogging;

        // No volatile support for properties, let's use a private backing field.
        public LogTypes LogTypes { get; set; }

        public void Start(LogFile logFile = null)
        {
            _isLogging = true;
        }

        public void Stop()
        {
            _isLogging = false;
        }

        public void Message(LogTypes logType, string text)
        {
            lock( _syncObject )
                SetLogger(logType, text);
        }

        public void NewLine()
        {
            lock( _syncObject )
                SetLogger(LogTypes.None, string.Empty);
        }

        public void Clear()
        {
            Console.Clear();
        }

        public void PushIndent()
        {
            _indent++;
        }

        public void PopIndent()
        {
            _indent--;

            if( _indent < 0 )
                _indent = 0;
        }
        private readonly object _syncObject = new object();

        private void SetLogger(LogTypes type, string text)
        {
            if( !_isLogging )
                return;

            if( (LogTypes & type) == type )
            {
                if( type == LogTypes.None )
                {
                    if( _indent > 0 )
                    {
                        Console.Write(new string('\t', _indent * 2));
                        Console.WriteLine(text);
                    }
                    else
                        Console.WriteLine(text);
                }
                else
                {
                    Console.Write($"{DateTime.Now:T} |");
                    var temp = Console.ForegroundColor;
                    Console.ForegroundColor = _logTypesInfo[type].Item1;
                    Console.Write(_logTypesInfo[type].Item2);
                    Console.ForegroundColor = temp;

                    if( _indent > 0 )
                    {
                        Console.Write("| ");
                        Console.Write(new string('\t', _indent * 2));
                        Console.WriteLine(text);
                    }
                    else
                        Console.WriteLine($"| {text}");
                }
            }
        }
    }
}
