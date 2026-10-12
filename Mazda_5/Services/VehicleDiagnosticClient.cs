using System.Diagnostics;
using System.IO.Ports;
using System.Text;

namespace mazda_5.Services
{
    public class VehicleDiagnosticClient : IDisposable
    {
        private readonly SerialPort _port;

        public VehicleDiagnosticClient(string portName, int baudRate)
        {
            _port = new SerialPort(portName, baudRate)
            {
                Parity = Parity.None,
                DataBits = 8,
                StopBits = StopBits.One,
                Handshake = Handshake.None,
                NewLine = "\r",
                ReadTimeout = 500,
                WriteTimeout = 500
            };
        }

        public void Open()
        {
            if (_port.IsOpen)
                return;

            try
            {
                _port.Open();
            }
            catch (UnauthorizedAccessException)
            {
                Thread.Sleep(200);
                _port.Open();
            }
        }

        public string Send(string command, int timeout = 1000)
        {
            

            if (!_port.IsOpen)
                return "Error: Port is not open.";

            _port.DiscardInBuffer();
            _port.DiscardOutBuffer();

            _port.WriteLine(command);

            var sw = Stopwatch.StartNew();
            string buffer = "";

            while (sw.ElapsedMilliseconds < timeout)
            {
                try
                {
                    buffer += _port.ReadExisting();
                }
                catch { }

                if (buffer.Contains(">"))
                    break;

                Thread.Sleep(50);
            }

            return CleanResponse(buffer);
        }

        public string Read()
        {

            var buffer = new StringBuilder();

            // Czekamy na odpowiedź z adaptera
            DateTime timeout = DateTime.Now.AddMilliseconds(500);

            while (DateTime.Now < timeout)
            {
                try
                {
                    string chunk = _port.ReadExisting();

                    if (!string.IsNullOrEmpty(chunk))
                    {
                        buffer.Append(chunk);

                        // STN/ELM kończy odpowiedź znakiem '>'
                        if (chunk.Contains(">"))
                            break;
                    }
                }
                catch
                {
                    break;
                }

                Thread.Sleep(10);
            }

            // Czyszczenie odpowiedzi
            string response = buffer.ToString()
                .Replace("\r", "")
                .Replace("\n", "")
                .Replace("\0", "")
                .Trim();

            return response;
        }


        public void SafeClose()
        {
            try
            {
                if (_port.IsOpen)
                    _port.Close();
            }
            catch { }
        }


        public void Dispose()
        {
            SafeClose();
            _port.Dispose();
        }

        private string CleanResponse(string raw)
        {
            if (string.IsNullOrWhiteSpace(raw))
                return "";

            // Zamień CR na nowe linie
            var lines = raw
                .Replace("\r", "\n")
                .Split('\n', StringSplitOptions.RemoveEmptyEntries)
                .Select(l => l.Trim())
                .ToList();

            if (lines.Count == 0)
                return "";

            // Usuń echo komendy (pierwsza linia)
            if (lines.Count > 1 && lines[0].Length <= 6)
                lines.RemoveAt(0);

            // Usuń prompt '>'
            lines = lines.Where(l => l != ">").ToList();

            return lines.Count > 0 ? lines[0] : "";
        }
    }
}