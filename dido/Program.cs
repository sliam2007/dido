using Automation.BDaq;

namespace DO_StaticDO
{
    class StaticDO
    {


        static void Main(string[] args)
        {
            // Create device DigitalOutout object
            InstantDoCtrl instantDoCtrl = new InstantDoCtrl();

            //string deviceDescription = "DemoDevice,BID#0";
            string deviceDescription = "USB-4761,BID#0";
            byte[] bufferForWriting = new byte[1]; 
            byte[] bufferForReading = new byte[1];

            int startPort = 0;
            int portCount = 1;

            ErrorCode errorCode = ErrorCode.Success;

            try
            {
                Console.WriteLine($"Start...");

                // Select device 
                instantDoCtrl.SelectedDevice = new DeviceInformation(deviceDescription);

                bufferForWriting[0] = 15; // (bits 0-3 = decimal 15, 1-4 channels)

                // write data to device from bufferForWriting
                errorCode = instantDoCtrl.Write(0, 1, bufferForWriting);
                
                Thread.Sleep(2000);

                // read data from device to bufferForReading
                instantDoCtrl.Read(startPort, portCount, bufferForReading);

                Console.WriteLine($"Port state is : {bufferForReading[0]}");

                // all channels off
                bufferForWriting[0] = 0;

                // write data to device from bufferForWriting
                errorCode = instantDoCtrl.Write(0, 1, bufferForWriting);

                // channel 1 is ON (bit 0 = decimal 1)
                bufferForWriting[0] = 1;
                for (int i = 1; i < 9; i++)
                {

                    // write data to device from bufferForWriting
                    errorCode = instantDoCtrl.Write(0, 1, bufferForWriting);
                    Thread.Sleep(1000);

                    // shift buffer data to change channel
                    bufferForWriting[0] = (byte)(bufferForWriting[0] << 1);
                }

                Thread.Sleep(1000);

                // all channels off
                bufferForWriting[0] = 0;

                // write data to device from bufferForWriting
                errorCode = instantDoCtrl.Write(0, 1, bufferForWriting);
                for (int i = 1; i < 5; i++)
                {
                    bufferForWriting[0] = 255; // (bits 0-7 = decimal 255, all channels)

                    // write data to device from bufferForWriting
                    errorCode = instantDoCtrl.Write(0, 1, bufferForWriting);
                    Thread.Sleep(500);

                    // all channels off
                    bufferForWriting[0] = 0;

                    // write data to device from bufferForWriting
                    errorCode = instantDoCtrl.Write(0, 1, bufferForWriting);

                    Thread.Sleep(500);
                }

                // all channels off
                bufferForWriting[0] = 0;

                // write data to device from bufferForWriting
                errorCode = instantDoCtrl.Write(0, 1, bufferForWriting);
            }
            catch
            { }
        }
        static bool BioFailed(ErrorCode err)
        {
            return err < ErrorCode.Success && err >= ErrorCode.ErrorHandleNotValid;
        }
    }
}
