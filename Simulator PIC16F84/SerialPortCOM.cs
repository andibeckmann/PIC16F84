using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;

namespace Simulator_PIC16F84
{
    public class SerialPortCOM
    {
        private RegisterFileMap registerMap;

        /// <summary>
        /// Siehe "Active"
        /// </summary>
        private bool _Active = false;
        /// <summary>
        /// Counter zum Unterdrücken von Sendungen.
        /// Wird verwendet um eine Endlosschleife zu verhindern wenn Daten empfangen werden
        /// </summary>
        private int BlockSend = 0;

        /// <summary>
        /// Serial Port Verbindung
        /// </summary>
        private SerialPort Port;

        public SerialPortCOM(RegisterFileMap registerMap)
        {
            this.registerMap = registerMap;
        }

        /// <summary>
        /// Gibt den Zustand der Kommunikation an
        /// </summary>
        public bool Active
        {
            get
            {
                return _Active;
            }
        }

        /// <summary>
        /// Gibt alle verfügbaren Ports zurück
        /// </summary>
        public string[] Ports
        {
            get
            {
                return SerialPort.GetPortNames();
            }
        }
        /// <summary>
        /// Öffnet den Serial Port
        /// </summary>
        /// <param name="Portname">Portname (Kann aus Ports ausgelesen werden)</param>
        public void Start(string Portname)
        {
            if (Port != null && Port.IsOpen)
                return;
            Port = new SerialPort(Portname, 4800, Parity.None, 8, StopBits.One);

            Port.DataReceived += Port_DataReceived;
            try
            {
                Port.Open();
                _Active = true;
            }
            catch(UnauthorizedAccessException)
            {                
                return;
            }

           
            registerMap.getRegister(0x05).RegisterChanged += SerialCom_DataChanged;
            registerMap.getRegister(0x06).RegisterChanged += SerialCom_DataChanged;
            registerMap.getRegister(0x85).RegisterChanged += SerialCom_DataChanged;
            registerMap.getRegister(0x86).RegisterChanged += SerialCom_DataChanged;
        }

        /// <summary>
        /// Neue Daten empfangen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort Port = (SerialPort)sender;
            byte[] Buffer = new byte[5];
            if (Port.BytesToRead >= Buffer.Length)
            {
                Port.Read(Buffer, 0, Buffer.Length);
                BlockSend = 2;
                if (Buffer[4] == 0xD) // Letztes bit ist CR -> Parsen von Daten
                {
                    registerMap.getRegister(0x05).Value = (byte)((registerMap.getRegister(0x06).Value & 0xE0) | (ParseByte(Buffer, 0) & 0x1F)); // Oberen 3 bits bleiben gleich!
                    registerMap.getRegister(0x06).Value = ParseByte(Buffer, 2);
                }
            }            
        }

        /// <summary>
        /// Port A/B oder Tris A/B hat sich geändert
        /// </summary>
        /// <param name="Value"></param>
        /// <param name="Sender"></param>
        private void SerialCom_DataChanged(object Sender, int index)
        {
            if (BlockSend != 0) // Senden unterdrücken wenn grade erst empfangen wurde
            {
                BlockSend--;
                return;
            }

            try
            {
                byte[] Data = BuildPacket();
                if (Port != null && Port.IsOpen)
                {
                    Port.Write(Data, 0, Data.Length);
                }
            }
            catch(Exception)
            {

            }
        }

        /// <summary>
        /// Schließt den Port
        /// </summary>
        public void Stop()
        {
            _Active = false;
            if(Port.IsOpen)
                Port.Close();

            registerMap.getRegister(0x05).RegisterChanged -= SerialCom_DataChanged;
            registerMap.getRegister(0x06).RegisterChanged -= SerialCom_DataChanged;
            registerMap.getRegister(0x85).RegisterChanged -= SerialCom_DataChanged;
            registerMap.getRegister(0x86).RegisterChanged -= SerialCom_DataChanged;
        }

        /// <summary>
        /// Convertiert 2 byte von der Seriellen Schnittstelle zu einem byte
        /// </summary>
        /// <param name="Packet"></param>
        /// <param name="Index"></param>
        /// <returns></returns>
        private byte ParseByte(byte[] Packet, int Index)
        {
            int Temp = 0;
            Temp = (Packet[Index] - 0x30) << 4;
            Temp = Temp | (Packet[Index + 1] - 0x30);
            return (byte)Temp;
        }

        /// <summary>
        /// Baut ein Paket zum Senden
        /// </summary>
        /// <returns></returns>
        private byte[] BuildPacket()
        {
            byte[] Packet = new byte[9];

            byte Value = registerMap.getRegister(0x85).Value; // mit 0x1F verunden, da nur die ersten 6 bit gebraucht werden
            Packet[0] = (byte)((Value >> 4) + 0x30); // Oberes Halbbit
            Packet[1] = (byte)((Value & 0xF) + 0x30); // Unteres Halbbit

            Value = registerMap.getRegister(0x05).Value;  // mit 0x1F verunden, da nur die ersten 6 bit gebraucht werden
            Packet[2] = (byte)((Value >> 4) + 0x30);// Oberes Halbbit
            Packet[3] = (byte)((Value & 0xF) + 0x30); // Unteres Halbbit

            Value = registerMap.getRegister(0x86).Value;
            Packet[4] = (byte)((Value >> 4) + 0x30);// Oberes Halbbit
            Packet[5] = (byte)((Value & 0xF) + 0x30); // Unteres Halbbit

            Value = registerMap.getRegister(0x05).Value;
            Packet[6] = (byte)((Value >> 4) + 0x30);// Oberes Halbbit
            Packet[7] = (byte)((Value & 0xF) + 0x30); // Unteres Halbbit
            Packet[8] = 0xD; // CR

            Console.WriteLine("Serial: " + Encoding.ASCII.GetString(Packet));
            return Packet;
        }
    }
}
