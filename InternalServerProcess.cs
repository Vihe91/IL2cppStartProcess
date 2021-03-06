﻿using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Lavender.Systems
{
    internal static class InternalServerProcess
    {
        static uint ptr = uint.MaxValue;
        
        [DllImport("StartProcess")]
        static extern uint StartProcess(string dir, string command);
        [DllImport("StartProcess")]
        static extern int KillProcess(uint pid);

        public static void Start()
        {
            var processPath = Directory.GetCurrentDirectory() + "/Lavender.exe";

            Debug.Log($"ProcessPath:{processPath}");

            if (File.Exists(processPath))
            {
                Debug.Log($"File Exists, preping ini settings");
                Settings.SetSetting("Server_StartMap", true);

                var settings = Settings.ExportSettings();

                var args = $" -batchmode -nographics -server -logFile \"server_log.txt\" -ini ~{settings}~";
                Debug.Log($"Args:{args}");
                if (ptr != uint.MaxValue)
                {
                    Debug.Log("Internal server process already exists");
                }
                else
                {
                    ptr = StartProcess(Directory.GetCurrentDirectory(), processPath + args);
                    Debug.Log($"pid:{ptr}");
                }
            }
            else
            {
                Debug.Log($"File doesnt exist");
            }
        }

        public static void Stop()
        {
            Debug.Log($"killing if running pid:{ptr}");

            if (ptr != uint.MaxValue)
            {
                Debug.Log($"killing");
                KillProcess(ptr);
            }
        }
    }
}
