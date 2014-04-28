using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simulator_PIC16F84.Instruktionen;

namespace Simulator_PIC16F84
{
    public class ProgramMemoryByte
    {
        private int value;

        public ProgramMemoryByte()
        {
            value = 0;
        }

        public int Value {
            get { return value; }
            set { this.value = value; }
        }

        public void DecodeInstruction(WorkingRegister W, ProgramCounter PC)
        {
            int k;
            int f;
            int b;
            bool d;

            //ADDWF Instruktion
            if ((value & (int)0x3F00) == (int)0x0700)
            {
                ExtractFileRegisterAndDestinationBit(out f, out d);
                ADDWF Operation = new ADDWF(f, d, W);
            }

            //ANDWF Instruktion
            if ((value & (int)0x3F00) == (int)0x0500)
            {
                ExtractFileRegisterAndDestinationBit(out f, out d);
                ANDWF Operation = new ANDWF(f, d, W);
            }

            ////CLRF Instruktion
            //if ((value & (int)0x3F80) == (int)0x0180)
            //{
            //    f = value & (int)0x7F;
            //    CLRF Operation = new CLRF(f);
            //}

            ////CLRW Instruktion
            //if ((value & (int)0x3F80) == (int)0x0100)
            //{
            //    CLRW Operation = new CLRW(W);
            //}

            ////COMF Instruktion
            //if ((value & (int)0x3F00) == (int)0x0900)
            //{
            //    ExtractFileRegisterAndDestinationBit(out f, out d);
            //    COMF Operation = new COMF(f, d);
            //}

            ////DECF Instruktion
            //if ((value & (int)0x3F00) == (int)0x0300)
            //{
            //    ExtractFileRegisterAndDestinationBit(out f, out d);
            //    DECF Operation = new DECF(f, d);
            //}

            ////DECFSZ Instruktion
            //if ((value & (int)0x3F00) == (int)0x0B00)
            //{
            //    ExtractFileRegisterAndDestinationBit(out f, out d);
            //    DECFSZ Operation = new DECFSZ(f, d);
            //}

            ////INCF Instruktion
            //if ((value & (int)0x3F00) == (int)0x0A00)
            //{
            //    ExtractFileRegisterAndDestinationBit(out f, out d);
            //    INCF Operation = new INCF(f, d);
            //}

            ////INCFSZ Instruktion
            //if ((value & (int)0x3F00) == (int)0x0F00)
            //{
            //    ExtractFileRegisterAndDestinationBit(out f, out d);
            //    INCFSZ Operation = new INCFSZ(f, d);
            //}

            ////IORWF Instruktion
            //if ((value & (int)0x3F00) == (int)0x0400)
            //{
            //    ExtractFileRegisterAndDestinationBit(out f, out d);
            //    IORWF Operation = new IORWF(f, d);
            //}

            ////MOVF Instruktion
            //if ((value & (int)0x3F00) == (int)0x0800)
            //{
            //    f = value & (int)0x7F;
            //    if ((value & 0x80) == 0x80)
            //        d = true;
            //    else
            //        d = false;
            //    MOVF Operation = new MOVF(f, d);
            //}

            ////MOVWF Instruktion
            //if ((value & (int)0x3F80) == (int)0x0080)
            //{
            //    f = value & (int)0x7F;
            //    MOVWF Operation = new MOVWF(f);
            //}

            //NOP Instruktion
            if ((value & (int)0x3F9F) == (int)0x0000)
            {
                NOP Operation = new NOP();
            }

            ////RLF Instruktion
            //if ((value & (int)0x3F00) == (int)0x0D00)
            //{
            //    ExtractFileRegisterAndDestinationBit(out f, out d);
            //    RLF Operation = new RLF(f, d);
            //}

            ////RRF Instruktion
            //if ((value & (int)0x3F00) == (int)0x0C00)
            //{
            //    ExtractFileRegisterAndDestinationBit(out f, out d);
            //    RRF Operation = new RRF(f, d);
            //}

            ////SUBWF Instruktion
            //if ((value & (int)0x3F00) == (int)0x0200)
            //{
            //    ExtractFileRegisterAndDestinationBit(out f, out d);
            //    SUBWF Operation = new SUBWF(f, d);
            //}
            ////SWAPF Instruktion
            //if ((value & (int)0x3F00) == (int)0x0E00)
            //{
            //    ExtractFileRegisterAndDestinationBit(out f, out d);
            //    SWAPF Operation = new SWAPF(f, d);
            //}

            ////XORWF Instruktion
            //if ((value & (int)0x3F00) == (int)0x0600)
            //{
            //    ExtractFileRegisterAndDestinationBit(out f, out d);
            //    XOWRF Operation = new XORWF(f, d);
            //}

            //BCF Instruktion
            if ((value & (int)0x3C00) == (int)0x1000)
            {
                ExtractFileRegisterAndBitAddress(out f, out b);
                BCF Operation = new BCF(f, b, W);
            }

            //BSF Instruktion
            if ((value & (int)0x3C00) == (int)0x1400)
            {
                ExtractFileRegisterAndBitAddress(out f, out b);
                BSF Operation = new BSF(f, b, W);
            }

            //BTFSC Instruktion
            if ((value & (int)0x3C00) == (int)0x1800)
            {
                ExtractFileRegisterAndBitAddress(out f, out b);
                BTFSC Operation = new BTFSC(f, b, PC, W);
            }

            //BTFSS Instruktion
            if ((value & (int)0x3C00) == (int)0x1C00)
            {
                ExtractFileRegisterAndBitAddress(out f, out b);
                BTFSS Operation = new BTFSS(f, b, W);
            }

            //ADDLW Instruktion
            if ((value & (int)0x3E00) == (int)0x3E00)
            {
                k = GetLiteral();
                ADDLW Operation = new ADDLW(k, W);
            }

            ////ANDLW Instruktion
            //if ((value & (int)0x3F00) == (int)0x3900)
            //{
            //    k = GetLiteral();
            //    ANDLW Operation = new ANDLW(k, W);
            //}

            //CALL Instruktion
            if ((value & (int)0x3800) == (int)0x2000)
            {
                k = value & (int)0x7FF;
                CALL Operation = new CALL(k);
            }

            ////CLRWDT Instruktion
            //if ((value & (int)0xFFFF) == (int)0x0064)
            //{
            //    CLRWDT Operation = new CLRWDT();
            //}

            ////GOTO Instruktion
            //if ((value & (int)0x3800) == (int)0x2800)
            //{
            //    k = value & (int)0x7FF;
            //    GOTO Operation = new GOTO(k);
            //}

            ////IORLW Instruktion
            //if ((value & (int)0x3F00) == (int)0x3800)
            //{
            //    k = GetLiteral();
            //    IORLW Operation = new IORLW(k, W);
            //}

            ////MOVLW Instruktion
            //if ((value & (int)0x3C00) == (int)0x3000)
            //{
            //    k = GetLiteral();
            //    MOVLW Operation = new MOVLW(k, W);
            //}

            ////RETFIE Instruktion
            //if ((value & (int)0x3FFF) == (int)0x0009)
            //{
            //    RETFIE Operation = new RETFIE();
            //}

            ////RETLW Instruktion
            //if ((value & (int)0x3C00) == (int)0x3400)
            //{
            //    k = GetLiteral();
            //    RETLW Operation = new RETLW(k);
            //}

            ////RETURN Instruktion
            //if ((value & (int)0x3FFF) == (int)0x0008)
            //{
            //    RETURN Operation = new RETURN();
            //}

            ////SLEEP Instruktion
            //if ((value & (int)0x3FFF) == (int)0x0063)
            //{
            //    SLEEP Operation = new SLEEP();
            //}

            ////SUBLW Instruktion
            //if ((value & (int)0x3E00) == (int)0x3C00)
            //{
            //    k = GetLiteral();
            //    SUBLW Operation = new SUBLW(k);
            //}

            ////XORLW Instruktion
            //if ((value & (int)0x3F00) == (int)0x3A00)
            //{
            //    k = GetLiteral();
            //    XORLW Operation = new XORLW(k);
            //}
        }

        private int GetLiteral()
        {
            int k;
            k = value & (int)0xFF;
            return k;
        }

        private void ExtractFileRegisterAndBitAddress(out int f, out int b)
        {
            f = value & (int)0x7F;
            b = value & (int)0x380;
        }

        private void ExtractFileRegisterAndDestinationBit(out int f, out bool d)
        {
            f = value & (int)0x7F;
            if ((value & 0x80) == 0x80)
                d = true;
            else
                d = false;
        }

    }
}
