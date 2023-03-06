using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;
using System.Runtime;
using System.Runtime.InteropServices;
using Automation.Common;

namespace PNI.Comman
{
    class KeyBoardInputHandler
    {
        #region " Keyboard Handler alternative code !"
        //[StructLayout(LayoutKind.Sequential)]
        //public struct KEYBOARD_INPUT
        //{
        //    public const uint Type = 1;
        //    public ushort wVk;
        //    public ushort wScan;
        //    public uint dwFlags;
        //    public uint time;
        //    public IntPtr dwExtraInfo;
        //}

        //[StructLayout(LayoutKind.Sequential)]
        //struct MOUSEINPUT
        //{
        //    public int dx;
        //    public int dy;
        //    public uint mouseData;
        //    public uint dwFlags;
        //    public uint time;
        //    public IntPtr dwExtraInfo;
        //};

        //[StructLayout(LayoutKind.Explicit)]
        //struct KEYBDINPUT
        //{
        //    [FieldOffset(0)]
        //    public ushort wVk;
        //    [FieldOffset(2)]
        //    public ushort wScan;
        //    [FieldOffset(4)]
        //    public uint dwFlags;
        //    [FieldOffset(8)]
        //    public uint time;
        //    [FieldOffset(12)]
        //    public IntPtr dwExtraInfo;
        //};

        //[StructLayout(LayoutKind.Sequential)]
        //struct HARDWAREINPUT
        //{
        //    public uint uMsg;
        //    public ushort wParamL;
        //    public ushort wParamH;
        //};

        //[StructLayout(LayoutKind.Explicit)]
        //struct INPUT
        //{
        //    [FieldOffset(0)]
        //    public int type;
        //    [FieldOffset(4)]
        //    public MOUSEINPUT mi;
        //    [FieldOffset(4)]
        //    public KEYBDINPUT ki;
        //    [FieldOffset(4)]
        //    public HARDWAREINPUT hi;
        //};
        //[DllImport("user32.dll", SetLastError = true)]
        //static extern uint SendInput(uint nInputs, IntPtr pInput, int cbSize);

        //public KeyBoardInputHandler()
        //{ }
        //public void sendDownArrowInput(object sender, EventArgs e)
        //{
        //    //textBox1.Focus();
        //    INPUT Input = new INPUT();

        //    Input.type = 1;
        //    Input.ki.wVk = 0x41;  //ASCII for letter 'A'
        //    Input.ki.dwFlags = 0;  //Key is pressed down
        //    Input.ki.dwExtraInfo = IntPtr.Zero;
        //    IntPtr pInput;
        //    pInput = Marshal.AllocHGlobal(Marshal.SizeOf(Input));

        //    Marshal.StructureToPtr(Input, pInput, false);
        //    SendInput(1, pInput, Marshal.SizeOf(Input));
        //    Input.ki.dwFlags = 2;  //Key is released on the keyboard

        //    Marshal.StructureToPtr(Input, pInput, false);
        //    SendInput(1, pInput, Marshal.SizeOf(Input));
        //}
        #endregion

        AutoItX3Lib.AutoItX3 auto = new AutoItX3Lib.AutoItX3();
      
        
         //* !=ALT   e.g.  !a =ALT+A
         //*  += shift   e.g.  Send("!+a") would send "ALT+SHIFT+a".
         //* ^ = Ctrl  e.g. Send("^!a") would send "CTRL+ALT+a"
         //*  # = Win  e.g. Send("#r") would send Win+r 
         
         
        public void type(string input)
        {
            System.Threading.Thread.Sleep(200);
            auto.Send(input, 0);
           
            AbstractScript.throwExceptionIfUserInterruption();
        }

        #region " Dedicated Keys"

        public void Tab()
        {
            //System.Threading.Thread.Sleep(500);
            auto.Send("{TAB}", 0);
            //System.Threading.Thread.Sleep(500);
            //AbstractScript.throwExceptionIfUserInterruption();
        }
        public void TabInLoop(int length)
        {
            for (int i = 0; i < length; i++)
            {
                Tab();
                System.Threading.Thread.Sleep(100);
            }
        }

        public void Enter()
        {
            auto.Send("{ENTER}", 0);
            AbstractScript.throwExceptionIfUserInterruption();
        }
        public void Click()
        { 
                
        }       

        public void EndKey()
        {
            auto.Send("{END}", 0);
        }
        public void HomeKey()
        {
            auto.Send("{HOME}", 0);
            
        }
        public void DownArrow()
        {
            auto.Send("{DOWN}", 0);
        }
        public void UpArrow()
        {
            auto.Send("{UP}", 0);
        }
        public void Delete()
        {
            auto.Send("{DELETE}", 0);
        }
        public void UpArrowInLoop(int length)
        {
            for (int i = 0; i < length; i++)
            {
                UpArrow();
                System.Threading.Thread.Sleep(100);
            }
        }
        public void LeftArrow()
        {
            auto.Send("{LEFT}", 0);
        }
        public void RightArrow()
        {
            auto.Send("{RIGHT}", 0);
        }
        public void EscapeKey()
        {
            auto.Send("{ESCAPE}", 0);
        }

        //public void E()
        //{
        //    auto.Send("{E}", 0);
        //}

        #endregion

        #region " Function  Keys"

        public void F1()
        {
            auto.Send("{F1}", 0);   
        }
        public void F2()
        {
            auto.Send("{F2}", 0);
        }
        public void F3()
        {
            auto.Send("{F3}", 0);
        }
        public void F4()
        {
            auto.Send("{F4}", 0);
        }
        public void F5()
        {
            auto.Send("{F5}", 0);
        }
        public void F6()
        {
            auto.Send("{F6}", 0);
        }
        public void F7()
        {
            auto.Send("{F7}", 0);
        }
        public void F8()
        {
            auto.Send("{F8}", 0);
        }
        public void F9()
        {
            auto.Send("{F9}", 0);
        }
        public void F10()
        {
            auto.Send("{F10}", 0);
        }
        public void F11()
        {
            auto.Send("{F11}", 0);
        }
        public void F12()
        {
            auto.Send("{F12}", 0);
        }
        #endregion

        #region " Combination Key"
        public void ShiftEndDelete()
        {
            auto.Send("+{END}{DELETE}", 0);
        }

        public void ShiftTab()
        {
            auto.Send("+{TAB}", 0);
        }
        public void ControlPlusS()
        {
            auto.Send("^{s}", 0);
        }


        public void ControlPlusQ()
        {
            auto.Send("^{q}", 0);
        }
        //public void ControlPlusF9()
        //{
        //    auto.Send("^{F9}", 0);
        //}


        public void ShiftF12()
        {
            auto.Send("+{F12}", 0);
        }


        public void ShiftF4()
        {
            auto.Send("+{F4}", 0);
        }

        public void ShiftF5()
        {
            auto.Send("+{F5}", 0);
        }
        public void AltA()
        {
            auto.Send("!{A}", 0);
        }

        public void CtrlInsert()
        {
            auto.Send("^{Insert}", 0);
        }

        public void AltPlusS()
        {
            auto.Send("!{S}", 0);
        }

        #endregion

        #region " Other keys
        
        public void ALT()
        {
            auto.Send("{ALT}",0);
        }
       
        public void E()
        {
            auto.Send("{E}", 0);
        }
        
        public void C()
        {
            auto.Send("{C}", 0);
        }

        public void PageDown()
        {
            auto.Send("{PGDN}", 0);
        }

        #endregion

    }
}
