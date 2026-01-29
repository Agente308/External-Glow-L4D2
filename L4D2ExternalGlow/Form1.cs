using MemoryHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace L4D2ExternalGlow
{
    public partial class Form1 : Form
    {
        const int team = 0xE4;
        const int health = 0xEC;
        const int lifestate = 0x144;
        const int is_incap = 0x1EA9;
        const int is_ghost = 0x1C9A;

        const int glowproperty = 0x01DC;
        const int glowtype = 0x01DC + 0x04;
        const int glowrange = 0x01DC + 0x08;
        const int glowrangemin = 0x01DC + 0x0C;
        const int glowcolor = 0x01DC + 0x10;
        const int glowflashing = 0x01DC + 0x14;
        const int server_entitylist = 0x0075E9D8;

        private volatile bool isRunning = true;

        public static bool enableGlow = true;
        public static bool enableGlowInfecteds = true;
        public static bool enableGlowGhosts = true;
        public static bool enableGlowSurvivors = true;

        private HashSet<IntPtr> trackedEntities = new HashSet<IntPtr>();
        private object trackLock = new object();

        [DllImport("user32.dll")]
        private static extern short GetAsyncKeyState(Keys vKey);

        IntPtr server;
        MemHelpL4D2 uwufy = new MemHelpL4D2();
        public Form1()
        {
            InitializeComponent();
        }

        private bool SafeDisableGlow(IntPtr buffer)
        {
            try
            {
                lock (trackLock)
                {
                    if (trackedEntities.Contains(buffer))
                    {
                        uwufy.WriteInt32(buffer, glowtype, 0);
                        trackedEntities.Remove(buffer);
                        return true;
                    }
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        private void SafeEnableGlow(IntPtr buffer, byte r, byte g, byte b)
        {
            try
            {
                uwufy.WriteInt32(buffer, glowtype, 3);
                uwufy.WriteInt32(buffer, glowrange, 999999999);
                uwufy.WriteInt32(buffer, glowrangemin, 0);
                uwufy.WriteByte(buffer, glowcolor + 0, r);
                uwufy.WriteByte(buffer, glowcolor + 1, g);
                uwufy.WriteByte(buffer, glowcolor + 2, b);
                uwufy.WriteByte(buffer, glowcolor + 3, 255);
                uwufy.WriteByte(buffer, glowflashing, 0);

                lock (trackLock)
                {
                    trackedEntities.Add(buffer);
                }
            }
            catch { }
        }

        private void ApplyGlowToEntities()
        {
            try
            {
                if (!enableGlow)
                {
                    lock (trackLock)
                    {
                        foreach (var entity in trackedEntities.ToList())
                        {
                            try
                            {
                                uwufy.WriteInt32(entity, glowtype, 0);
                            }
                            catch { }
                        }
                        trackedEntities.Clear();
                    }
                    return;
                }

                var serverEntitiesAddr = uwufy.ReadPointer(server, server_entitylist);
                if (serverEntitiesAddr != IntPtr.Zero)
                {
                    var entityPointers = uwufy.ReadPointerArray(serverEntitiesAddr, 64);
                    HashSet<IntPtr> currentValidEntities = new HashSet<IntPtr>();

                    for (int i = 1; i < 64; i++)
                    {
                        try
                        {
                            var buffer = entityPointers[i];
                            if (buffer == IntPtr.Zero || buffer.ToInt32() < 0x19000)
                                continue;

                            var classNamePtr = uwufy.ReadPointer(buffer, 0x74);
                            if (classNamePtr == IntPtr.Zero)
                                continue;

                            var classNameBytes = uwufy.ReadBytes(classNamePtr, 0, 16);
                            if (classNameBytes == null || classNameBytes.Length == 0)
                                continue;

                            var className = System.Text.Encoding.ASCII.GetString(classNameBytes).Split('\0')[0];

                            if (string.IsNullOrEmpty(className) || className == "infected")
                                continue;

                            if (className != "witch" && className != "player")
                                continue;

                            var entityHealth = BitConverter.ToInt32(uwufy.ReadBytes(buffer, health, 4), 0);
                            var state = BitConverter.ToInt32(uwufy.ReadBytes(buffer, lifestate, 4), 0);

                            if (entityHealth <= 0 || state != 0)
                            {
                                SafeDisableGlow(buffer);
                                continue;
                            }

                            bool isValid = false;
                            byte r = 255, g = 255, b = 0;

                            if (className == "witch")
                            {
                                if (!enableGlowInfecteds)
                                {
                                    SafeDisableGlow(buffer);
                                    continue;
                                }

                                isValid = true;
                                r = 255; g = 0; b = 255;
                            }
                            else if (className == "player")
                            {
                                var modelPtr = uwufy.ReadPointer(buffer, 0x24C);
                                if (modelPtr == IntPtr.Zero)
                                    continue;

                                var modelBytes = uwufy.ReadBytes(modelPtr, 0, 64);
                                if (modelBytes == null || modelBytes.Length == 0)
                                    continue;

                                var modelName = System.Text.Encoding.ASCII.GetString(modelBytes).Split('\0')[0];
                                if (string.IsNullOrEmpty(modelName))
                                    continue;

                                var flags = BitConverter.ToInt32(uwufy.ReadBytes(buffer, 0xF0, 4), 0);

                                if (modelName == "models/survivors/survivor_mechanic.mdl" ||
                                    modelName == "models/survivors/survivor_producer.mdl" ||
                                    modelName == "models/survivors/survivor_gambler.mdl" ||
                                    modelName == "models/survivors/survivor_coach.mdl" ||
                                    modelName == "models/survivors/survivor_teenager.mdl" ||
                                    modelName == "models/survivors/survivor_manager.mdl" ||
                                    modelName == "models/survivors/survivor_biker.mdl" ||
                                    modelName == "models/survivors/survivor_namvet.mdl")
                                {
                                    if (!enableGlowSurvivors)
                                    {
                                        SafeDisableGlow(buffer);
                                        continue;
                                    }

                                    var ecValue = BitConverter.ToInt32(uwufy.ReadBytes(buffer, 0xEC, 4), 0);
                                    if (ecValue != 0 && flags == 512)
                                    {
                                        isValid = true;
                                        r = 0; g = 0; b = 255;
                                    }
                                }
                                else if (modelName == "models/infected/hulk.mdl" ||
                                         modelName == "models/infected/witch.mdl" ||
                                         modelName == "models/infected/smoker.mdl" ||
                                         modelName == "models/infected/spitter.mdl" ||
                                         modelName == "models/infected/jockey.mdl" ||
                                         modelName == "models/infected/hunter.mdl" ||
                                         modelName == "models/infected/boomer.mdl" ||
                                         modelName == "models/infected/boomette.mdl" ||
                                         modelName == "models/infected/charger.mdl")
                                {
                                    var ecValue = BitConverter.ToInt32(uwufy.ReadBytes(buffer, 0xEC, 4), 0);
                                    if (ecValue != 0 && flags == 512)
                                    {
                                        var isGhost = BitConverter.ToBoolean(uwufy.ReadBytes(buffer, is_ghost, 1), 0);

                                        if (isGhost)
                                        {
                                            if (!enableGlowGhosts)
                                            {
                                                SafeDisableGlow(buffer);
                                                continue;
                                            }

                                            isValid = true;
                                            r = 255; g = 165; b = 0;
                                        }
                                        else
                                        {
                                            if (!enableGlowInfecteds)
                                            {
                                                SafeDisableGlow(buffer);
                                                continue;
                                            }

                                            isValid = true;
                                            r = 255; g = 0; b = 255;
                                        }
                                    }
                                }
                            }

                            if (!isValid)
                            {
                                SafeDisableGlow(buffer);
                                continue;
                            }

                            SafeEnableGlow(buffer, r, g, b);
                            currentValidEntities.Add(buffer);
                        }
                        catch
                        {
                            continue;
                        }
                    }

                    lock (trackLock)
                    {
                        var entitiesToRemove = trackedEntities.Except(currentValidEntities).ToList();
                        foreach (var entity in entitiesToRemove)
                        {
                            try
                            {
                                uwufy.WriteInt32(entity, glowtype, 0);
                            }
                            catch { }
                        }
                        trackedEntities.RemoveWhere(e => !currentValidEntities.Contains(e));
                    }
                }
            }
            catch { }
        }

        private void ExitCheck_Tick(object sender, EventArgs e)
        {
            if (GetAsyncKeyState(Keys.F10) < 0)
            {
                Application.Exit();
            }
        }

        private void FastUpdateLoop()
        {
            while (isRunning)
            {
                try
                {
                    ApplyGlowToEntities();
                    Thread.Sleep(14);
                }
                catch
                {
                    Thread.Sleep(100);
                }
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            isRunning = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;

            GlowCHKKKK.Checked = enableGlow;
            InfectedsCHK.Checked = enableGlowInfecteds;
            GhostsCHK.Checked = enableGlowGhosts;
            SurvivorCHK.Checked = enableGlowSurvivors;

            uwufy.GetProcess("left4dead2");
            server = uwufy.GetModuleBase("server.dll");

            Thread updateThread = new Thread(FastUpdateLoop)
            {
                IsBackground = true,
                Priority = ThreadPriority.Normal
            };
            updateThread.Start();

            System.Windows.Forms.Timer exitTimer = new System.Windows.Forms.Timer();
            exitTimer.Interval = 100;
            exitTimer.Tick += ExitCheck_Tick;
            exitTimer.Start();
        }


        private void GlowCHKKKK_CheckedChanged(object sender, EventArgs e)
        {
            enableGlow = GlowCHKKKK.Checked;
        }

        private void InfectedsCHK_CheckedChanged(object sender, EventArgs e)
        {
            enableGlowInfecteds = InfectedsCHK.Checked;
        }

        private void GhostsCHK_CheckedChanged(object sender, EventArgs e)
        {
            enableGlowGhosts = GhostsCHK.Checked;
        }

        private void SurvivorCHK_CheckedChanged(object sender, EventArgs e)
        {
            enableGlowSurvivors = SurvivorCHK.Checked;
        }
    }
}