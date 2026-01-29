using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace MemoryHelper
{
    public class MemHelpL4D2
    {

        #region imports

        [DllImport("kernel32.dll")]
        static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, [Out] byte[] lpBuffer, int dwSize, IntPtr lpNumberOfBytesRead);

        [DllImport("kernel32.dll")]
        static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, int dwSize, IntPtr lpNumberOfBytesWritten);

        #endregion

        #region procstuff
        public Process proc;
        public Process GetProcess(string procname)
        {
            proc = Process.GetProcessesByName(procname)[0];
            return proc;
        }
        public IntPtr GetModuleBase(string modulename)
        {
            if (modulename.Contains(".exe"))
                return proc.MainModule.BaseAddress;

            foreach (ProcessModule module in proc.Modules)
            {
                if (module.ModuleName == modulename)
                    return module.BaseAddress;
            }
            return IntPtr.Zero;
        }
        #endregion

        #region readpointer
        public IntPtr ReadPointer(IntPtr addy)
        {
            byte[] buffer = new byte[4];
            ReadProcessMemory(proc.Handle, addy, buffer, buffer.Length, IntPtr.Zero);
            return new IntPtr(BitConverter.ToInt32(buffer, 0));
        }

        public IntPtr ReadPointer(IntPtr addy, int offset)
        {
            byte[] buffer = new byte[4];
            ReadProcessMemory(proc.Handle, IntPtr.Add(addy, offset), buffer, buffer.Length, IntPtr.Zero);

            return new IntPtr(BitConverter.ToInt32(buffer, 0));
        }
        #endregion 

        #region readbytes
        public byte[] ReadBytes(IntPtr addy, int bytes)
        {
            byte[] buffer = new byte[bytes];
            ReadProcessMemory(proc.Handle, addy, buffer, buffer.Length, IntPtr.Zero);
            return buffer;
        }
        public byte[] ReadBytes(IntPtr addy, int offset, int bytes)
        {
            byte[] buffer = new byte[bytes];
            ReadProcessMemory(proc.Handle, IntPtr.Add(addy, offset), buffer, buffer.Length, IntPtr.Zero);
            return buffer;
        }
        public IntPtr[] ReadPointerArray(IntPtr address, int count)
        {
            byte[] buffer = new byte[count * 4];
            ReadProcessMemory(proc.Handle, address, buffer, buffer.Length, IntPtr.Zero);

            IntPtr[] pointers = new IntPtr[count];
            for (int i = 0; i < count; i++)
            {
                pointers[i] = new IntPtr(BitConverter.ToInt32(buffer, i * 4));
            }
            return pointers;
        }
        #endregion

        #region write 
        public bool WriteBytes(IntPtr address, byte[] newbytes)
        {
            return WriteProcessMemory(proc.Handle, address, newbytes, newbytes.Length, IntPtr.Zero);
        }
        public bool WriteBytes(IntPtr address, int offset, byte[] newbytes)
        {
            return WriteProcessMemory(proc.Handle, IntPtr.Add(address, offset), newbytes, newbytes.Length, IntPtr.Zero);
        }

        public bool WriteInt32(IntPtr address, int offset, int value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            return WriteProcessMemory(proc.Handle, IntPtr.Add(address, offset), bytes, bytes.Length, IntPtr.Zero);
        }

        public bool WriteByte(IntPtr address, int offset, byte value)
        {
            byte[] bytes = new byte[] { value };
            return WriteProcessMemory(proc.Handle, IntPtr.Add(address, offset), bytes, bytes.Length, IntPtr.Zero);
        }
        #endregion
    }
}